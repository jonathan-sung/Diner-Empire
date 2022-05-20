using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeadDisplay : MonoBehaviour
{
    public Image[] cookingBeads;
    public Image[] movementBeads;
    public Image[] prepareBeads;

    public void UpdateBeads(int cookingScore, int movementScore, int prepScore)
    {
        for (int i = 0; i < cookingBeads.Length; i++)
        {
            cookingBeads[i].enabled = (i < cookingScore) ? true : false;
            movementBeads[i].enabled = (i < movementScore) ? true : false;
            prepareBeads[i].enabled = (i < prepScore) ? true : false;
        }
    }
}
