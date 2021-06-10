using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public static string currentCity;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
}
