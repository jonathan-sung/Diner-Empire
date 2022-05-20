using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Item item;
    private InfoDisplay infoDisplay;
    private RestaurantManager rm;
    public int itemIndex;
    public int slotType; //1 inventory, 2 prep table, 3 cooker, 4 worktop, 5 fridge, 6 AI ingredient menu, 7 AI dish menu, 8 Restaurant UI (top-bar), 9 Minimise AI UI, 10 AI instruction number
    public int rightClickType; //0 - no right click function, 1 - dishes

    public bool over;
    Texture2D cursorMouseL;
    Texture2D cursorMouseR;
    Texture2D cursor;

    // Use this for initialization
    void Start()
    {
        over = false;
        infoDisplay = GameObject.Find("Player").GetComponent<InfoDisplay>();
        rm = GameObject.Find("Restaurant Manager").GetComponent<RestaurantManager>();
        cursorMouseL = Resources.Load<Texture2D>("Sprites/cursor_mouse_l");
        cursorMouseR = Resources.Load<Texture2D>("Sprites/cursor_mouse_r");
        cursor = Resources.Load<Texture2D>("Sprites/cursor");
        CheckItem();
    }

    // Update is called once per frame
    void Update()
    {
        if (over)
        {
            CheckItem();
            UpdateCursor();
            if (item != null) infoDisplay.Display(item);
        }
    }

    private void CheckItem()
    {
        if (slotType == 1)
        {
            item = transform.parent.parent.parent.GetComponent<Inventory>().inventory[itemIndex];
        }
        else if (slotType == 2)
        {
            item = transform.parent.parent.GetComponent<PrepTable>().slot;
        }
        else if (slotType == 3)
        {
            if (itemIndex < transform.parent.parent.GetComponent<Cooker>().ingredients.Length)
            {
                item = transform.parent.parent.GetComponent<Cooker>().ingredients[itemIndex];
            }
            else
            {
                item = transform.parent.parent.GetComponent<Cooker>().output;
            }
        }
        else if (slotType == 4)
        {
            item = transform.parent.parent.GetComponent<Worktop>().slot;
        }
        else if (slotType == 5)
        {
            //fridge
            item = transform.parent.parent.GetComponent<Fridge>().contents[itemIndex];
        }
        else if (slotType == 6)
        {
            //AI ingredients
            item = transform.parent.parent.parent.parent.GetComponent<AIController>().contents[itemIndex];
        }
        else if (slotType == 7)
        {
            //AI dish
            if (itemIndex < rm.recipeList.Length && rm.recipeList[itemIndex] != null) item = rm.recipeList[itemIndex].dish;
        }
        else if (slotType == 8)
        {
            //Restaurant UI
            item = transform.parent.parent.GetComponent<RestaurantManager>().item[itemIndex];
        }
        else if (slotType == 10)
        {
            //Restaurant UI
            int instructionLength = transform.parent.parent.parent.parent.parent.parent.GetComponent<AIController>().instructionLength;
            item = new Item("Instructions in Progress", null);
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        over = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        over = false;
        Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
        infoDisplay.ResetDisplay();
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        over = false;
        infoDisplay.ResetDisplay();
        if (rightClickType == 1 && pointerEventData.button == PointerEventData.InputButton.Right)
        {
            RecipeBook rb = GameObject.Find("RecipeBook").GetComponent<RecipeBook>();
            if (rb)
            {
                if (itemIndex + 2 > 0 && itemIndex + 2 <= rb.rm.recipeList.Length + 1)
                {
                    if (rb.currentPage == itemIndex + 2 && rb.display)
                    {
                        rb.display = false;
                    }
                    else
                    {
                        rb.display = true;
                        rb.currentPage = itemIndex + 2;
                    }
                }

            }
        }
        else
        {
            Cursor.SetCursor(cursor, Vector2.zero, CursorMode.Auto);
        }
    }

    void UpdateCursor()
    {
        if (rightClickType == 1)
        {
            Cursor.SetCursor(cursorMouseR, Vector2.zero, CursorMode.Auto);
        }
        if (slotType == 9)
        {
            Cursor.SetCursor(cursorMouseL, Vector2.zero, CursorMode.Auto);
        }
    }
}
