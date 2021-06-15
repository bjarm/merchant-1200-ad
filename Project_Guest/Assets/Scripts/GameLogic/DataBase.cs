using System;
using System.Collections.Generic;
using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;

public static class DataBase
{
    private const string FileName = "gameDB.db";
    private static readonly string DBPath;
    private static SqliteConnection _connection;
    private static SqliteCommand _command;

    static DataBase()
    {
        DBPath = GetDataBasePath();
    }
    
    private static string GetDataBasePath()
    {
    #if UNITY_EDITOR
        return Path.Combine(Application.streamingAssetsPath, FileName);
    #elif UNITY_STANDALONE
        string filePath = Path.Combine(Application.dataPath, FileName);
        if (!File.Exists(filePath)) UnpackDatabase(filePath);
        return filePath;
    #endif
    }
    private static void UnpackDatabase(string toPath)
    {
        string fromPath = Path.Combine(Application.streamingAssetsPath, FileName);

        var reader = new WWW(fromPath);
        while (!reader.isDone) { }

        File.WriteAllBytes(toPath, reader.bytes);
    }
    private static void OpenConnection()
    {
        _connection = new SqliteConnection("Data Source=" + DBPath);
        _command = new SqliteCommand(_connection);
        _connection.Open();
    }
    public static void CloseConnection()
    {
        _connection.Close();
        _command.Dispose();
    }
    public static void ExecuteQueryWithoutAnswer(string query)
    {
        OpenConnection();
        _command.CommandText = query;
        _command.ExecuteNonQuery();
        CloseConnection();
    }
    public static string ExecuteQueryWithAnswer(string query)
    {
        OpenConnection();
        _command.CommandText = query;
        var answer = _command.ExecuteScalar();
        CloseConnection();

        if (answer != null) return answer.ToString();
        else return null;
    }
    public static DataTable GetTable(string query)
    {
        OpenConnection();

        var adapter = new SqliteDataAdapter(query, _connection);

        var DS = new DataSet();
        adapter.Fill(DS);
        adapter.Dispose();

        CloseConnection();

        return DS.Tables[0];
    }
    public static int GetGoldAmount(string cityName) 
    {
        return int.Parse(ExecuteQueryWithAnswer($"SELECT Gold FROM CityWarehouses WHERE City = '{cityName}';"));
    }
    public static int GetProductAmount(string cityName, string product) 
    {
        return int.Parse(ExecuteQueryWithAnswer($"SELECT {product} FROM CityWarehouses WHERE City = '{cityName}';"));
    }
    public static DataTable GetProductTable(string cityName) 
    { 
        return GetTable($"SELECT * FROM CityWarehouses WHERE City = '{cityName}';"); 
    }
    public static DataTable GetPricesTable(string cityName) 
    { 
        return GetTable($"SELECT * FROM Prices WHERE City = '{cityName}';");
    }

    public static double GetCurrentPrice(string city, string product)
    {
        return double.Parse(ExecuteQueryWithAnswer($"SELECT {product} FROM Prices WHERE City = '{city}';"));
    }

    public enum TradeOperationType
    {
        BuyOperation,
        SellOperation
    }
    public static void TradeOperation(TradeOperationType type, string city, string product, int delta)
    {
        var goldAmountForProducts = (int)GetCurrentPrice(city, product) * delta;
        
        var buyCondition = (GetGoldAmount("Player") - goldAmountForProducts >= 0) && type == TradeOperationType.BuyOperation;
        var sellCondition = (GetGoldAmount(city) - goldAmountForProducts >= 0) && type == TradeOperationType.SellOperation;
        var isEnoughProductsToTrade = ((GetProductAmount(city, product) > 0) && type == TradeOperationType.BuyOperation) || 
                                      ((GetProductAmount("Player", product) > 0) && type == TradeOperationType.SellOperation);

        if ((buyCondition || sellCondition) && isEnoughProductsToTrade)
        {
            var newCityAmount = int.Parse(ExecuteQueryWithAnswer($"SELECT {product} FROM CityWarehouses WHERE City = '{city}';"));
            var newCityGoldAmount = GetGoldAmount(city);
            var newPlayerAmount = int.Parse(ExecuteQueryWithAnswer($"SELECT {product} FROM CityWarehouses WHERE City = 'Player';"));
            var newPlayerGoldAmount = GetGoldAmount("Player");
            switch (type)
            {
                case TradeOperationType.BuyOperation:
                    newCityAmount -= delta;
                    newCityGoldAmount += goldAmountForProducts;
                    newPlayerAmount += delta;
                    newPlayerGoldAmount -= goldAmountForProducts;
                    break;
                case TradeOperationType.SellOperation:
                    newCityAmount += delta;
                    newCityGoldAmount -= goldAmountForProducts;
                    newPlayerAmount -= delta;
                    newPlayerGoldAmount += goldAmountForProducts;
                    break;
            }
            ExecuteQueryWithoutAnswer($"UPDATE CityWarehouses SET {product} = '{newCityAmount}'  WHERE City = '{city}';");
            ExecuteQueryWithoutAnswer($"UPDATE CityWarehouses SET Gold = '{newCityGoldAmount}'  WHERE City = '{city}';");
            ExecuteQueryWithoutAnswer($"UPDATE CityWarehouses SET {product} = '{newPlayerAmount}'  WHERE City = 'Player';");
            ExecuteQueryWithoutAnswer($"UPDATE CityWarehouses SET Gold = '{newPlayerGoldAmount}'  WHERE City = 'Player';");
        }
    }

