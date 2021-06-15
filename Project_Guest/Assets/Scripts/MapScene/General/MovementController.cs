using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Paths;

public class MovementController : MonoBehaviour
{

	public GameObject characterObject;

	private Character character;
	private FollowingByPath followingByPath;

	// Start is called before the first frame update
	void Start()
	{
		followingByPath = characterObject.GetComponent<FollowingByPath>();
		character = characterObject.GetComponent<Character>();
	}

	// Update is called once per frame
	void Update()
	{
        
	}

	public void GoToCity(City destinationCity) 
	{
		if (followingByPath.onTheWay) 
		{
			return;
		}
		Paths paths = new Paths();
		followingByPath.path = paths.getWay(GameManager.currentCity.cityName, destinationCity.cityName);
		EventManager.DateChanged.Publish(DataBase.GetPathLengthInDays(GameManager.currentCity.cityName, destinationCity.cityName));
		GameManager.currentCity = destinationCity;
	} 
}