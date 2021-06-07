using UnityEngine;
using Mono.Data.Sqlite;
using System.Data;
using System.IO;

static class DataBase
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

    public static int GetCurrentPrice(string city, string product)
    {
        return int.Parse(ExecuteQueryWithAnswer($"SELECT {product} FROM Prices WHERE City = '{city}';"));
    }
    public static void TradeOperation(string type, string city, string product, int delta)
    {
        var goldAmountForProducts = GetCurrentPrice(city, product) * delta;
        
        var buyCondition = (GetGoldAmount("Player") - goldAmountForProducts >= 0) && type == "BUY_OPERATION";
        var sellCondition = (GetGoldAmount(city) - goldAmountForProducts >= 0) && type == "SELL_OPERATION";
        var isEnoughProductsToTrade = ((GetProductAmount(city, product) > 0) && type == "BUY_OPERATION") || 
                                      ((GetProductAmount("Player", product) > 0) && type == "SELL_OPERATION");

        if ((buyCondition || sellCondition) && isEnoughProductsToTrade)
        {
            var newCityAmount = int.Parse(ExecuteQueryWithAnswer($"SELECT {product} FROM CityWarehouses WHERE City = '{city}';"));
            var newCityGoldAmount = GetGoldAmount(city);
            var newPlayerAmount = int.Parse(ExecuteQueryWithAnswer($"SELECT {product} FROM CityWarehouses WHERE City = 'Player';"));
            var newPlayerGoldAmount = GetGoldAmount("Player");
            switch (type)
            {
                case "BUY_OPERATION":
                    newCityAmount -= delta;
                    newCityGoldAmount += goldAmountForProducts;
                    newPlayerAmount += delta;
                    newPlayerGoldAmount -= goldAmountForProducts;
                    break;
                case "SELL_OPERATION":
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
}