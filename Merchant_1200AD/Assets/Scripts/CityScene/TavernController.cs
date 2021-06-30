using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class TavernController : MonoBehaviour
{
    public RectTransform subDisplayPrefab;
    public RectTransform mainCanvas;
    
    [Header("Tavern screen objects")]
    public RectTransform tavernDisplay;
    public GameObject rentRoomButton;
    public GameObject drinkButton;
    
    [Header("Church screen objects")]
    public RectTransform churchDisplay;
    public GameObject makeDonationButton;
    public GameObject attendDivineServiceButton;
    
    void Awake()
    {
        // Initializing of tavern screen
        rentRoomButton.GetComponent<Button>().onClick.AddListener(delegate { GenerateSubDisplay(ActionType.RentRoom, ScreenType.Tavern); });
        drinkButton.GetComponent<Button>().onClick.AddListener(delegate { GenerateSubDisplay(ActionType.Drink, ScreenType.Tavern); });
        
        // Initializing of church screen
        makeDonationButton.GetComponent<Button>().onClick.AddListener(delegate { GenerateSubDisplay(ActionType.MakeDonation, ScreenType.Church); });
        attendDivineServiceButton.GetComponent<Button>().onClick.AddListener(delegate { GenerateSubDisplay(ActionType.AttendDivineService, ScreenType.Church); });
        
        EventManager.OperationFailed.Subscribe(GenerateInfoDisplay);
    }

    private enum ActionType
    {
        RentRoom,
        Drink,
        MakeDonation,
        AttendDivineService
    }

    private enum ScreenType
    {
        Tavern,
        Church
    }
    private void GenerateSubDisplay(ActionType actionType, ScreenType screenType)
    {
        GameObject instance;
        switch (screenType)
        {
            case ScreenType.Tavern:
                instance = Instantiate(subDisplayPrefab.gameObject, tavernDisplay, false);        
                break;
            
            case ScreenType.Church:
                instance = Instantiate(subDisplayPrefab.gameObject, churchDisplay, false);        
                break;
            
            default:
                instance = Instantiate(subDisplayPrefab.gameObject, tavernDisplay, false);        
                break;
        }
        switch (actionType)
        {
            case ActionType.RentRoom:
                instance.transform.Find("Text").GetComponent<Text>().text =
                    "Стоимость проживания в этом заведении - 10 монет.\n\n Остаться на ночь?";
                instance.transform.Find("InteractivePanel").Find("YesButton").GetComponent<Button>().onClick.AddListener(delegate
                {
                    Economy.ChangePlayerGoldAmount(-10);
                    EventManager.DateChanged.Publish(1);
                    Destroy(instance);
                });
                break;
            case ActionType.Drink:
                instance.transform.Find("Text").GetComponent<Text>().text =
                    "Кружка свежего пива здесь стоит 5 монет.\n\n Заказать кружку?";
                instance.transform.Find("InteractivePanel").Find("YesButton").GetComponent<Button>().onClick.AddListener(delegate
                {
                    Economy.ChangePlayerGoldAmount(-5);
                    Destroy(instance);
                });
                break;
            case ActionType.MakeDonation:
                instance.transform.Find("Text").GetComponent<Text>().text =
                    "Сколько монет вы бы хотели пожертвовать?";
                instance.transform.Find("InteractivePanel").Find("YesButton").Find("Text").GetComponent<Text>().text =
                    "10";
                instance.transform.Find("InteractivePanel").Find("NoButton").Find("Text").GetComponent<Text>().text =
                    "50";
                instance.transform.Find("InteractivePanel").Find("OptionalButton").Find("Text").GetComponent<Text>().text =
                    "100";
                instance.transform.Find("InteractivePanel").Find("OptionalButton").gameObject.SetActive(true);
                instance.transform.Find("CloseButton").gameObject.SetActive(true);
                
                instance.transform.Find("InteractivePanel").Find("YesButton").GetComponent<Button>().onClick.AddListener(delegate
                {
                    Economy.ChangePlayerGoldAmount(-10);
                    Destroy(instance);
                });
                instance.transform.Find("InteractivePanel").Find("NoButton").GetComponent<Button>().onClick.AddListener(delegate
                {
                    Economy.ChangePlayerGoldAmount(-50);
                    Destroy(instance);
                });
                instance.transform.Find("InteractivePanel").Find("OptionalButton").GetComponent<Button>().onClick.AddListener(delegate
                {
                    Economy.ChangePlayerGoldAmount(-100);
                    Destroy(instance);
                });
                break;
            case ActionType.AttendDivineService:
                instance.transform.Find("Text").GetComponent<Text>().text =
                    "Судя по всему, сейчас не время богослужения.";
                instance.transform.Find("CloseButton").gameObject.SetActive(true);
                instance.transform.Find("InteractivePanel").gameObject.SetActive(false);
                break;
        }

        instance.transform.Find("InteractivePanel").Find("NoButton").GetComponent<Button>().onClick.AddListener(delegate
        {
            Destroy(instance);
        });
    }

    private void GenerateInfoDisplay(string text)
    {
        var instance = Instantiate(subDisplayPrefab.gameObject, mainCanvas, false);
        instance.transform.Find("Text").GetComponent<Text>().text = text;
        instance.transform.Find("CloseButton").gameObject.SetActive(true);
        instance.transform.Find("InteractivePanel").gameObject.SetActive(false);
    }
}
