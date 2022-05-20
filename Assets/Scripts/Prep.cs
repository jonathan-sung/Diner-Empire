using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prep : MonoBehaviour
{

    private AIController ai;
    public bool pause;
    public string subtask;
    public string problem;
    public int problemNum;

    private Ingredient rawIngredient;
    public Ingredient targetIngredient;
    private Vector3 prepTableLocation;
    private Vector3 placeLocation;
    private Vector3 fridgeLocation;

    private int stage;
    /*
    1. go to fridge
    2. get ingredient
    3. check chop and seasoning
    4. go to prep table
    5. put ingredient into prep table
    6. check chop and chop accordingly 
    7. check seasoning and season accordingly
    8. check it matches with the desired ingredient
    9. retrieve ingredient from prep table
    10. go to worktop
    11. place ingredient at location
        */

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
            //go to fridge
            if (AtLocationOrActiveLocation(ai.unit.appliance, 5, fridgeLocation))
            {
                stage = 2;
            }
            else if (!ai.um.move)
            {
                ai.um.Move(fridgeLocation);
            }
        }
        if (stage == 2)
        {
            //get ingredient
            if (!ai.um.move && !AtLocationOrActiveLocation(ai.unit.appliance, 5, fridgeLocation))
            {
                stage = 1;
            }
            if (ai.unit.appliance != null && ai.unit.appliance.GetComponent<Fridge>() != null && ai.unit.appliance.GetComponent<Fridge>().GetIngredient(rawIngredient))
            {
                stage = 3;
            }
            else
            {
                //Inventory full or Fridge occupied
                ProblemHandling(2);
                return;
            }
            if (CheckUnitHasIngredient()) return;
        }
        if (stage == 3)
        {
            //check chop and seasoning
            int ingredientIndex = ScoreIngredient(targetIngredient);
            if (ingredientIndex < 0)
            {
                //not got raw ingredient
                stage = 2;
                return;
            }
            if (ingredientIndex >= 0 && CheckIngredient(targetIngredient, ai.inv.inventory[ingredientIndex] as Ingredient))
            {
                stage = 10;
                return;
            }
            stage = 4;
        }
        if (stage == 4)
        {
            //go to prep table
            if (CheckUnitHasIngredient()) return;
            if (AtLocationOrActiveLocation(ai.unit.appliance, 2, prepTableLocation))
            {
                stage = 5;
            }
            else if (!ai.um.move)
            {
                ai.um.Move(prepTableLocation);
            }

        }
        if (stage == 5)
        {
            //place ingredient into prep table
            if (!ai.um.move && !AtLocationOrActiveLocation(ai.unit.appliance, 2, prepTableLocation))
            {
                stage = 4;
            }
            if (ai.unit.appliance != null && ai.unit.appliance.GetComponent<PrepTable>() != null)
            {
                ai.unit.appliance.GetComponent<PrepTable>().Activate();
                if (ai.unit.appliance.GetComponent<PrepTable>().slot == null)
                {
                    ai.unit.appliance.GetComponent<PrepTable>().Activate();
                    int ingredientIndex = ScoreIngredient(targetIngredient);
                    if (ingredientIndex < 0)
                    {
                        //Debug.Log("not got raw ingredient");
                        stage = 2;
                        return;
                    }
                    else
                    {
                        ai.inv.ToggleSwap(ingredientIndex);
                        stage = 6;
                    }
                }
                else if (ai.unit.appliance.GetComponent<PrepTable>().slot != null && CheckIngredient(targetIngredient, ai.unit.appliance.GetComponent<PrepTable>().slot as Ingredient))
                {
                    ai.unit.appliance.GetComponent<PrepTable>().Activate();
                    stage = 6;
                }
                else
                {
                    if (!ai.unit.appliance.GetComponent<PrepTable>().AIPopIngredient())
                    {
                        ProblemHandling(2);
                        return;
                    }
                }

            }
        }
        if (stage == 6)
        {
            //check chop and chop accordingly
            if (ai.unit.appliance != null && ai.unit.appliance.GetComponent<PrepTable>() != null)
            {
                ai.unit.appliance.GetComponent<PrepTable>().Activate();
            }
            if (!ai.um.move && !AtLocationOrActiveLocation(ai.unit.appliance, 2, prepTableLocation))
            {
                stage = 4;
            }
            else if (ai.unit.appliance != null && ai.unit.appliance.GetComponent<PrepTable>() != null && CheckName(targetIngredient, ai.unit.appliance.GetComponent<PrepTable>().slot))
            {
                if (CheckChop(targetIngredient, ai.unit.appliance.GetComponent<PrepTable>().slot) == 0 && !ai.unit.appliance.GetComponent<PrepTable>().chopping)
                {
                    Chop();
                }
                else if (CheckChop(targetIngredient, ai.unit.appliance.GetComponent<PrepTable>().slot) == 1)
                {
                    stage = 7;
                }
                else if (CheckChop(targetIngredient, ai.unit.appliance.GetComponent<PrepTable>().slot) == -1)
                {
                    stage = 1;
                }
            }
            else if (ai.unit.appliance != null && ai.unit.appliance.GetComponent<PrepTable>() != null && !CheckName(targetIngredient, ai.unit.appliance.GetComponent<PrepTable>().slot) && ai.unit.appliance.GetComponent<PrepTable>().slot != null)
            {
                stage = 2;
                return;
            }
        }
        if (stage == 7)
        {
            //check seasoning and season accordingly
            if (ai.unit.appliance != null && ai.unit.appliance.GetComponent<PrepTable>() != null)
            {
                ai.unit.appliance.GetComponent<PrepTable>().Activate();
            }
            if (!ai.um.move && !AtLocationOrActiveLocation(ai.unit.appliance, 2, prepTableLocation))
            {
                stage = 4;
            }
            else if (ai.unit.appliance != null && ai.unit.appliance.GetComponent<PrepTable>() != null && CheckName(targetIngredient, ai.unit.appliance.GetComponent<PrepTable>().slot))
            {
                if (CheckSeasoning(targetIngredient, ai.unit.appliance.GetComponent<PrepTable>().slot))
                {
                    stage = 8;
                }
                else
                {
                    Season(targetIngredient);
                }
            }
            else if (ai.unit.appliance != null && ai.unit.appliance.GetComponent<PrepTable>() != null && !CheckName(targetIngredient, ai.unit.appliance.GetComponent<PrepTable>().slot) && ai.unit.appliance.GetComponent<PrepTable>().slot != null)
            {
                stage = 2;
                return;
            }
        }
        if (stage == 8)
        {
            //check it matches with the desired ingredient
            if (!ai.um.move && !AtLocationOrActiveLocation(ai.unit.appliance, 2, prepTableLocation))
            {
                stage = 4;
            }
            else if (ai.unit.appliance != null && ai.unit.appliance.GetComponent<PrepTable>() != null)
            {
                ai.unit.appliance.GetComponent<PrepTable>().Activate();
                if (CheckIngredient(targetIngredient, ai.unit.appliance.GetComponent<PrepTable>().slot))
                {
                    stage = 9;
                }
                else
                {
                    stage = 6;
                }

            }
        }
        if (stage == 9)
        {
            //retrieve ingredient from prep table
            if (ai.unit.appliance != null && ai.unit.appliance.GetComponent<PrepTable>() != null && CheckIngredient(targetIngredient, ai.unit.appliance.GetComponent<PrepTable>().slot))
            {
                ai.unit.appliance.GetComponent<PrepTable>().Activate();
                ai.unit.appliance.GetComponent<PrepTable>().AIPopIngredient(targetIngredient);
                placeLocation = FindClosestPlace();
                stage = 10;
            }
            else
            {
                stage = 6;
            }
        }
        if (stage == 10)
        {
            //go to worktop
            if (AtLocationOrActiveLocation(ai.unit.appliance, 4, placeLocation))
            {
                if (ai.unit.appliance != null && ai.unit.appliance.GetComponent<Worktop>() != null && ai.unit.appliance.GetComponent<Worktop>().slot == null)
                {
                    stage = 11;
                }
                else
                {
                    placeLocation = FindClosestPlace();
                    if (placeLocation == ai.worktopGroup.appliance[0].standingLocation)
                    {
                        stage = 11;
                    }
                }

            }
            else if (!ai.um.move)
            {
                ai.um.Move(placeLocation);
            }
        }
        if (stage == 11)
        {
            //place ingredient at location
            if (!ai.um.move && !AtLocationOrActiveLocation(ai.unit.appliance, 4, placeLocation))
            {
                stage = 10;
            }
            if (ai.unit.appliance != null && ai.unit.appliance.GetComponent<Worktop>() != null)
            {
                for (int i = 0; i < ai.inv.inventory.Length; i++)
                {
                    if (ai.inv.inventory[i] is Ingredient && CheckIngredient(targetIngredient, ((Ingredient)ai.inv.inventory[i])))
                    {
                        ai.unit.appliance.GetComponent<Worktop>().Activate();
                        if (ai.unit.appliance != null && ai.unit.appliance.GetComponent<Worktop>().slot != null)
                        {
                            if (FindClosestPlace() == ai.worktopGroup.appliance[0].standingLocation)
                            {
                                if (!ai.unit.appliance.GetComponent<Worktop>().AIPopIngredient())
                                {
                                    ProblemHandling(2);
                                    return;
                                }
                            }
                            else
                            {
                                placeLocation = FindClosestPlace();
                                stage = 10;
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
            stage = 1;
            return;
        }
    }

    bool CheckUnitHasIngredient()
    {
        int ingredientIndex = ScoreIngredient(targetIngredient);
        if (ingredientIndex >= 0 && CheckIngredient(targetIngredient, ai.inv.inventory[ingredientIndex] as Ingredient))
        {
            stage = 10;
            return true;
        }
        return false;
    }

    bool AtLocationOrActiveLocation(GameObject obj, int applianceType, Vector3 location)

    {
        //2 prep table, 3 cooker, 4 worktop, 5 fridge
        if (obj == null) return false;
        Appliance appliance = obj.GetComponent<Appliance>();
        if (appliance != null)
        {
            switch (applianceType)
            {
                case 2:
                    if (appliance is PrepTable && TransformCloseToLocation(location)) return true;
                    break;
                case 3:
                    if (appliance is Cooker && TransformCloseToLocation(location)) return true;
                    break;
                case 4:
                    if (appliance is Worktop && TransformCloseToLocation(location)) return true;
                    break;
                case 5:
                    if (appliance is Fridge && TransformCloseToLocation(location)) return true;
                    break;
            }
            return false;
        }
        else
        {
            return false;
        }
    }


    bool TransformCloseToLocation(Vector3 location)
    {
        float distanceX = Mathf.Abs((transform.position - (location + (new Vector3(0.5f, 0.5f, 0)))).x);
        float distanceY = (transform.position - (location + (new Vector3(0.5f, 0.5f, 0)))).y;
        if (distanceX <= 0.1f && distanceY >= 0 && distanceY <= 0.9f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private Vector3 FindClosestPlace()
    {
        int index = -1;
        float lowestDistance = 1000; //big distance
        for (int i = 0; i < ai.worktopGroup.appliance.Length; i++)
        {
            if (!(((Worktop)ai.worktopGroup.appliance[i]) is Bin) && !(((Worktop)ai.worktopGroup.appliance[i]) is Table) && ai.worktopGroup.item[i] == null && (transform.position - ai.worktopGroup.appliance[i].standingLocation).magnitude < lowestDistance)
            {
                lowestDistance = (transform.position - ai.worktopGroup.appliance[i].standingLocation).magnitude;
                index = i;
            }
        }
        if (index >= 0)
        {
            return ai.worktopGroup.appliance[index].standingLocation;
        }
        else
        {
            return ai.worktopGroup.appliance[0].standingLocation;
        }

    }

    private void Chop()
    {
        ai.unit.appliance.GetComponent<PrepTable>().Chop();
    }

    private void Season(Ingredient target)
    {
        int seasoningValue = 0;
        if (target.seasoning == Ingredient.Seasoning.Salt)
        {
            seasoningValue = 1;
        }
        else if (target.seasoning == Ingredient.Seasoning.Pepper)
        {
            seasoningValue = 2;
        }
        else if (target.seasoning == Ingredient.Seasoning.Chilli)
        {
            seasoningValue = 3;
        }
        ai.unit.appliance.GetComponent<PrepTable>().Season(seasoningValue);
        ai.unit.appliance.GetComponent<PrepTable>().choosenSeasoning = seasoningValue;
    }

    private bool CheckName(Ingredient a, Ingredient b)
    {
        if (a == null || b == null) return false;
        if (a.name == b.name)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private int CheckChop(Ingredient a, Ingredient b)
    {
        //-1 - invalid, 0 - not same, 1 - is same
        if (a == null || b == null) return 0;
        if (a.chop == b.chop)
        {
            return 1;
        }
        else if (b.chopNum > a.chopNum)
        {
            return -1;
        }
        else
        {
            return 0;
        }
    }
    private bool CheckSeasoning(Ingredient a, Ingredient b)
    {
        if (a == null || b == null) return false;
        if (a.seasoning == b.seasoning)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private bool CheckIngredient(Ingredient a, Ingredient b)
    {
        if (a == null || b == null) return false;
        if (CheckName(a, b) && CheckChop(a, b) == 1 && CheckSeasoning(a, b))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private int ScoreIngredient(Ingredient it) //compares all of the inventory ingredients to see which one most closely resembles the desired ingredient and gives a 'Comparison Score' 
    {
        int minIndex = -1;
        int minScore = 99;
        for (int i = 0; i < ai.inv.inventory.Length; i++)
        {
            if (ai.inv.inventory[i] != null && ai.inv.inventory[i] is Ingredient && ai.inv.inventory[i].name == it.name)
            {
                if (((Ingredient)ai.inv.inventory[i]).seasoning != it.seasoning && it.seasoning == Ingredient.Seasoning.None) continue;
                if (it.chopNum < ((Ingredient)ai.inv.inventory[i]).chopNum) continue;
                int currentScore = it.chopNum - ((Ingredient)ai.inv.inventory[i]).chopNum + 1;
                if (((Ingredient)ai.inv.inventory[i]).seasoning == it.seasoning) currentScore--;
                if (currentScore < 0) continue;
                if (currentScore < minScore)
                {
                    minScore = currentScore;
                    minIndex = i;
                }
            }
        }
        return minIndex;
    }

    public bool StartPrep(PrepInstruction instruction)
    {
        Reset();
        rawIngredient = instruction.ingredient;
        prepTableLocation = instruction.prepTableLocation;
        if (prepTableLocation == new Vector3(0, 0, -1)) prepTableLocation = ai.FindClosestAppliance(ai.prepTableGroup);
        placeLocation = FindClosestPlace();
        targetIngredient = new Ingredient(rawIngredient, instruction.seasoning, instruction.chop);
        fridgeLocation = ai.FindClosestAppliance(ai.fridgeGroup);
        if (rawIngredient == null || prepTableLocation == new Vector3(0, 0, -1) || placeLocation == new Vector3(0, 0, -1) || fridgeLocation == new Vector3(0, 0, -1))
        {
            return false;
        }
        stage = 1;
        return true;
    }

    public void Reset()
    {
        stage = 0;
        pause = false;
        subtask = null;
        problem = null;
        problemNum = 0;
        rawIngredient = null;
        targetIngredient = null;
        prepTableLocation = new Vector3(0, 0, -1);
        placeLocation = new Vector3(0, 0, -1);
        fridgeLocation = new Vector3(0, 0, -1);
    }

    private void ProblemHandling(int problemIndex)
    {
        problemNum = problemIndex;
        if (problemIndex == 1)
        {
            //if (ai.instructionInProgress) ai.Pause();
        }
    }

    public void CheckProblems()
    {
        switch (problemNum)
        {
            case 1:
                problem = "MI Stage: " + stage;
                break;
            case 2:
                problem = "IF or SO: " + stage;
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
                subtask = "Moving to Fridge";
                break;
            case 2:
                subtask = "Retrieving Ingredient";
                break;
            case 3:
                subtask = "Checking Ingredient";
                break;
            case 4:
                subtask = "Moving to Prep Table";
                break;
            case 5:
                subtask = "Placing Ingredient";
                break;
            case 6:
                subtask = "Chopping Ingredient";
                break;
            case 7:
                subtask = "Seasoning Ingredient";
                break;
            case 8:
                subtask = "Checking Ingredient";
                break;
            case 9:
                subtask = "Retrieving Ingredient";
                break;
            case 10:
                subtask = "Moving to Worktop";
                break;
            case 11:
                subtask = "Placing Ingredient";
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
