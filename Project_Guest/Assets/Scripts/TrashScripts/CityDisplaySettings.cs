using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CityDisplaySettings : MonoBehaviour
{
    public Button enterCityButton;
    public Text cityNameText;

    public void Open(string cityName)
    {
        gameObject.SetActive(true);
        cityNameText.text = cityName;
        
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
    // Start is called before the first frame update
    void Start()
    {
        enterCityButton.onClick.AddListener(delegate
        {
            PlayerInfo.currentCity = cityNameText.text;
            SceneManager.LoadScene("CityScene");
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
