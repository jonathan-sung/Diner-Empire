using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fridge : Appliance
{
    public Image[] image;
    public Ingredient[] contents = new Ingredient[12];
    public RestaurantManager rm;
    float ingredientCost = 0.4f;

    // Use this for initialization
    new void Start()
    {
        canvas.enabled = false;
        UpdateUISprites();
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        CheckCollision();
        if (activated)
        {
            CheckCanvas();
        }
        //Debug.Log(gameObject.name + ": " + (u != null));
    }

    public void UpdateUISprites()
    {
        for (int i = 0; i < image.Length; i++)
        {
            if (contents[i] != null)
            {
                image[i].sprite = contents[i].sprite;
                image[i].color = new Color(1, 1, 1, 1);
            }
        }
    }

    public void SetupContents(RestaurantManager rm)
    {
        for (int i = 0; i < rm.ingredientList.Length; i++)
        {
            contents[i] = rm.ingredientList[i];
        }
        UpdateUISprites();
    }

    public void GetIngredient(int index)
    {
        if (u.GetComponent<Inventory>() != null && contents[index] != null)
        {
            if (u.GetComponent<Inventory>().AddItem(new Ingredient(contents[index])))
            {
                rm.AddMoney(-ingredientCost);
            }
        }
    }
    public bool GetIngredient(Ingredient ingredient)
    {
        int index = -1;
        for (int i = 0; i < contents.Length; i++)
        {
            if (ingredient == contents[i])
            {
                index = i;
                break;
            }
        }
        if (index < 0) return false;
        if (u.GetComponent<Inventory>() != null && contents[index] != null)
        {
            if (u.GetComponent<Inventory>().AddItem(new Ingredient(contents[index])))
            {
                rm.AddMoney(-ingredientCost);
                return true;
            }
        }
        return false;
    }
}
