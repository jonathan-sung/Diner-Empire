using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrepInstruction : Instruction
{
    public Ingredient ingredient;
    public Ingredient.Chop chop;
    public Ingredient.Seasoning seasoning;
    public Vector3 prepTableLocation;

    public PrepInstruction(Ingredient ingredient, Ingredient.Chop chop, Ingredient.Seasoning seasoning, Vector3 prepTableLocation)
    {
        this.ingredient = ingredient;
        this.chop = chop;
        this.seasoning = seasoning;
        this.prepTableLocation = prepTableLocation;
    }

}
