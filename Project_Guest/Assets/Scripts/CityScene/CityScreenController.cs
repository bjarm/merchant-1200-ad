using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CityScreenController : MonoBehaviour
{
    public GameObject exitButton;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Open(GameObject display)
    {
        display.SetActive(true);
    }
    public void Close(GameObject display)
    {
        display.SetActive(false);
    }
}
