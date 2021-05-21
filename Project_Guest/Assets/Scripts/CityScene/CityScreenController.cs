using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityScreenController : MonoBehaviour
{
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
