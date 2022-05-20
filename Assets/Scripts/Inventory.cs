using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{

    public Item[] inventory = new Item[4];
    public Image[] image = new Image[4];
    public Image[] slot = new Image[4];

    public int selectedSlot = -1; // -1 for no slot, otherwise index

    void Start()
    {
        for (int i = 0; i < image.Length; i++)
        {
            image[i].sprite = null;
            image[i].color = new Color(1, 1, 1, 0);
        }
    }

    void Update()
    {
        CheckSwap();
        UpdateSprites();

    }

    private void CheckSwap()
    {
        if (selectedSlot != -1 && gameObject.GetComponent<Unit>().appliance != null && gameObject.GetComponent<Unit>().appliance.GetComponent<Appliance>().activated)
        {
            if (gameObject.GetComponent<Unit>().appliance.GetComponent<Cooker>() != null)
            {
                if (inventory[selectedSlot] is Ingredient && gameObject.GetComponent<Unit>().appliance.GetComponent<Cooker>().AddItem(inventory[selectedSlot] as Ingredient))
                {
                    RemoveItem(selectedSlot);
                    selectedSlot = -1;
                }
            }
            else if (gameObject.GetComponent<Unit>().appliance.GetComponent<PrepTable>() != null)
            {
                if (inventory[selectedSlot] is Ingredient && gameObject.GetComponent<Unit>().appliance.GetComponent<PrepTable>().AddItem(inventory[selectedSlot] as Ingredient))
                {
                    RemoveItem(selectedSlot);
                    selectedSlot = -1;
                }
            }
            else if (gameObject.GetComponent<Unit>().appliance.GetComponent<Bin>() != null)
            {
                if (gameObject.GetComponent<Unit>().appliance.GetComponent<Bin>().AddItem(inventory[selectedSlot]))
                {
                    RemoveItem(selectedSlot);
                    selectedSlot = -1;
                }
            }
            else if (gameObject.GetComponent<Unit>().appliance.GetComponent<Worktop>() != null)
            {
                if (gameObject.GetComponent<Unit>().appliance.GetComponent<Worktop>().AddItem(inventory[selectedSlot]))
                {
                    RemoveItem(selectedSlot);
                    selectedSlot = -1;
                }
            }


        }
    }

    public void ToggleSwap(int i)
    {
        if (i == selectedSlot)
        {
            selectedSlot = -1;
        }
        else
        {
            selectedSlot = i;
        }
    }

    public void UpdateSprites()
    {
        for (int i = 0; i < image.Length; i++)
        {
            if (inventory[i] != null)
            {
                image[i].color = new Color(1, 1, 1, 1);
                image[i].sprite = inventory[i].sprite;
            }
            if (i == selectedSlot)
            {
                slot[i].sprite = Items.panel_3;
            }
            else
            {
                slot[i].sprite = Items.panel_4;
            }
        }
    }

    public bool AddItem(Item item) //return value determines whether the transaction was successful
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = item;
                return true;
            }
        }
        return false;
    }

    public bool Swap(ref Item item)
    {
        if (selectedSlot != -1)
        {
            Item temp = inventory[selectedSlot];
            inventory[selectedSlot] = item;
            item = temp;
            if (inventory[selectedSlot] == null) RemoveItem(selectedSlot);
            selectedSlot = -1;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Swap(ref Ingredient item)
    {
        if (selectedSlot != -1 && (inventory[selectedSlot] is Ingredient || inventory[selectedSlot] == null))
        {
            Ingredient temp = (Ingredient)inventory[selectedSlot];
            inventory[selectedSlot] = item;
            item = temp;
            if (inventory[selectedSlot] == null) RemoveItem(selectedSlot);
            selectedSlot = -1;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool Swap(ref Dish item)
    {
        if (selectedSlot != -1 && inventory[selectedSlot] == null)
        {
            Dish temp = null;
            inventory[selectedSlot] = item;
            item = temp;
            if (inventory[selectedSlot] == null) RemoveItem(selectedSlot);
            selectedSlot = -1;
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool AddItem(Item item, int index)
    {
        if (inventory[index] == null)
        {
            inventory[index] = item;
            Debug.Log("Added: " + inventory[index].name);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void RemoveItem(int index)
    {
        inventory[index] = null;
        image[index].sprite = null;
        image[index].color = new Color(1, 1, 1, 0);
    }

    public void ClearInventory()
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            RemoveItem(i);
        }
    }

    public void DisplayInventory()
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] != null) Debug.Log((inventory[i]).name + " : " + ((Ingredient)inventory[i]).seasoning + " : " + ((Ingredient)inventory[i]).chop);
        }
    }

    public bool Full()
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null) return false;
        }
        return true;
    }
}
