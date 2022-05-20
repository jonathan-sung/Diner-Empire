using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : Item
{
    public enum Seasoning { None, Salt, Pepper, Chilli };
    public enum Chop { None, Chopped, Diced, Minced };
    public Seasoning seasoning;
    public Chop chop;
    public int chopNum;

    public Ingredient(Sprite sprite, string name, Seasoning seasoning, Chop chop) : base(name, sprite)
    {
        this.sprite = sprite;
        this.name = name;
        this.seasoning = seasoning;
        this.chop = chop;
        UpdateChopNum();
    }

    public Ingredient(Ingredient ingredient, Seasoning seasoning, Chop chop) : base(ingredient.name, ingredient.sprite)
    {
        this.seasoning = seasoning;
        this.chop = chop;
        UpdateChopNum();
    }

    public Ingredient(Ingredient ingredient) : base(ingredient.name, ingredient.sprite)
    {
        seasoning = ingredient.seasoning;
        chop = ingredient.chop;
        UpdateChopNum();
    }

    public void ChopIngredient()
    {
        if (chop == Chop.None)
        {
            chop = Chop.Chopped;
        }
        else if (chop == Chop.Chopped)
        {
            chop = Chop.Diced;
        }
        else if (chop == Chop.Diced)
        {
            chop = Chop.Minced;
        }
        UpdateChopNum();
        sprite = Items.ChoppedSprite(sprite);//change sprite
    }

    public void Season(Seasoning s)
    {
        //change sprite stuff
        seasoning = s;
    }

    private void UpdateChopNum()
    {
        if (chop == Chop.None)
        {
            chopNum = 0;
        }
        else if (chop == Chop.Chopped)
        {
            chopNum = 1;
        }
        else if (chop == Chop.Diced)
        {
            chopNum = 2;
        }
        else if (chop == Chop.Minced)
        {
            chopNum = 3;
        }
    }

    void Update()
    {

    }

}
