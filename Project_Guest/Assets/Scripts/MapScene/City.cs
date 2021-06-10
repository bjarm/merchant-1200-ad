using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
	private MapSceneController mapSceneController;
	
	private string cityName;
	private int cityPopulation;
	private bool mouse;

	// Start is called before the first frame update
	void Start()
	{
		mapSceneController = GameObject.Find("Controllers").GetComponent<MapSceneController>();
		cityName = gameObject.name;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(1) && mouse == true)
		{
			GameObject.Find("Controllers").GetComponent<MovementController>().GoToCity(cityName);
		}
	}

	void OnMouseUp()
	{
		mapSceneController.OpenCityDisplay();
	}

	void OnMouseEnter()
	{
		mouse = true;
	}

	void OnMouseExit() 
	{
		mouse = false;
	}
}