using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CaravanController : MonoBehaviour
{
	public RectTransform rowPrefab;
	public RectTransform parent;

	public GameObject cargoDisplay;
	public GameObject checkCargoButton;

	private void Awake()
	{
		checkCargoButton.GetComponent<Button>().onClick.AddListener(delegate
		{
			GenerateCargoList();
			cargoDisplay.SetActive(true);
		});
	}

	public void GenerateCargoList()
	{
		OnReceivedModels(GetItems());
	}

	private class CargoProduct
	{
		public string Name { get; set; }

		public int Amount { get; set; }
	}
	
	private CargoProduct[] GetItems()
	{
		var productNames = Economy.productList;
		var results = new CargoProduct[productNames.Count + 1];
		var count = 1;
		
		results[0] = new CargoProduct()
		{
			Name = "Gold",
			Amount = Economy.GetPlayerGoldAmount()
		};
		foreach (var product in productNames)
		{
			results[count] = new CargoProduct()
			{
				Name = product,
				Amount = Economy.GetPlayerProductAmount(product)
			};
			count++;
		}
		return results;
		
	}

	private class CargoItem
	{
		public Text name;
		public Text amount;

		public CargoItem(Transform rootView)
		{
			name = rootView.Find("Name").GetComponent<Text>();
			amount = rootView.Find("Amount").GetComponent<Text>();
		}
	}
	
	private void InitializeCargoItem(GameObject viewGameObject, CargoProduct model)
	{
		var item = new CargoItem(viewGameObject.transform);
		item.name.text = model.Name;
		item.amount.text = model.Amount.ToString();
	}

	private void OnReceivedModels(CargoProduct[] models)
	{
		foreach (Transform child in parent)
		{
			Destroy(child.gameObject);
		}

		foreach (var model in models)
		{
			var instance = Instantiate(rowPrefab.gameObject, parent, false);
			InitializeCargoItem(instance, model);
		}
	}
}
