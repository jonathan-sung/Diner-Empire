using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bin : Worktop
{

    new public bool AddItem(Item item) //return value determines whether the transaction was successful
    {
        slot = null;
        slot = item;
        return true;
    }

}
