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
    public GameObject playerGold;
    public GameObject cityGold;
    public string currentCity = "Novgorod"; // Добавить получение города из предыдущей сцены

    void Start()
    {
        GenerateMarketList();
    }

    void Update()
    {
        playerGold.GetComponent<Text>().text = DataBase.ExecuteQueryWithAnswer("SELECT Gold FROM CityWarehouses WHERE City = 'Player'; ");
        cityGold.GetComponent<Text>().text = DataBase.ExecuteQueryWithAnswer($"SELECT Gold FROM CityWarehouses WHERE City = '{currentCity}';");
    }

    public void GenerateMarketList()
    {
        OnReceivedModels(GetItems());
    }

    public class LocalProduct: Product
    {
        private int cityAmount;
        private int playerAmount;

        public int CityAmount
        {
            get { return cityAmount; }
            set { cityAmount = value; }
        }
        public int PlayerAmount
        {
            get { return playerAmount; }
            set { playerAmount = value; }
        }
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

    void InitializeMarketItem(GameObject viewGameObject, LocalProduct model)
    {
        MarketItem item = new MarketItem(viewGameObject.transform);
        item.product.text = model.Name;
        item.value.text = model.ProductValue.ToString();
        item.cityAmount.text = model.CityAmount.ToString();
        item.caravanAmount.text = model.PlayerAmount.ToString();
        viewGameObject.GetComponent<Button>().onClick.AddListener(delegate { GenerateProductDisplay(viewGameObject); });
    }

    LocalProduct[] GetItems()
    {
        DataTable cityWarehouse = DataBase.GetTable($"SELECT * FROM CityWarehouses WHERE City = '{currentCity}';");
        DataTable playerWarehouse = DataBase.GetTable("SELECT * FROM CityWarehouses WHERE City = 'Player';");
        DataTable prices = DataBase.GetTable("SELECT * FROM Prices WHERE Type = 'Current price';");

        var count = cityWarehouse.Columns.Count - 2;
        var results = new LocalProduct[count];
        var productNames = new string[] { "Bread", "Fur", "Salt", "Wine", "Herring" };

        for (int i = 0; i < count; i++)
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

    void OnReceivedModels(LocalProduct[] models)
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }

        foreach (var model in models)
        {
            var instance = Instantiate(prefab.gameObject);
            instance.transform.SetParent(content, false);
            InitializeMarketItem(instance, model);
        }
    }

    public void GenerateProductDisplay(GameObject marketItem)
    {
        MarketItem parseMarketItem = new MarketItem(marketItem.transform);
        productDisplay.Find("ProductName").GetComponent<Text>().text = parseMarketItem.product.text;
        productDisplay.Find("AmountPanel").Find("Value").GetComponent<Text>().text = parseMarketItem.value.text;
        productDisplay.Find("AmountPanel").Find("CityAmount").GetComponent<Text>().text = parseMarketItem.cityAmount.text;
        productDisplay.Find("AmountPanel").Find("CaravanAmount").GetComponent<Text>().text = parseMarketItem.caravanAmount.text;
        productDisplay.Find("BuyButton").GetComponent<Button>().onClick.AddListener(BuyEvent);
        productDisplay.gameObject.SetActive(true);
    }

    public void BuyEvent()
    {
        int productAmount = int.Parse(productDisplay.Find("InputField").GetComponent<InputField>().text);

    }
}
