using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookInstruction : Instruction
{
    public Recipe recipe;
    public Vector3 cookerLocation;

    public CookInstruction(Recipe recipe, Vector3 cookerLocation)
    {
        this.recipe = recipe;
        this.cookerLocation = cookerLocation;
    }
}
