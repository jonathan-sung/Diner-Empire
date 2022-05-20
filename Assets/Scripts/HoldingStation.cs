using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldingStation : Worktop
{
    public void SetDishTimer(bool on, Item dish)
    {
        if (dish is Dish)
        {
            (dish as Dish).timer.activated = on;
        }
    }
}
