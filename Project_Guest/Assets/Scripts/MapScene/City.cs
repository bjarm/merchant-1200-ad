using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    [SerializeField] private CityDisplaySettings cityDisplay;

    private string cityName;
    private int cityPopulation;

    // Start is called before the first frame update
    void Start()
    {
        cityName = gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            GameObject.Find("Controllers").GetComponent<MovementController>().GoToCity(cityName);
        }
    }

    void OnMouseUp()
    {
        cityDisplay.Open(cityName);
    }
}