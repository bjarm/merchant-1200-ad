using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Newtonsoft.Json;

public static class Economy
{
	// Unpacking cities data from json file to _citiesList dictionary in the class constructor
    private const string CitiesFileName = "cities.json";
    private static readonly string CitiesPath = Path.Combine(Application.streamingAssetsPath, CitiesFileName);
    private static Dictionary<string, City> _cities;
    
    // Unpacking products data from json file to _productsList dictionary in the class constructor
    private const string ProductsFileName = "products.json";
    private static readonly string ProductsPath = Path.Combine(Application.streamingAssetsPath, ProductsFileName);
    private static Dictionary<string, Product> _products;
    
    // Unpacking player data from json file to _player object in the class constructor
    private const string PlayerFileName = "player.json";
    private static readonly string PlayerPath = Path.Combine(Application.streamingAssetsPath, PlayerFileName);
    private static Player _player;

    // The number of days during which the amount of a certain product must meet the demand in order to maintain the nominal price
    private const int SettlementPlanningRatio = 7;
    // Temporary roads file
    private static Dictionary<string, CityRoads> _roads;
    
    // List with the names of all products
    public static List<string> productList;

    static Economy()
    {
	    _cities = JsonConvert.DeserializeObject<Dictionary<string, City>>(File.ReadAllText(CitiesPath));
	    _products = JsonConvert.DeserializeObject<Dictionary<string, Product>>(File.ReadAllText(ProductsPath));
	    _player = JsonConvert.DeserializeObject<Player>(File.ReadAllText(PlayerPath));
	    // Temporary
	    _roads = JsonConvert.DeserializeObject<Dictionary<string, CityRoads>>(File.ReadAllText(CitiesPath));
	    
	    productList = _products.Select(product => product.Key).ToList();
    }

    public static void SaveData()
    {
	    JsonConvert.SerializeObject(_cities, Formatting.Indented);
	    JsonConvert.SerializeObject(_player, Formatting.Indented);
    }
    
    public static int GetGoldAmount(string cityName)
    {
	    return _cities[cityName].gold;
    }

    public static int GetPlayerGoldAmount()
    {
	    return _player.gold;
    }
    
    public static void ChangePlayerGoldAmount(int delta)
    {
	    _player.gold = GetPlayerGoldAmount() + delta;
    }
    
    public static int GetProductAmount(string cityName, string productName)
    {
	    return _cities[cityName].listOfGoods[productName];
    }
    
    public static int GetPlayerProductAmount(string productName)
    {
	    return _player.listOfGoods[productName];
    }

    public static double GetCurrentPrice(string cityName, string productName)
    {
	    var city = _cities[cityName];
	    var product = _products[productName];
	    return (SettlementPlanningRatio * product.necessity * product.consumption * city.population) / city.listOfGoods[productName] * product.nominalPrice;
    }
    
    public static int GetAmountOfProduction(string productType)
    {
	    return _products[productType].productionAmount;
    }
    
    public enum TradeOperationType
    {
	    BuyOperation,
	    SellOperation
    }
    
    public static void TradeOperation(TradeOperationType type, string city, string product, int delta)
    {
	    var goldAmountForProducts = (int)GetCurrentPrice(city, product) * delta;
        
        var isEnoughGoldToBuy = (GetPlayerGoldAmount() - goldAmountForProducts >= 0) && type == TradeOperationType.BuyOperation;
        var isEnoughGoldToSell = (GetGoldAmount(city) - goldAmountForProducts >= 0) && type == TradeOperationType.SellOperation;
        var isEnoughProductsToTrade = (((GetProductAmount(city, product) - delta) >= 0) && type == TradeOperationType.BuyOperation) || 
                                      (((GetPlayerProductAmount(product) - delta) >= 0) && type == TradeOperationType.SellOperation);

        if ((isEnoughGoldToBuy || isEnoughGoldToSell) && isEnoughProductsToTrade)
        {
            var cityProductAmount = _cities[city].listOfGoods[product];
            var cityGoldAmount = GetGoldAmount(city);
            var playerProductAmount = _player.listOfGoods[product];
            var playerGoldAmount = GetPlayerGoldAmount();
            switch (type)
            {
                case TradeOperationType.BuyOperation:
	                cityProductAmount -= delta;
	                cityGoldAmount += goldAmountForProducts;
	                playerProductAmount += delta;
	                playerGoldAmount -= goldAmountForProducts;
                    break;
                case TradeOperationType.SellOperation:
	                cityProductAmount += delta;
	                cityGoldAmount -= goldAmountForProducts;
	                playerProductAmount -= delta;
	                playerGoldAmount += goldAmountForProducts;
                    break;
            }
            
            _cities[city].listOfGoods[product] = cityProductAmount;
            _cities[city].gold = cityGoldAmount;
            _player.listOfGoods[product] = playerProductAmount;
            _player.gold = playerGoldAmount;
        }
        else if (!isEnoughGoldToBuy && type == TradeOperationType.BuyOperation)
        {
            EventManager.OperationFailed.Publish("У вас недостаточно золота");
        }
        else if (!isEnoughGoldToSell && type == TradeOperationType.SellOperation)
        {
            EventManager.OperationFailed.Publish("У продавца недостаточно золота");
        }
        if (!isEnoughProductsToTrade)
        {
            EventManager.OperationFailed.Publish("Не хватает товара");
        }
    }
    
    public static void BuildingTransfersGoods(string city, string product, int amount)
    {
	    _cities[city].listOfGoods[product] += amount;
    }

    public static void PopulationConsumeGoods(string city, int amountOfDays)
    {
	    foreach (var product in _products)
	    {
		    var necessity = product.Value.necessity;
		    var consumption = product.Value.consumption;
		    var population = _cities[city].population;
		    var currentAmount = GetProductAmount(city, product.Key);
		    var actualAmount = (int)(currentAmount - necessity * consumption * population * amountOfDays);
		    if (actualAmount >= 0)
		    {
			    var currentGoldAmount = GetGoldAmount(city);
			    var actualGoldAmount =
				    (int)(currentGoldAmount + necessity * consumption * population * amountOfDays * GetCurrentPrice(city, product.Key));
			    _cities[city].listOfGoods[product.Key] = actualAmount;
			    _cities[city].gold = actualGoldAmount;
		    }
		    else
		    {
			    // А надо ли?
		    }
	    }
    }
    
    // Temporary
    public static int GetPathLengthInDays(string fromCity, string toCity)
    {
	    return _roads[fromCity].listOfRoads[toCity];
    }
    
    [Serializable]
    public class City
    { 
      public string name;
      public Dictionary<string, int> listOfGoods;
      public int gold;
      public int population;
    }
    
    [Serializable]
    public class Product
    {
	    public double nominalPrice;
	    public double necessity;
	    public double consumption;
	    public int productionAmount;
    }
    
    [Serializable]
    public class Player
    {
	    public Dictionary<string, int> listOfGoods;
	    public int gold;
    }

    // Temporary
    [Serializable]
    public class CityRoads
    {
	    public Dictionary<string, int> listOfRoads;
    }

    public static void Test()
    {
	    Debug.Log(_roads["Toropets"].listOfRoads["Polotsk"]);
    }
}
