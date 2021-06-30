using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;

public class MarketController : MonoBehaviour
{
    public RectTransform prefab;
    public RectTransform content;
    public RectTransform marketDisplay;
    public RectTransform productDisplay;
    public GameObject playerGoldPanel;
    public GameObject cityGoldPanel;
    
    public City currentCity; // Добавить получение города из предыдущей сцены

    private void Awake()
    {
        currentCity = GameManager.currentCity;
    }

    public void GenerateMarketList()
    {
        OnReceivedModels(GetItems());
        playerGoldPanel.GetComponent<Text>().text = Economy.GetPlayerGoldAmount().ToString();
        cityGoldPanel.GetComponent<Text>().text = Economy.GetGoldAmount(currentCity.cityName).ToString();
    }

    private class LocalProduct: Product
    {
        public int CityAmount { get; set; }

        public int PlayerAmount { get; set; }
    }

    private class MarketItem
    {
        public Text product;
        public Text value;
        public Text cityAmount;
        public Text caravanAmount;

        public MarketItem(Transform rootView)
        {
            product = rootView.Find("ProductName").GetComponent<Text>();
            value = rootView.Find("Value").GetComponent<Text>();
            cityAmount = rootView.Find("CityAmount").GetComponent<Text>();
            caravanAmount = rootView.Find("CaravanAmount").GetComponent<Text>();
        }
    }

    private void InitializeMarketItem(GameObject viewGameObject, LocalProduct model)
    {
        var item = new MarketItem(viewGameObject.transform);
        item.product.text = model.Name;
        item.value.text = model.ProductValue.ToString();
        item.cityAmount.text = model.CityAmount.ToString();
        item.caravanAmount.text = model.PlayerAmount.ToString();
        viewGameObject.GetComponent<Button>().onClick.AddListener(delegate { GenerateProductDisplay(viewGameObject); });
    }

    private LocalProduct[] GetItems()
    {
        var productNames = Economy.productList;
        var results = new LocalProduct[productNames.Count];
        var count = 0;

        foreach (var product in productNames)
        {
            results[count] = new LocalProduct
            {
                Name = productNames[count],
                ProductValue = (int)Economy.GetCurrentPrice(currentCity.cityName, product),
                CityAmount = Economy.GetProductAmount(currentCity.cityName, product),
                PlayerAmount = Economy.GetPlayerProductAmount(product)
            };
            count++;
        }
        return results;
    }

    private void OnReceivedModels(LocalProduct[] models)
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        foreach (var model in models)
        {
            var instance = Instantiate(prefab.gameObject, content, false);
            InitializeMarketItem(instance, model);
        }
    }

    private void GenerateProductDisplay(GameObject marketItem)
    {
        var instance = Instantiate(productDisplay.gameObject, marketDisplay, false).transform;
        var parsedMarketItem = new MarketItem(marketItem.transform);
        instance.Find("ProductName").GetComponent<Text>().text = parsedMarketItem.product.text;
        instance.Find("CloseButton").GetComponent<Button>().onClick.AddListener(delegate
        {
            Destroy(instance.gameObject);
        });
        instance.Find("AmountPanel").Find("Value").GetComponent<Text>().text = parsedMarketItem.value.text;
        instance.Find("AmountPanel").Find("CityAmount").GetComponent<Text>().text = parsedMarketItem.cityAmount.text;
        instance.Find("AmountPanel").Find("CaravanAmount").GetComponent<Text>().text = parsedMarketItem.caravanAmount.text;
        instance.Find("InteractivePanel").Find("BuyButton").GetComponent<Button>().onClick.AddListener(delegate
        {
            TradeEvent(parsedMarketItem, instance, Economy.TradeOperationType.BuyOperation);
            Destroy(instance.gameObject);
        });
        instance.Find("InteractivePanel").Find("SellButton").GetComponent<Button>().onClick.AddListener(delegate
        {
            TradeEvent(parsedMarketItem, instance, Economy.TradeOperationType.SellOperation);
            Destroy(instance.gameObject);
        });

        void ChangeTotal(int delta)
        {
            var price = Economy.GetCurrentPrice(currentCity.cityName, parsedMarketItem.product.text);
            var amountObject = instance.Find("InteractivePanel").Find("TotalPanel").Find("Amount").GetComponent<Text>();
            amountObject.text = ((int)(int.Parse(amountObject.text) + delta * price)).ToString();
        }

        var inputPanelText = instance.Find("InteractivePanel").Find("InputPanel").Find("Text").GetComponent<Text>();
        instance.Find("InteractivePanel").Find("Minus10").GetComponent<Button>().onClick.AddListener(delegate
        {
            if ((int.Parse(inputPanelText.text) - 10) >= 0)
            {
                inputPanelText.text = (int.Parse(inputPanelText.text) - 10).ToString();   
                ChangeTotal(-10);
            }
        });
        instance.Find("InteractivePanel").Find("Minus1").GetComponent<Button>().onClick.AddListener(delegate
        {
            if ((int.Parse(inputPanelText.text) - 1) >= 0)
            {
                inputPanelText.text = (int.Parse(inputPanelText.text) - 1).ToString();
                ChangeTotal(-1);
            }
        });
        instance.Find("InteractivePanel").Find("Plus1").GetComponent<Button>().onClick.AddListener(delegate
        {
            inputPanelText.text = (int.Parse(inputPanelText.text) + 1).ToString();
            ChangeTotal(1);
        });
        instance.Find("InteractivePanel").Find("Plus10").GetComponent<Button>().onClick.AddListener(delegate
        {
            inputPanelText.text = (int.Parse(inputPanelText.text) + 10).ToString();
            ChangeTotal(10);
        });
    }

    private void TradeEvent(MarketItem marketItem, Transform productDisplayInstance, Economy.TradeOperationType type)
    {
        var tradeAmount = int.Parse(productDisplayInstance.Find("InteractivePanel").Find("InputPanel").Find("Text").GetComponent<Text>().text);
        var productType = marketItem.product.text;
        switch (type)
        {
            case Economy.TradeOperationType.BuyOperation:
                Economy.TradeOperation(type, currentCity.cityName, productType, tradeAmount);
                break;
            case Economy.TradeOperationType.SellOperation:
                Economy.TradeOperation(type, currentCity.cityName, productType, tradeAmount);
                break;
        }
        GenerateMarketList();
    }
}
