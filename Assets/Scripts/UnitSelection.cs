using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitSelection : MonoBehaviour
{
    Unit currentSelected;
    Customer currentCustomer;
    WorktopHover currentWorktopHover;
    int cycleIndex;

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Debug.DrawRay(new Vector2(position.x, position.y), Vector2.zero, Color.red);
            //RaycastHit2D hit = Physics2D.Raycast(new Vector2(position.x, position.y), Vector2.zero, 0, (1 << 8) | (1 << 9));
            RaycastHit2D[] hits = Physics2D.RaycastAll(new Vector2(position.x, position.y), Vector2.zero, 0, (1 << 8) | (1 << 9) | (1 << 11));
            if (hits.Length > 0)
            {
                if (cycleIndex < (hits.Length - 1))
                {
                    cycleIndex++;
                }
                else
                {
                    cycleIndex = 0;
                }
                if (hits[cycleIndex].collider != null && !EventSystem.current.IsPointerOverGameObject() || (EventSystem.current.IsPointerOverGameObject() && (Bubble.overBubble)))
                {
                    Unit u = hits[cycleIndex].collider.gameObject.GetComponent<Unit>();
                    if (currentSelected != null && currentSelected == u)
                    {
                        Deselect();
                    }
                    if (u != null && !(u is Customer))
                    {
                        if (currentSelected != null) Deselect();
                        u.selected = true;
                        currentSelected = u;
                        return;
                    }
                }
                CheckHitCustomer(hits);
                CheckHitWorktopHover(hits);
            }
            else if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (currentSelected != null)
                {
                    Deselect();
                }
            }
        }
        if (Input.GetMouseButtonDown(2))
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(new Vector2(position.x, position.y), Vector2.zero, 0, (1 << 11));
            ShowCustomerDishRecipe(hits);
        }
        if (currentCustomer != null)
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(new Vector2(position.x, position.y), Vector2.zero, 0, (1 << 11));
            if (!CheckHitCustomer(hits))
            {
                currentCustomer.showInfo = false;
                currentCustomer.infoDisplay.ResetDisplay();
                currentCustomer = null;
            }
        }
        if (currentWorktopHover != null)
        {
            Vector3 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D[] hits = Physics2D.RaycastAll(new Vector2(position.x, position.y), Vector2.zero, 0, (1 << 9));
            if (!CheckHitWorktopHover(hits))
            {
                currentWorktopHover.over = false;
                currentWorktopHover.infoDisplay.ResetDisplay();
                currentWorktopHover = null;
            }
        }
    }

    void ShowCustomerDishRecipe(RaycastHit2D[] hits)
    {
        for (int i = 0; i < hits.Length; i++)
        {
            Unit u = hits[i].collider.gameObject.GetComponent<Unit>();
            if (u && (u is Customer))
            {
                int itemIndex = GetDishIndex((u as Customer).dish, (u as Customer).rm.recipeList);
                RecipeBook rb = GameObject.Find("RecipeBook").GetComponent<RecipeBook>();
                if (rb && itemIndex >= 0)
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
                    return;
                }

            }
        }
    }

    int GetDishIndex(Dish dish, Recipe[] recipes)
    {
        if (dish != null && recipes != null)
        {
            for (int i = 0; i < recipes.Length; i++)
            {
                if (dish == recipes[i].dish)
                    return i;
            }
        }
        return -1;
    }

    bool CheckHitCustomer(RaycastHit2D[] hits)
    {
        for (int i = 0; i < hits.Length; i++)
        {
            Unit u = hits[i].collider.gameObject.GetComponent<Unit>();
            if (u != null && (u is Customer))
            {
                if (currentCustomer != null && u != currentCustomer) currentCustomer.showInfo = false;
                currentCustomer = u as Customer;
                currentCustomer.showInfo = true;
                return true;
            }
        }
        return false;
    }

    bool CheckHitWorktopHover(RaycastHit2D[] hits)
    {
        for (int i = 0; i < hits.Length; i++)
        {
            WorktopHover u = hits[i].collider.gameObject.GetComponent<WorktopHover>();
            if (u != null)
            {
                if (currentWorktopHover != null && u != currentWorktopHover) currentWorktopHover.over = false;
                currentWorktopHover = u;
                currentWorktopHover.over = true;
                return true;
            }
        }
        return false;
    }

    private void Deselect()
    {
        currentSelected.selected = false;
        currentSelected = null;
    }

}
