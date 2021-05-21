using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    public static ItemGenerator _ItemGenerator;
    public List<Item> ItemList = new List<Item>();

    void Awake()
    {
        _ItemGenerator = this;
    }

    public Item ItemGen(int win_id)
    {
        Item item = new Item();

        item.name = ItemList[win_id].name;
        item.id = ItemList[win_id].id;
        item.iconPath = ItemList[win_id].iconPath;
        item.description = ItemList[win_id].description;

        return item;
    }
}
