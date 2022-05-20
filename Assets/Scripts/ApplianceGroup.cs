using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplianceGroup : MonoBehaviour
{

    public int type; //2 prep table, 3 cooker, 4 worktop, 5 fridge

    //location
    //item
    //appliance

    public bool[] searching;
    public Appliance[] appliance;
    public Item[] item; /*Check inventory for target item; if result is true return target item location and pass it onto findpath script
    should i keep updating the item list every frame?
        */
    public void Init(int customerGrouping)
    {
        if (type == 4)
        {
            appliance = FindObjectsOfType(typeof(Worktop)) as Appliance[];
            item = new Item[appliance.Length];
            searching = new bool[appliance.Length];
            for (int i = 0; i < appliance.Length; i++)
            {
                item[i] = ((Worktop)appliance[i]).slot;
            }
        }
        else if (type == 3)
        {
            appliance = FindObjectsOfType(typeof(Cooker)) as Appliance[];
        }
        else if (type == 2)
        {
            appliance = FindObjectsOfType(typeof(PrepTable)) as Appliance[];
        }
        else if (type == 5)
        {
            appliance = FindObjectsOfType(typeof(Fridge)) as Appliance[];
        }
        else if (type == 6)
        {
            appliance = FindObjectsOfType(typeof(Table)) as Appliance[];
            appliance = ChangeSeating(customerGrouping, appliance);
            item = new Item[appliance.Length];
            searching = new bool[appliance.Length];
            for (int i = 0; i < appliance.Length; i++)
            {
                item[i] = ((Table)appliance[i]).slot;
            }
        }
    }

    void Update()
    {
        if (item != null)
        {
            if (type == 4)
            {
                for (int i = 0; i < appliance.Length; i++)
                {
                    item[i] = ((Worktop)appliance[i]).slot;
                }
                //DisplaySearching();
            }
            else if (type == 6)
            {
                for (int i = 0; i < appliance.Length; i++)
                {
                    item[i] = ((Table)appliance[i]).slot;
                }
            }
        }
    }

    Appliance[] ChangeSeating(int tableSize, Appliance[] a)
    {
        if (tableSize % 2 != 0 || a.Length % tableSize != 0) return a;
        Appliance[] b = new Appliance[a.Length];
        int pairSize = tableSize / 2;
        int[,] tuplesOld = new int[a.Length / pairSize, pairSize];
        int[,] tuplesNew = new int[a.Length / pairSize, pairSize];
        //create tuples for original table seating
        for (int j = 0; j < a.Length / pairSize; j++)
        {
            for (int i = 0; i < pairSize; i++)
            {
                tuplesOld[j, i] = i + (j * pairSize);
            }
        }
        //create tuples for new seating plan based on the original one
        for (int j = 0; j < tuplesNew.GetLength(0); j++)
        {
            for (int i = 0; i < pairSize; i++)
            {
                if (j < tuplesNew.GetLength(0) / 2)
                {
                    tuplesNew[2 * j, i] = tuplesOld[j, i];
                }
                else
                {
                    tuplesNew[((2 * j) + 1) - tuplesNew.GetLength(0), i] = tuplesOld[j, i];
                }
            }
        }
        //assign new tuples to the seating plan
        for (int j = 0; j < tuplesNew.GetLength(0); j++)
        {
            for (int i = 0; i < pairSize; i++)
            {
                b[i + (j * pairSize)] = a[tuplesNew[j, i]];
            }
        }
        return b;
    }

    public int AtLocation(Vector3Int location)
    {
        for (int i = 0; i < appliance.Length; i++)
        {
            if (location == appliance[i].location)
            {
                return i;
            }
        }
        return -1;
    }

    public void DisplayItemNames()
    {
        if (item != null)
        {
            for (int i = 0; i < item.Length; i++)
            {
                if (item[i] == null) continue;
                Debug.Log(item[i].name);
            }
        }
    }

    void DisplaySearching()
    {
        for (int i = 0; i < searching.Length; i++)
        {
            if (searching[i])
            {
                Debug.Log("Searching True: " + i);
            }
        }
    }

    public Vector4 FindIngredient(Ingredient ingredient)
    {
        for (int i = 0; i < item.Length; i++)
        {
            bool criteria = item[i] != null;
            criteria = criteria && item[i].name == ingredient.name;
            criteria = criteria && ((Ingredient)item[i]).seasoning == ingredient.seasoning;
            criteria = criteria && ((Ingredient)item[i]).chop == ingredient.chop;
            criteria = criteria && !searching[i];
            if (criteria) return new Vector4(appliance[i].standingLocation.x, appliance[i].standingLocation.y, appliance[i].standingLocation.z, i);
        }
        return new Vector4(0, 0, -1, -1);
    }
}
