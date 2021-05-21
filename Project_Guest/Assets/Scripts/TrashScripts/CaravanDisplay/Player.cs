using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player _Player;

    public static Dictionary<int, Item> _Cargo = new Dictionary<int, Item>(); 

    void Awake()
    {
        _Player = this;
    }
}
