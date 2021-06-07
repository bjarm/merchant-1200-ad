using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Paths;

public class MovementController : MonoBehaviour
{

    public GameObject characterObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GoToCity(string destinationCity) 
    {
        Paths paths = new Paths();
        characterObject.GetComponent<FollowingByPath>().path = paths.getWay(characterObject.GetComponent<Character>().location, destinationCity);
    } 
}
