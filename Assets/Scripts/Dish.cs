using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dish : Item
{
    public float quality;
    public float cookTime;
    public float price;
    public DishTimer timer;

    public Dish(string name, Sprite sprite, float cookTime, float price) : base(name, sprite)
    {
        this.cookTime = cookTime;
        this.price = price;
        quality = -1;
    }

    public Dish(Dish dish, float quality) : base(dish.name, dish.sprite)
    {
        cookTime = dish.cookTime;
        price = dish.price;
        this.quality = quality;
        AddDecay();
    }

    void AddDecay()
    {
        GameObject g = new GameObject("Dish Timer");
        g = Object.Instantiate(g);
        g.AddComponent<DishTimer>();
        timer = g.GetComponent<DishTimer>();
        timer.dish = this;
        timer.StartTimer();
    }
}
