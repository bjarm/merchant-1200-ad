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
        playerGoldPanel.GetComponent<Text>().text = DataBase.GetGoldAmount("Player").ToString();
        cityGoldPanel.GetComponent<Text>().text = DataBase.GetGoldAmount(currentCity.cityName).ToString();
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
        var cityWarehouse = DataBase.GetProductTable(currentCity.cityName);
        var playerWarehouse = DataBase.GetProductTable("Player");
        var prices = DataBase.GetPricesTable(currentCity.cityName);

        var count = cityWarehouse.Columns.Count - 3;
        var results = new LocalProduct[count];
        
        var productNames = new string[] { "Bread", "Fur", "Salt", "Wine", "Herring" };

        for (var i = 0; i < count; i++)
        {
            results[i] = new LocalProduct
            {
                Name = productNames[i],
                ProductValue = double.Parse(prices.Rows[0][i + 1].ToString()),
                CityAmount = int.Parse(cityWarehouse.Rows[0][i + 3].ToString()),
                PlayerAmount = int.Parse(playerWarehouse.Rows[0][i + 3].ToString())
            };
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
            TradeEvent(parsedMarketItem, instance, DataBase.TradeOperationType.BuyOperation);
            Destroy(instance.gameObject);
        });
        instance.Find("InteractivePanel").Find("SellButton").GetComponent<Button>().onClick.AddListener(delegate
        {
            TradeEvent(parsedMarketItem, instance, DataBase.TradeOperationType.SellOperation);
            Destroy(instance.gameObject);
        });

        void ChangeTotal(int delta)
        {
            var price = DataBase.GetCurrentPrice(currentCity.cityName, parsedMarketItem.product.text);
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

    private void TradeEvent(MarketItem marketItem, Transform productDisplayInstance, DataBase.TradeOperationType type)
    {
        var tradeAmount = int.Parse(productDisplayInstance.Find("InteractivePanel").Find("InputPanel").Find("Text").GetComponent<Text>().text);
        var productType = marketItem.product.text;
        switch (type)
        {
            case DataBase.TradeOperationType.BuyOperation:
                DataBase.TradeOperation(type, currentCity.cityName, productType, tradeAmount);
                break;
            case DataBase.TradeOperationType.SellOperation:
                DataBase.TradeOperation(type, currentCity.cityName, productType, tradeAmount);
                break;
        }
        GenerateMarketList();
    }
}
