using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{

    public Sprite sprite;
    public string name;

    public Item(string name, Sprite sprite)
    {
        this.name = name;
        this.sprite = sprite;
    }

}
