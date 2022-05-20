using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DishTimer : MonoBehaviour
{

    public Dish dish;
    float currentTime;
    float timeBeforeDecay = 120;
    float decayRate = 3; //1 quality per decayRate, so a decay rate of 3 means the dish quality will decrease by 1 every 3 seconds
    bool start;
    public bool activated;

    public void StartTimer()
    {
        currentTime = 0;
        start = true;
        activated = true;
    }

    void Update()
    {
        if (start && activated)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= timeBeforeDecay) dish.quality -= Time.deltaTime / (decayRate * 100);
            if (dish.quality <= 0) { dish.quality = 0; Destroy(gameObject); }
        }
    }
}
