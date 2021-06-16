using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class City : MonoBehaviour
{
	private MapSceneController mapSceneController;
	
	public string cityName;
	public Coordinates cityCoordinates;
	public List<Production> listOfProductions = new List<Production>();
	
	private bool mouse;

	// Start is called before the first frame update
	void Awake()
	{
		mapSceneController = GameObject.Find("Controllers").GetComponent<MapSceneController>();
		var cityObject = gameObject;
		cityName = cityObject.name;
		var position = cityObject.transform.position;
		cityCoordinates = new Coordinates(position.x, position.y);

		switch (cityName)
		{
			// Внимание! Костыль-костылич! Мне лень писать нормальную функцию/метод для инициализации уникальных зданий
			// в зависимости от города до перехода от sql в json, поэтому я тупа по именам выдам нашим трем городам
			// свои здания прямо здесь.
			case "Novgorod":
				BuildNewProductionInCity("Fur");
				BuildNewProductionInCity("Salt");
				break;
			case "Riga":
				BuildNewProductionInCity("Herring");
				BuildNewProductionInCity("Wine");
				break;
			case "Toropets":
				BuildNewProductionInCity("Bread");
				BuildNewProductionInCity("Wine");
				break;
		}
		
		EventManager.DateChanged.Subscribe(ProduceGoods);
		EventManager.DateChanged.Subscribe(ConsumeGoods);
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetMouseButtonDown(1) && mouse == true)
		{
			if (DataBase.GetPathLengthInDays(GameManager.currentCity.cityName, cityName) != 0)
			{
				GameObject.Find("Controllers").GetComponent<MovementController>().GoToCity(this);
			}
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

	[Serializable]
	public class Coordinates
	{
		public float x;
		public float y;

		public Coordinates(float newX, float newY)
		{
			x = newX;
			y = newY;
		}
	}
	
	public class Production
	{
		public string productType;
		public int productionAmountPerDay;
	}

	public void ProduceGoods(int days)
	{
		foreach (var building in listOfProductions)
		{
			DataBase.BuildingTransfersGoods(cityName, building.productType, building.productionAmountPerDay * days);
		}
	}

	public void BuildNewProductionInCity(string product)
	{
		var newProduction = new Production
		{
			productType = product, productionAmountPerDay = DataBase.GetAmountOfProduction(product)
		};
		listOfProductions.Add(newProduction);
	}

	public void ConsumeGoods(int days)
	{
		DataBase.PopulationConsumeGoods(cityName, days);
		DataBase.UpdatePriceForCity(cityName);
	}
}