    public static void BuildingTransfersGoods(string city, string product, int amount)
    {
        var newCityAmount = int.Parse(ExecuteQueryWithAnswer($"SELECT {product} FROM CityWarehouses WHERE City = '{city}';")) + amount;
        ExecuteQueryWithoutAnswer($"UPDATE CityWarehouses SET {product} = '{newCityAmount}'  WHERE City = '{city}';");
    }

    public static void PopulationConsumeGoods(string city, int amountOfDays)
    {
        foreach (var product in GetProductList())
        {
            var necessity =
                double.Parse(
                    ExecuteQueryWithAnswer($"SELECT Necessity FROM Productions WHERE Product_type = '{product}';"));
            var consumption =
                double.Parse(
                    ExecuteQueryWithAnswer($"SELECT Consumption FROM Productions WHERE Product_type = '{product}';"));
            var population = GetProductAmount(city, "Population");
            var currentAmount = GetProductAmount(city, product);
            var actualAmount = (int)(currentAmount - necessity * consumption * population * amountOfDays);
            var currentGoldAmount = GetProductAmount(city, "Gold");
            var actualGoldAmount =
                (int)(currentGoldAmount + necessity * consumption * population * amountOfDays * GetCurrentPrice(city, product));
            ExecuteQueryWithoutAnswer($"UPDATE CityWarehouses SET {product} = '{actualAmount}'  WHERE City = '{city}';");
            ExecuteQueryWithoutAnswer($"UPDATE CityWarehouses SET 'Gold' = '{actualGoldAmount}'  WHERE City = '{city}';");
        }
        // Необходимо убрать возможность введения числа товара в минус
    }

    public static int GetPathLengthInDays(string fromCity, string toCity)
    {
        return int.Parse(ExecuteQueryWithAnswer($"SELECT {fromCity} FROM Roads WHERE City = '{toCity}';"));
    }

    public static int GetAmountOfProduction(string productType)
    {
        return int.Parse(ExecuteQueryWithAnswer($"SELECT Production_amount FROM Productions WHERE Product_type = '{productType}';"));
    }

    public static List<string> GetProductList()
    {
        var productList = new List<string>();
        var productTable = GetTable("SELECT Product_type FROM Productions;");
        foreach (DataRow row in productTable.Rows)
        {
            productList.Add(row[0].ToString());
        }

        return productList;
    }

    public static void UpdatePriceForCity(string city)
    {
        foreach (var product in GetProductList())
        {
            var necessity = float.Parse(ExecuteQueryWithAnswer($"SELECT Necessity FROM Productions WHERE Product_type = '{product}';"));
            var consumption = float.Parse(ExecuteQueryWithAnswer($"SELECT Consumption FROM Productions WHERE Product_type = '{product}';"));
            var producedAmount = GetAmountOfProduction(product);
            var population = GetProductAmount(city, "Population");
            var basePrice = GetCurrentPrice("Nominal", product);
            var actualPrice = (necessity * consumption * population) / producedAmount * basePrice;
            ExecuteQueryWithoutAnswer($"UPDATE Prices SET {product} = '{actualPrice}'  WHERE City = '{city}';");
        }
    }
}