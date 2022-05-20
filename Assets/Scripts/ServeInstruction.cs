using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServeInstruction : Instruction
{
    //public Vector3 fromLocation;
    public Dish dish;
    public Vector3 placeLocation;

    public ServeInstruction(Dish dish, Vector3 placeLocation)
    {
        this.dish = dish;
        this.placeLocation = placeLocation;
    }
}
