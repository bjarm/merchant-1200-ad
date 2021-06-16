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
		var playerWarehouse = DataBase.GetProductTable("Player");

		var count = playerWarehouse.Columns.Count - 2;
		var results = new CargoProduct[count];
		var productNames = DataBase.GetProductList();

		for (var i = 0; i < count; i++)
		{
			if (i == 0)
			{
				results[i] = new CargoProduct()
				{
					Name = "Золото",
					Amount = int.Parse(playerWarehouse.Rows[0][i + 2].ToString())
				};
			}
			else
			{
				results[i] = new CargoProduct()
				{
					Name = productNames[i-1],
					Amount = int.Parse(playerWarehouse.Rows[0][i + 2].ToString())
				};
			}
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
