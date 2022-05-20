using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Appliance : MonoBehaviour
{
    public Vector3 standingLocation;
    public Vector3Int location;
    public BoxCollider2D c;
    public Unit u;
    public Canvas canvas;
    public CanvasScaler canvasScaler;
    public bool activated;
    public bool swap;

    private bool triggered;
    protected Collider2D col;

    protected List<Collider2D> queue = new List<Collider2D>();

    protected void Start()
    {
        standingLocation = location + new Vector3(0, -1, 0);
        canvas.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckCanvas();
        CheckCollision();
    }

    protected void CheckCollision()
    {
        if (triggered)
        {
            if (queue.Count != 0)
            {
                AddUnit();
            }
        }
        else if (!triggered)
        {
            RemoveUnit();
        }
    }

    protected void CheckCanvas()
    {
        if (u != null && u.selected && activated)
        {
            canvas.enabled = true;
        }
        else {
            canvas.enabled = false;
        }

    }

    private void AddUnit()
    {
        for (int i = 0; i < queue.Count; i++)
        {
            Unit tempUnit = queue[i].gameObject.GetComponent<Unit>();
            if (tempUnit != null && (u == null || tempUnit == u) && (tempUnit.appliance == null))
            {
                //Debug.Log("Entered " + name);
                u = tempUnit;
                u.appliance = gameObject;
                u.overAppliance = true;
            }
        }
    }

    private void RemoveUnit()
    {
        if (u != null && col.gameObject.GetComponent<Unit>() == u && u.appliance != null)
        {
            //Debug.Log("Left " + name);
            Deactivate();
            u.appliance = null;
            u.overAppliance = false;
            u = null;
        }
        if (queue.Count != 0) triggered = true;
    }

    public void ToggleSwap()
    {
        if (swap)
        {
            swap = false;
        }
        else
        {
            swap = true;
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        //Debug.Log("Enter");
        this.col = col;
        triggered = true;
        if (!queue.Contains(col) && col.gameObject.GetComponent<Customer>() == null)
        {
            queue.Add(col);
        }

    }

    void OnTriggerExit2D(Collider2D col)
    {
        //Debug.Log("Exit");
        this.col = col;
        ResetUnit(col);
        triggered = false;
        queue.Remove(col);
        RemoveUnit();
    }

    bool UnitHasAppliance(Collider2D col)
    {
        return (col.gameObject.GetComponent<Unit>() != null && col.gameObject.GetComponent<Unit>().appliance != null);
    }

    private void ResetUnit(Collider2D col)
    {
        Unit unit = col.gameObject.GetComponent<Unit>();
        if (unit != null)
        {
            if (this is PrepTable)
            {
                ResetUnitPrep(ref unit);
            }
            else if (this is Cooker)
            {
                ResetUnitCook(ref unit);
            }
        }
    }

    private void ResetUnitPrep(ref Unit unit)
    {
        unit.chopping = false;
        unit.seasoning = false;
    }

    private void ResetUnitCook(ref Unit unit)
    {
        unit.cooking = false;
    }

    public void Activate()
    {
        activated = true;
        canvas.enabled = true;
        if (canvasScaler != null) canvasScaler.enabled = true;
    }

    public void Deactivate()
    {
        activated = false;
        canvas.enabled = false;
        if (canvasScaler != null) canvasScaler.enabled = false;
    }

    public void ToggleActivate()
    {
        if (activated)
        {
            Deactivate();
        }
        else
        {
            Activate();
        }
    }

    public void ChangeStandingLocation(string type)
    {
        standingLocation = location;
        if (type == "Table")
        {
            c.offset = new Vector2(0, 0.5f);
        }
        else if (type == "Worktop")
        {
            c.offset = new Vector2(0, 0);
        }
        else
        {
            c.offset = new Vector2(0, 0.5f);
        }

    }

    protected void UnitPosition(bool a, bool b)
    {
        if (a || b)
        {
            if (u != null)
            {
                Vector3 activeLocation = standingLocation + Unit.activeOffset;
                if (u.transform.position != activeLocation)
                {
                    u.um.ControlledMove(activeLocation);
                }
            }
        }
    }

    protected void RemoveItem(ref Ingredient ingredient)
    {
        ingredient = null;
    }

    protected bool SameIngredient(Ingredient a, Ingredient b)
    {
        if (a != null && b != null)
        {
            if (a.name == b.name && a.seasoning == b.seasoning && a.chop == b.chop)
            {
                return true;
            }

        }
        return false;
    }
}
