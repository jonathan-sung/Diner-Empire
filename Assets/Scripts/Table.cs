using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Table : Worktop
{
    public bool occupied;
    GameObject chair;
    SpriteRenderer chairRenderer;

    new void Start()
    {
        occupied = false;
        base.Start();
        if (foregroundType)
        {
            chair = (GameObject)Instantiate(Resources.Load("Prefabs/Chair"));
            chair.transform.position = location + new Vector3(0.5f, 0.2f, 1);
            spriteRenderer.sortingLayerName = "Items 2";
            ChangeStandingLocation("Table");
        }
        else
        {
            chair = (GameObject)Instantiate(Resources.Load("Prefabs/Chair 2"));
            chair.transform.position = location + new Vector3(0.5f, -1.25f, -1);
            chairRenderer = chair.GetComponent<SpriteRenderer>();
        }
    }

    public void UpdateChair()
    {
        if (!foregroundType && chairRenderer != null)
        {
            if (occupied)
            {
                chairRenderer.sprite = Items.sittingChair;
            }
            else
            {
                chairRenderer.sprite = Items.normalChair;
            }
        }
    }

    public void EnterTable()
    {
        occupied = true;
    }

    public void ExitTable()
    {
        occupied = false;
    }
}
