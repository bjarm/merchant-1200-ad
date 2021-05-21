using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Item
{
    public string name;
    public int id;
    public string iconPath;
    [Multiline] public string description;
}
