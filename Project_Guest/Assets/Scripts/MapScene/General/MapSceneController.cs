using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapSceneController : MonoBehaviour
{
    public GameObject mainCharacter;

    // Resources for city display
    //public GameObject cityDisplay;
    public RectTransform canvas;
    public RectTransform prefab;
    
    private void Start()
    {
        EventManager.PlayerHasArrivedInTown.Subscribe(OpenCityDisplay);     
    }

    public void OpenCityDisplay()
    {
        var instance = Instantiate(prefab.gameObject, canvas, false);
        InitializeCityDisplay(instance);
        /*
        cityDisplay.transform.Find("CityName").GetComponent<Text>().text = GameManager.currentCity.cityName;
        cityDisplay.transform.Find("EnterButton").GetComponent<Button>().onClick.AddListener(
            delegate { SceneManager.LoadScene("CityScene"); });
        cityDisplay.transform.Find("CloseButton").GetComponent<Button>().onClick.AddListener(CleanMess);
        cityDisplay.SetActive(true);
        
        void CleanMess()
        {
            cityDisplay.transform.Find("EnterButton").GetComponent<Button>().onClick.RemoveAllListeners();
            cityDisplay.transform.Find("CloseButton").GetComponent<Button>().onClick.RemoveAllListeners();
            cityDisplay.gameObject.SetActive(false);
        }*/
    }

    private void InitializeCityDisplay(GameObject cityDisplay)
    {
        cityDisplay.transform.Find("CityName").GetComponent<Text>().text = GameManager.currentCity.cityName;
        cityDisplay.transform.Find("EnterButton").GetComponent<Button>().onClick.AddListener(
            delegate { SceneManager.LoadScene("CityScene"); });
        cityDisplay.transform.Find("CloseButton").GetComponent<Button>().onClick.AddListener(CleanMess);
        
        void CleanMess()
        {
            cityDisplay.transform.Find("EnterButton").GetComponent<Button>().onClick.RemoveAllListeners();
            cityDisplay.transform.Find("CloseButton").GetComponent<Button>().onClick.RemoveAllListeners();
            Destroy(cityDisplay);
        }
    }
}
