using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerGenerator : MonoBehaviour
{
    float minSpawnRate;
    float maxSpawnRate; //one customer generated per *maxSpawnRate* seconds
    float rushHourSpawnFactor;
    float currentSpawnRate; //generation interval in seconds
    public float spawnRate;
    float timer = 0;

    int customerGrouping;

    public bool activated;

    RestaurantManager rm;

    void Start()
    {
        activated = true;
        rm = GetComponent<RestaurantManager>();
        Init(rm.minSpawnRate, rm.maxSpawnRate, rm.rushHourSpawnFactor, rm.customerSpawnGrouping);
        UpdateFrequency();
    }

    // Update is called once per frame
    void Update()
    {
        if (activated)
        {
            timer += Time.deltaTime;
            CheckSpawnRate();
            if (timer >= currentSpawnRate)
            {
                timer = 0;
                SpawnCustomer(customerGrouping);
            }
            if (Input.GetKeyDown(KeyCode.C))
            {
                SpawnCustomer();
            }
        }
    }

    void Init(float minSpawnRate, float maxSpawnRate, float rushHourSpawnFactor, int customerGrouping)
    {
        this.minSpawnRate = minSpawnRate;
        this.maxSpawnRate = maxSpawnRate;
        this.rushHourSpawnFactor = rushHourSpawnFactor;
        this.customerGrouping = customerGrouping;
    }

    public void CheckSpawnRate()
    {
        if (rm.rushHour)
        {
            currentSpawnRate = spawnRate / rushHourSpawnFactor;
        }
        else
        {
            currentSpawnRate = spawnRate;
        }
    }

    public void SpawnCustomer()
    {
        if (!activated) return;
        GameObject g = (GameObject)Instantiate(Resources.Load("Prefabs/Customer"));
        g.GetComponent<Customer>().rm = GetComponent<RestaurantManager>();
    }

    public void SpawnCustomer(Dish dish)
    {
        if (!activated) return;
        GameObject g = (GameObject)Instantiate(Resources.Load("Prefabs/Customer"));
        g.GetComponent<Customer>().rm = GetComponent<RestaurantManager>();
        g.GetComponent<Customer>().dish = dish;
    }

    public void SpawnCustomer(int groupNumber)
    {
        if (!activated) return;
        Dish dish = rm.SelectRandomDish();
        for (int i = 0; i < groupNumber; i++)
        {
            SpawnCustomer(dish);
        }
    }

    public void UpdateFrequency()
    {
        //spawnRate = (maxSpawnRate - minSpawnRate) * Mathf.Sin(Mathf.PI * (rm.restaurantRating + 1) / 2) + minSpawnRate; // - Sin(x) wave
        spawnRate = -(maxSpawnRate - minSpawnRate) * (rm.restaurantRating - 1) + minSpawnRate; //Linear graph
    }
}
