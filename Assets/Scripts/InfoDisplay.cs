using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InfoDisplay : MonoBehaviour
{

    public Canvas canvas;
    public Text itemName;
    public Text seasoning;
    public Text chop;
    public Text quality;

    public GameObject display;
    private int offset = 100;

    // Use this for initialization
    void Start()
    {
        canvas.enabled = false;
    }

    public void Display(Item item)
    {
        if (item == null) return;
        canvas.enabled = true;
        display.transform.position = Camera.main.ScreenToViewportPoint(new Vector3(Screen.width * (Input.mousePosition.x), Screen.height * (Input.mousePosition.y + offset)));
        if (display.transform.position.y > Screen.height)
        {
            display.transform.position = Camera.main.ScreenToViewportPoint(new Vector3(Screen.width * (Input.mousePosition.x), Screen.height * (Input.mousePosition.y - offset)));
        }

        if (item is Ingredient)
        {
            ReadItem((Ingredient)item);
            quality.enabled = false;
            seasoning.enabled = true;
            chop.enabled = true;
        }
        else if (item is Dish)
        {
            ReadItem((Dish)item);
            if (((Dish)item).quality >= 0)
            {
                quality.enabled = true;
            }
            else
            {
                quality.enabled = false;
            }
            seasoning.enabled = false;
            chop.enabled = false;
        }
        else if (item.sprite == null)
        {
            ReadUI(item);
            quality.enabled = false;
            seasoning.enabled = false;
            chop.enabled = false;
        }
        else {
            ReadItem(item);
            quality.enabled = false;
            seasoning.enabled = false;
            chop.enabled = false;
        }
    }

    public void ResetDisplay()
    {
        canvas.enabled = false;
    }

    private void ReadUI(Item item)
    {
        itemName.text = item.name;
    }

    public void ReadCustomer(Customer customer)
    {
        if (CustomerExists(customer) && CustomerHasOrder(customer) && !EventSystem.current.IsPointerOverGameObject())
        {
            canvas.enabled = true;
            quality.enabled = false;
            seasoning.enabled = true;
            chop.enabled = true;
            display.transform.position = Camera.main.ScreenToViewportPoint(new Vector3(Screen.width * (Input.mousePosition.x), Screen.height * (Input.mousePosition.y + offset)));
            if (display.transform.position.y > Screen.height)
            {
                display.transform.position = Camera.main.ScreenToViewportPoint(new Vector3(Screen.width * (Input.mousePosition.x), Screen.height * (Input.mousePosition.y - offset)));
            }
            float timeLeft = (customer.waitTime - customer.currentTime) * customer.rm.timeScaleFactor;
            itemName.text = "Dish: " + customer.dish.name;
            seasoning.text = "Time Left: " + Mathf.FloorToInt((timeLeft / 3600) % 24).ToString().PadLeft(2, '0') + ":" + Mathf.FloorToInt((timeLeft / 60) % 60).ToString().PadLeft(2, '0');
            chop.text = "Order in Queue: " + customer.priorityInQueue;
        }
    }

    bool CustomerExists(Customer c)
    {
        if (c != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool CustomerHasOrder(Customer c)
    {
        if (c.stage >= 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ReadItem(Item item)
    {
        itemName.text = "Name: " + item.name;
    }

    private void ReadItem(Ingredient item)
    {
        itemName.text = "Name: " + item.name;
        seasoning.text = "Seasoning: " + item.seasoning.ToString();
        chop.text = "Chop: " + item.chop.ToString();
    }

    private void ReadItem(Dish item)
    {
        itemName.text = "Dish: " + item.name;
        quality.text = "Quality: " + (item.quality * 100).ToString("F0") + "/100";
    }
}
