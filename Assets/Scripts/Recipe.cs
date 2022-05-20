using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recipe
{

    public Ingredient[] ingredients;
    public Dish dish;

    public Recipe(Ingredient a, Ingredient b, Ingredient c, Dish d)
    {
        ingredients = new Ingredient[3];
        ingredients[0] = a;
        ingredients[1] = b;
        ingredients[2] = c;
        dish = d;
    }
}
