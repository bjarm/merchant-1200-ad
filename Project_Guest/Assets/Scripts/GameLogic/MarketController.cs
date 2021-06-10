using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;

public class MarketController : MonoBehaviour
{
    public RectTransform prefab;
    public RectTransform content;
    public RectTransform productDisplay;
    public GameObject playerGoldPanel;
    public GameObject cityGoldPanel;

    public string currentCity = PlayerInfo.currentCity; // Добавить получение города из предыдущей сцены

    private void Start()
    {
        GenerateMarketList();
    }

    public void GenerateMarketList()
    {
        OnReceivedModels(GetItems());
        playerGoldPanel.GetComponent<Text>().text = DataBase.GetGoldAmount("Player").ToString();
        cityGoldPanel.GetComponent<Text>().text = DataBase.GetGoldAmount(currentCity).ToString();
    }

    public class LocalProduct: Product
    {
        public int CityAmount { get; set; }

        public int PlayerAmount { get; set; }
    }

    public class MarketItem
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
        var cityWarehouse = DataBase.GetProductTable(currentCity);
        var playerWarehouse = DataBase.GetProductTable("Player");
        var prices = DataBase.GetPricesTable(currentCity);

        var count = cityWarehouse.Columns.Count - 2;
        var results = new LocalProduct[count];
        var productNames = new string[] { "Bread", "Fur", "Salt", "Wine", "Herring" };

        for (var i = 0; i < count; i++)
        {
            results[i] = new LocalProduct
            {
                Name = productNames[i],
                ProductValue = double.Parse(prices.Rows[0][i + 1].ToString()),
                CityAmount = int.Parse(cityWarehouse.Rows[0][i + 2].ToString()),
                PlayerAmount = int.Parse(playerWarehouse.Rows[0][i + 2].ToString())
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

    public void GenerateProductDisplay(GameObject marketItem)
    {
        var parsedMarketItem = new MarketItem(marketItem.transform);
        productDisplay.Find("ProductName").GetComponent<Text>().text = parsedMarketItem.product.text;
        productDisplay.Find("AmountPanel").Find("Value").GetComponent<Text>().text = parsedMarketItem.value.text;
        productDisplay.Find("AmountPanel").Find("CityAmount").GetComponent<Text>().text = parsedMarketItem.cityAmount.text;
        productDisplay.Find("AmountPanel").Find("CaravanAmount").GetComponent<Text>().text = parsedMarketItem.caravanAmount.text;
        productDisplay.Find("BuyButton").GetComponent<Button>().onClick.AddListener(delegate
        {
            TradeEvent(parsedMarketItem, "BUY_OPERATION"); 
            CleanMess();
        });
        productDisplay.Find("SellButton").GetComponent<Button>().onClick.AddListener(delegate
        {
            TradeEvent(parsedMarketItem, "SELL_OPERATION"); 
            CleanMess();
        });
        productDisplay.gameObject.SetActive(true);

        void CleanMess()
        {
            productDisplay.Find("BuyButton").GetComponent<Button>().onClick.RemoveAllListeners();
            productDisplay.Find("SellButton").GetComponent<Button>().onClick.RemoveAllListeners();
            productDisplay.gameObject.SetActive(false);
        }
    }

    public void TradeEvent(MarketItem marketItem, string type)
    {
        var tradeAmount = int.Parse(productDisplay.Find("InputField").GetComponent<InputField>().text);
        var productType = marketItem.product.text;
        switch (type)
        {
            case "BUY_OPERATION":
                DataBase.TradeOperation(type, currentCity, productType, tradeAmount);
                break;
            case "SELL_OPERATION":
                DataBase.TradeOperation(type, currentCity, productType, tradeAmount);
                break;
        }
        GenerateMarketList();
    }
}
