using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.position = new Vector3(GameManager.currentCity.cityCoordinates.x,
            GameManager.currentCity.cityCoordinates.y, -5);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HasArrivedInTown()
    {
        EventManager.PlayerHasArrivedInTown.Publish();
    }
}
