using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationCursor : MonoBehaviour
{

    float currentTime = 0;

    void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime >= 0.4f)
        {
            Destroy(gameObject);
        }
    }
}
