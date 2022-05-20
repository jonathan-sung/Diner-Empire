using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapComponent : MonoBehaviour
{

    public Tilemap[] tm;
    public ApplianceGroup[] applianceGroups;
    public static bool tilesInitialised;
    public RestaurantManager restaurantManager;
    // Use this for initialization
    void Start()
    {
        restaurantManager = GameObject.Find("Restaurant Manager").GetComponent<RestaurantManager>();
        AssignComponentsToTiles();
        StartApplianceGroups();
    }

    void StartApplianceGroups()
    {
        if (applianceGroups == null) { Debug.Log("No appliance groups found"); return; }
        for (int i = 0; i < applianceGroups.Length; i++)
        {
            applianceGroups[i].Init(restaurantManager.customerSpawnGrouping);
        }
        tilesInitialised = true;
    }

    private void AssignComponentsToTiles()
    {
        for (int k = 0; k < tm.Length; k++)
        {
            for (int j = tm[k].cellBounds.yMin; j <= tm[k].cellBounds.yMax; j++)
            {
                for (int i = tm[k].cellBounds.xMin; i <= tm[k].cellBounds.xMax; i++)
                {
                    Vector3Int position = new Vector3Int(i, j, 0);
                    Sprite s = tm[k].GetSprite(position);
                    if (s != null)
                    {
                        if (s.name == "cooker_1")
                        {
                            GameObject g = (GameObject)Instantiate(Resources.Load("Prefabs/Cooker"));
                            g.transform.position = position + new Vector3(0.5f, 0.5f, 0);
                            g.GetComponent<Cooker>().location = position;
                        }
                        else if (s.name == "fridge_2")
                        {
                            GameObject g = (GameObject)Instantiate(Resources.Load("Prefabs/Fridge"));
                            g.transform.position = position + new Vector3(0.5f, 0.5f, 0);
                            g.GetComponent<Fridge>().location = position;
                            g.GetComponent<Fridge>().rm = restaurantManager;
                            g.GetComponent<Fridge>().SetupContents(restaurantManager);
                        }
                        else if (s.name == "prep_table_1")
                        {
                            GameObject g = (GameObject)Instantiate(Resources.Load("Prefabs/Prep Table"));
                            g.transform.position = position + new Vector3(0.4f, 0.8f, 0);
                            g.GetComponent<PrepTable>().location = position;
                        }
                        else if (s.name == "worktop_1" || s.name == "worktop_2" || s.name == "worktop_3")
                        {
                            GameObject g = (GameObject)Instantiate(Resources.Load("Prefabs/Worktop"));
                            g.transform.position = position + new Vector3(0.5f, 0.78f, 0);
                            g.GetComponent<Worktop>().location = position;
                        }
                        else if (s.name == "worktop_4")
                        {
                            GameObject g = (GameObject)Instantiate(Resources.Load("Prefabs/Worktop"));
                            g.transform.position = position + new Vector3(0.5f, -0.25f, 0);
                            g.GetComponent<Worktop>().location = position;
                            g.GetComponent<Worktop>().foregroundType = true;
                        }
                        else if (s.name == "bin")
                        {
                            GameObject g = (GameObject)Instantiate(Resources.Load("Prefabs/Bin"));
                            g.transform.position = position + new Vector3(0.5f, 0.78f, 0);
                            g.GetComponent<Bin>().location = position;
                        }
                        else if (s.name == "table_2" || s.name == "table_3" || s.name == "table_5")
                        {
                            GameObject g = (GameObject)Instantiate(Resources.Load("Prefabs/Table"));
                            g.transform.position = position + new Vector3(0.5f, 0.78f, 0);
                            g.GetComponent<Table>().location = position;
                        }
                        else if (s.name == "table_1" || s.name == "table_4" || s.name == "table_6")
                        {
                            GameObject g = (GameObject)Instantiate(Resources.Load("Prefabs/Table"));
                            g.transform.position = position + new Vector3(0.5f, 0.45f, 0);
                            g.GetComponent<Table>().location = position;
                            g.GetComponent<Table>().foregroundType = true;
                        }
                        else if (s.name == "holding_station")
                        {
                            GameObject g = (GameObject)Instantiate(Resources.Load("Prefabs/HoldingStation"));
                            g.transform.position = position + new Vector3(0.5f, 0.75f, 0);
                            g.GetComponent<HoldingStation>().location = position;
                            GameObject h = (GameObject)Instantiate(Resources.Load("Prefabs/HoldingStationCover"));
                            h.transform.position = position + new Vector3(0.5f, 0.5f, 0);

                        }
                    }

                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
