using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furniture : MonoBehaviour
{

    public SpriteRenderer sr;
    // Use this for initialization
    void Start()
    {
        sr.sortingOrder = -(Mathf.RoundToInt(transform.position.y));
        if (name == "Chair 2")
        {
            sr.sortingOrder = -(Mathf.RoundToInt(transform.position.y));
        }
    }
}
