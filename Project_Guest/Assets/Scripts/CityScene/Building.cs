using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Building : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
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
    public void OnPointerClick(PointerEventData eventData)
    {
        buildingDisplay.SetActive(true);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.GetComponent<SpriteRenderer>().color = Color.yellow;
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        transform.GetComponent<SpriteRenderer>().color = Color.white;
    }
}
