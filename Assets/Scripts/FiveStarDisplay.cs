using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FiveStarDisplay : MonoBehaviour
{

    public Image[] star;

    public void UpdateDisplay(float rating)
    {
        int numOfFullStars = Mathf.FloorToInt(rating * star.Length);
        float remainderStar = (rating * star.Length) - Mathf.FloorToInt(rating * star.Length);
        for (int i = 0; i < numOfFullStars; i++)
        {
            star[i].fillAmount = 1;
        }
        for (int i = numOfFullStars; i < star.Length; i++)
        {
            star[i].fillAmount = 0;
        }
        if (numOfFullStars < star.Length) star[numOfFullStars].fillAmount = remainderStar;
    }
}
