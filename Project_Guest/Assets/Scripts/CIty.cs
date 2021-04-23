using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
    [SerializeField] private CityDisplaySettings cityDisplay;

    private string cityName;

    // Start is called before the first frame update
    void Start()
    {
        cityName = gameObject.name;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMouseUp()
    {
        cityDisplay.Open(cityName);
    }
}