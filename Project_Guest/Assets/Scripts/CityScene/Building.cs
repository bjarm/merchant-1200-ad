using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    [SerializeField] private GameObject buildingDisplay;

    // Start is called before the first frame update
    void Start()
    {
        buildingDisplay.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseUp()
    {
        buildingDisplay.SetActive(true);
    }
}
