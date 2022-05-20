using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Worktop : Appliance
{
    public Item slot;
    public Image itemSprite;
    public SpriteRenderer spriteRenderer;
    public bool foregroundType;

    // Use this for initialization
    new void Start()
    {
        base.Start();
        if (foregroundType)
        {
            spriteRenderer.sortingLayerName = "Items 1";
            ChangeStandingLocation("Worktop");
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckCollision();
        UpdateSprites();
        CheckItem();
        if (activated) CheckCanvas();
        if (this is Table) ((Table)this).UpdateChair();
    }

    private void CheckItem()
    {
        if (slot is Ingredient)
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 1);
            c.size = new Vector2(1.4f, 1.4f);
        }
        else if (slot is Dish)
        {
            transform.localScale = new Vector3(0.75f, 0.75f, 1);
            c.size = new Vector2(0.94f, 0.94f);
        }
    }

    public void PopIngredient()
    {
        if (slot != null)
        {
            if (u.GetComponent<Inventory>().AddItem(slot))
            {
                if (this is HoldingStation) ((HoldingStation)this).SetDishTimer(true, slot);
                slot = null;
            }
            else if (u.GetComponent<Inventory>().selectedSlot != -1) //full inventory
            {
                u.GetComponent<Inventory>().Swap(ref slot);
            }
        }
    }

    public bool AIPopIngredient(Ingredient ingredient)
    {
        if (slot != null)
        {
            if (slot is Ingredient && SameIngredient((Ingredient)slot, ingredient) && u.GetComponent<Inventory>().AddItem(slot))
            {
                slot = null;
                return true;
            }
        }
        return false;
    }

    public bool AIPopIngredient()
    {
        if (slot != null)
        {
            if (u.GetComponent<Inventory>().AddItem(slot))
            {
                if (this is HoldingStation) ((HoldingStation)this).SetDishTimer(true, slot);
                slot = null;
                return true;
            }
        }
        return false;
    }

    public bool AddItem(Item item) //return value determines whether the transaction was successful
    {
        if (slot == null)
        {
            slot = item;
            if (this is HoldingStation) ((HoldingStation)this).SetDishTimer(false, slot);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UpdateSprites()
    {
        if (slot != null)
        {
            itemSprite.sprite = slot.sprite;
            itemSprite.color = new Color(1, 1, 1, 1);
            if (!(this is Bin))
            {
                spriteRenderer.sprite = slot.sprite;
                spriteRenderer.color = new Color(1, 1, 1, 1);
            }
        }
        else
        {
            itemSprite.sprite = null;
            itemSprite.color = new Color(1, 1, 1, 0);
            if (!(this is Bin))
            {
                spriteRenderer.sprite = null;
                spriteRenderer.color = new Color(1, 1, 1, 0);
            }
        }
    }
}
