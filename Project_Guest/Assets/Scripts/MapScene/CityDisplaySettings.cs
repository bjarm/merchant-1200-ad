using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityDisplaySettings : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    [SerializeField] private Text cityNameText;

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
        Close();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
