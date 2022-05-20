using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Serve : MonoBehaviour
{

    private AIController ai;
    public bool pause;
    public string subtask;
    public string problem;
    public int problemNum;

    public Dish dish;
    private Vector3 placeLocation;

    private Vector3 dishLocation;

    private int stage;
    /*
    1. Find dish
    2. Go to worktop
    3. Retrieve dish
    4. go to customer table
    5. place dish onto customer table
        */

    int searchingIndex;

    void Start()
    {
        ai = GetComponent<AIController>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckSubtask();
        CheckProblems();
        if (pause) return;
        problemNum = 0;
        if (stage == 1)
        {
            int foundDish = FindDish();
            if (foundDish < 0)
            {
                ProblemHandling(1);
            }
            else
            {
                if (foundDish < ai.inv.inventory.Length && CheckDish(ai.inv.inventory[foundDish]))
                {
                    stage = 4;
                }
                else
                {
                    dishLocation = ai.worktopGroup.appliance[foundDish].standingLocation;
                    stage = 2;
                }
            }
        }
        if (stage == 2)
        {
            if ((transform.position + new Vector3(-0.5f, -0.5f, 0)) == dishLocation)
            {
                stage = 3;
            }
            else if (!ai.um.move)
            {
                ai.um.Move(dishLocation);
            }
        }
        if (stage == 3)
        {
            if (!ai.um.move && (transform.position + new Vector3(-0.5f, -0.5f, 0)) != dishLocation)
            {
                stage = 2;
            }
            else if (ai.unit.appliance != null && ai.unit.appliance.GetComponent<Worktop>() && CheckDish(ai.unit.appliance.GetComponent<Worktop>().slot))
            {
                if (ai.unit.appliance.GetComponent<Worktop>().AIPopIngredient())
                {
                    ResetSearching();
                    stage = 4;
                }
                else
                {
                    ProblemHandling(2);
                }
            }
            else
            {
                ProblemHandling(1);
                stage = 1; //stage = 1 to find dish
            }
        }
        if (stage == 4)
        {
            if ((transform.position + new Vector3(-0.5f, -0.5f, 0)) == placeLocation)
            {
                stage = 5;
            }
            else if (!ai.um.move)
            {
                ai.um.Move(placeLocation);
            }
        }
        if (stage == 5)
        {
            if (!ai.um.move && (transform.position + new Vector3(-0.5f, -0.5f, 0)) != placeLocation)
            {
                stage = 4;
            }
            if (ai.unit.appliance != null && ai.unit.appliance.GetComponent<Worktop>() != null)
            {
                for (int i = 0; i < ai.inv.inventory.Length; i++)
                {
                    if (CheckDish(ai.inv.inventory[i]))
                    {
                        ai.unit.appliance.GetComponent<Worktop>().Activate();
                        if (ai.unit.appliance != null && ai.unit.appliance.GetComponent<Worktop>().slot != null)
                        {
                            if (!ai.unit.appliance.GetComponent<Worktop>().AIPopIngredient())
                            {
                                ProblemHandling(2);
                                return;
                            }
                        }
                        else
                        {
                            ai.inv.ToggleSwap(i);
                            Reset();
                            ai.EndInstruction();
                            return;
                        }
                    }
                }
            }
            ProblemHandling(1);
        }
    }

    private int FindDish()
    {
        ResetSearching();
        for (int i = 0; i < ai.inv.inventory.Length; i++)
        {
            if (ai.inv.inventory[i] != null && CheckDish(ai.inv.inventory[i]))
            {
                return i;
            }
        }
        for (int i = 0; i < ai.worktopGroup.item.Length; i++)
        {
            if (ai.worktopGroup.item[i] != null && (ai.worktopGroup.appliance[i] as Table) == null && CheckDish(ai.worktopGroup.item[i]) && !ai.worktopGroup.searching[i])
            {
                ai.worktopGroup.searching[i] = true;
                searchingIndex = i;
                return i;
            }
        }
        return -1;
    }

    private bool CheckDish(Item it)
    {
        if (it is Dish && ((Dish)it).name == dish.name)
        {
            return true;
        }
        return false;
    }

    public bool StartServe(ServeInstruction instruction)
    {
        Reset();
        dish = instruction.dish;
        //dishLocation = instruction.fromLocation;
        placeLocation = instruction.placeLocation;
        if (dish == null || placeLocation == new Vector3(0, 0, -1))
        {
            return false;
        }
        stage = 1; //stage = 1 to find dish, stage = 2 to use given location
        return true;
    }

    public void Reset()
    {
        stage = 0;
        pause = false;
        subtask = null;
        problem = null;
        problemNum = 0;
        dish = null;
        placeLocation = new Vector3(0, 0, -1);
        dishLocation = new Vector3(0, 0, -1);
        ResetSearching();
    }

    void ResetSearching()
    {
        if (searchingIndex >= 0) ai.worktopGroup.searching[searchingIndex] = false;
        searchingIndex = -1;
    }

    private void ProblemHandling(int problemIndex)
    {
        problemNum = problemIndex;
        ai.um.Move(ai.selectedIdle);
        if (problemIndex == 2)
        {
            if (ai.instructionInProgress) ai.Pause();
        }
    }

    public void CheckProblems()
    {
        switch (problemNum)
        {
            case 1:
                problem = "Missing Dish";
                break;
            case 2:
                problem = "Inventory Full";
                break;
            default:
                problem = "";
                break;

        }
    }

    public void CheckSubtask()
    {
        switch (stage)
        {
            case 1:
                subtask = "Searching for Dish";
                break;
            case 2:
                subtask = "Moving to Dish";
                break;
            case 3:
                subtask = "Retrieving Dish";
                break;
            case 4:
                subtask = "Moving to Table";
                break;
            case 5:
                subtask = "Placing Dish";
                break;
            default:
                subtask = "";
                break;
        }
    }

    public void TogglePause()
    {
        pause = (pause) ? false : true;
    }
}
