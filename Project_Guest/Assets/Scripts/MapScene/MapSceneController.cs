using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapSceneController : MonoBehaviour
{
    public GameObject mainCharacter;

    // Resources for city display
    public GameObject cityDisplay;
   
    private void Awake()
    {
        EventManager.PlayerHasArrivedInTown.Subscribe(OpenCityDisplay);     
    }

    public void OpenCityDisplay()
    {
        cityDisplay.transform.Find("CityName").GetComponent<Text>().text = mainCharacter.GetComponent<Character>().location;
        cityDisplay.transform.Find("EnterButton").GetComponent<Button>().onClick.AddListener(
            delegate { SceneManager.LoadScene("CityScene"); });
        cityDisplay.transform.Find("CloseButton").GetComponent<Button>().onClick.AddListener(CleanMess);
        cityDisplay.SetActive(true);
        
        void CleanMess()
        {
            cityDisplay.transform.Find("EnterButton").GetComponent<Button>().onClick.RemoveAllListeners();
            cityDisplay.transform.Find("CloseButton").GetComponent<Button>().onClick.RemoveAllListeners();
            cityDisplay.gameObject.SetActive(false);
        }
    }
}
