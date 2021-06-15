using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static City currentCity;
    public static Date currentDate = new Date(1, 1, 1240);
    
    public static GameManager instance;

    private string jsonPath = Path.Combine(Application.streamingAssetsPath, "saved_info.json");
    
    [Serializable]
    public class PlayerInfo
    {
        public string savedCityName;
        public int savedDay;
        public int savedMonth;
        public int savedYear;
    }

    void Awake()
    {
        /*
        if (instance == null)
        {
            instance = this;
            currentCity = GameObject.Find("Riga").GetComponent<City>();
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
        */

        var loadedInfo = JsonUtility.FromJson<PlayerInfo>(System.IO.File.ReadAllText(jsonPath));
        currentCity = GameObject.Find(loadedInfo.savedCityName).GetComponent<City>();
        currentDate = new Date(loadedInfo.savedDay, loadedInfo.savedMonth, loadedInfo.savedYear);

    }

    private void OnDestroy()
    {
        PlayerInfo savedInfo = new PlayerInfo();
        savedInfo.savedCityName = currentCity.cityName;
        savedInfo.savedDay = currentDate.day;
        savedInfo.savedMonth = (int)currentDate.month;
        savedInfo.savedYear = currentDate.year;
        
        System.IO.File.WriteAllText(jsonPath, JsonUtility.ToJson(savedInfo, true));
    }
}
