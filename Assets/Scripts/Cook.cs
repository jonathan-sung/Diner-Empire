using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cook : MonoBehaviour
{

    private AIController ai;
    public bool pause;

    public Recipe recipe;
    private Vector3 cookerLocation;
    private Vector3 placeLocation;

    private Ingredient[] ingredient = new Ingredient[3];
    private Vector3[] ingredientLocation = new Vector3[3];
    private int[] ingredientPresent = new int[3]; //0 - not present, 1 - in inventory, 2 - at location

    private int stage; //what step of the progress of cooking the unit is at
    /*
    0-inactive
    1-extraction
    2-search for ingredients
    3-retrieve ingredients
    4-move to cooker
    5-place ingredients into cooker
    6-retrieve dish
    7-move to place location
    8-place dish
        */

    private int currentIngredient;
    private bool gotDish;

    private bool[] ingredientsInCooker = new bool[3];

    public string subtask;
    public string problem;

    public int problemNum;

    List<int> worktopIndexes = new List<int>();

    void Start()
    {
        ai = GetComponent<AIController>();
    }

    void Update()
    {
        CheckSubtask();
        CheckProblems();
        if (pause) return;
        problemNum = 0;
        if (stage == 1)
        {
            ExtractIngredients();
            stage = 2;
        }
        if (stage == 2)
        {
            ResetWorktopIndexes();
            if (GetIngredientLocations())
            {
                stage = 3;
                currentIngredient = -1;
            }
            else
            {
                ProblemHandling(1);//Missing Ingredient
                if (!ai.um.move && !TransformCloseToLocation(ai.selectedIdle))
                {
                    ai.um.Move(ai.selectedIdle);
                }
            }
        }
        if (stage == 3)
        {
            if (currentIngredient == -1 || (AtLocationOrActiveLocation(ai.unit.appliance, 4, ingredientLocation[currentIngredient])) || ingredientPresent[currentIngredient] == 1)
            {
                if (currentIngredient >= 0 && (AtLocationOrActiveLocation(ai.unit.appliance, 4, ingredientLocation[currentIngredient])) && ingredientPresent[currentIngredient] == 2)
                {
                    if (ai.unit.appliance != null && ai.unit.appliance.GetComponent<Worktop>() != null) //if another unit is occupying the appliance
                    {
                        if (ai.unit.appliance.GetComponent<Worktop>().slot != null && ai.unit.appliance.GetComponent<Worktop>().slot is Ingredient && SameIngredient((Ingredient)ai.unit.appliance.GetComponent<Worktop>().slot, ingredient[currentIngredient]))
                        {
                            ai.unit.appliance.GetComponent<Worktop>().Activate();
                            if (!ai.unit.appliance.GetComponent<Worktop>().AIPopIngredient(ingredient[currentIngredient]))
                            {
                                ProblemHandling(2);
                                return;
                            }
                            else
                            {
                                ResetCurrentWorktopIndex();
                            }
                            ai.unit.appliance.GetComponent<Worktop>().Deactivate();
                        }
                        else
                        {
                            ProblemHandling(1);
                            return;
                        }
                    }
                    else
                    {
                        ProblemHandling(3);
                        return;
                    }
                }
                currentIngredient++;
            }
            if (!ai.um.move && currentIngredient < 3 && (!AtLocationOrActiveLocation(ai.unit.appliance, 4, ingredientLocation[currentIngredient])) && ingredientPresent[currentIngredient] == 2)
            {
                MoveToIngredients();
            }
            if (currentIngredient >= 3)
            {
                stage = 4;
            }
        }
        if (stage == 4)
        {
            if (!ai.um.move && !AtLocationOrActiveLocation(ai.unit.appliance, 3, cookerLocation))
            {
                MoveToCooker();
            }
            if (AtLocationOrActiveLocation(ai.unit.appliance, 3, cookerLocation))
            {
                stage = 5;
            }
        }
        if (stage == 5)
        {
            if (!ai.um.move && !AtLocationOrActiveLocation(ai.unit.appliance, 3, cookerLocation))
            {
                stage = 4;
            }
            if (ai.unit.appliance != null && ai.unit.appliance.GetComponent<Cooker>() != null)
            {
                ai.unit.appliance.GetComponent<Cooker>().Activate();
                int iic = IngredientsInCooker(ai.unit.appliance.GetComponent<Cooker>());
                int iii = IngredientsInInventory();
                if (iic >= 0 && (iic + iii) < ingredient.Length)
                {
                    ProblemHandling(1);
                    return;
                }
                if (iic >= 0)
                {
                    PutIngredientsInCooker();
                }
                else if (iic == -1)
                {
                    ProblemHandling(4);
                    return;
                }
                if (iic >= 3)
                {
                    stage = 6;
                    ai.unit.appliance.GetComponent<Cooker>().StartCook();
                }
            }
            else
            {
                ProblemHandling(3);
                return;
            }
        }
        if (stage == 6)
        {
            if (!ai.um.move && !AtLocationOrActiveLocation(ai.unit.appliance, 3, cookerLocation))
            {
                stage = 4;
                for (int i = 0; i < ingredientsInCooker.Length; i++)
                {
                    ingredientsInCooker[i] = false;
                }
                return;
            }
            if (ai.unit.appliance != null && ai.unit.appliance.GetComponent<Cooker>() != null && ai.unit.appliance.GetComponent<Cooker>().output != null)
            {
                if (ai.inv.Full())
                {
                    ProblemHandling(5);
                    return;
                }
                if (!ai.inv.Full())
                {
                    ai.unit.appliance.GetComponent<Cooker>().ToggleSwap(3);
                    gotDish = true;
                }

            }
            if (gotDish)
            {
                for (int i = 0; i < ai.inv.inventory.Length; i++)
                {
                    if (ai.inv.inventory[i] != null && ai.inv.inventory[i] is Dish && ((Dish)ai.inv.inventory[i]).name == recipe.dish.name)
                    {
                        stage = 7;
                        placeLocation = FindClosestPlace();
                        MoveToPlace();
                    }
                }
            }
            if (ai.unit.appliance != null && ai.unit.appliance.GetComponent<Cooker>() != null && !ai.unit.appliance.GetComponent<Cooker>().cooking && !gotDish)
            {
                stage = 4;
                for (int i = 0; i < ingredientsInCooker.Length; i++)
                {
                    ingredientsInCooker[i] = false;
                }
                return;
            }
        }
        if (stage == 7)
        {
            if (!ai.um.move && !AtLocationOrActiveLocation(ai.unit.appliance, 4, placeLocation))
            {
                MoveToPlace();
            }
            if (AtLocationOrActiveLocation(ai.unit.appliance, 4, placeLocation))
            {
                stage = 8;
            }
        }
        if (stage == 8)
        {
            if (!ai.um.move && !AtLocationOrActiveLocation(ai.unit.appliance, 4, placeLocation))
            {
                MoveToPlace();
            }
            if (ai.unit.appliance != null && ai.unit.appliance.GetComponent<Worktop>() != null)
            {
                ai.unit.appliance.GetComponent<Worktop>().Activate();
            }
            for (int i = 0; i < ai.inv.inventory.Length; i++)
            {

                if (ai.inv.inventory[i] is Dish && ((Dish)ai.inv.inventory[i]).name == recipe.dish.name)
                {
                    if (ai.unit.appliance != null && ai.unit.appliance.GetComponent<Worktop>().slot != null)
                    {
                        if (!ai.unit.appliance.GetComponent<Worktop>().AIPopIngredient())
                        {
                            ProblemHandling(5);
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
            ProblemHandling(6);
        }
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
        float distanceY = Mathf.Abs((transform.position - (location + (new Vector3(0.5f, 0.5f, 0)))).y);
        if (distanceX <= 0.1f && distanceY <= 0.9f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void ProblemHandling(int problemIndex)
    {
        problemNum = problemIndex;
        if (problemIndex == 1)
        {
            stage = 2;
        }
        else if (problemIndex == 2)
        {
            stage = 2;
            if (ai.instructionInProgress) ai.Pause();
        }
        else if (problemIndex == 4 || problemIndex == 5 || problemIndex == 6)
        {
            if (ai.instructionInProgress) ai.Pause();
        }


    }

    public void CheckProblems()
    {
        switch (problemNum)
        {
            case 1:
                problem = "Missing Ingredient";
                break;
            case 2:
                problem = "Inventory Full / Item Missing";
                break;
            case 3:
                problem = "Space occupied";
                break;
            case 4:
                problem = "Wrong Ingredient in Cooker";
                break;
            case 5:
                problem = "Inventory Full or Missing Dish";
                break;
            case 6:
                problem = "Missing Dish";
                break;
            case 7:
                problem = "Not cooking dish";
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
            case 0:
                subtask = "";
                break;
            case 1:
                subtask = "Extracting Ingredients";
                break;
            case 2:
                subtask = "Searching for Ingredients";
                break;
            case 3:
                subtask = "Retrieving Ingredients";
                break;
            case 4:
                subtask = "Moving to Cooker";
                break;
            case 5:
                subtask = "Cooking Dish";
                break;
            case 6:
                subtask = "Retrieving Dish";
                break;
            case 7:
                subtask = "Moving to Place Location";
                break;
            case 8:
                subtask = "Placing Ingredient";
                break;
        }
    }

    public void TogglePause()
    {
        pause = (pause) ? false : true;
    }

    public bool StartCook(CookInstruction instruction)
    {
        Reset();
        recipe = instruction.recipe;
        cookerLocation = instruction.cookerLocation;
        if (cookerLocation == new Vector3(0, 0, -1)) cookerLocation = ai.FindClosestAppliance(ai.cookerGroup);
        placeLocation = FindClosestPlace();
        if (recipe == null || cookerLocation == new Vector3(0, 0, -1) || placeLocation == new Vector3(0, 0, -1))
        {
            return false;
        }
        stage = 1;
        return true;
    }

    public void Reset()
    {
        stage = 0;
        currentIngredient = 0;
        pause = false;
        gotDish = false;
        recipe = null;
        cookerLocation = new Vector3(0, 0, -1);
        placeLocation = new Vector3(0, 0, -1);
        for (int i = 0; i < ingredient.Length; i++)
        {
            ingredient[i] = null;
            ingredientLocation[i] = new Vector3(0, 0, -1);
            ingredientPresent[i] = 0;
            ingredientsInCooker[i] = false;
        }
        ResetWorktopIndexes();
    }

    void ResetWorktopIndexes()
    {
        for (int i = 0; i < worktopIndexes.Count; i++)
        {
            ai.worktopGroup.searching[worktopIndexes[i]] = false;
        }
        worktopIndexes.Clear();
    }

    void ResetCurrentWorktopIndex()
    {
        if (worktopIndexes.Count > 0)
        {
            ai.worktopGroup.searching[worktopIndexes[0]] = false;
            worktopIndexes.RemoveAt(0);
        }
    }

    private Vector3 FindClosestPlace()
    {
        int index = -1;
        float lowestDistance = 1000; //big distance
        for (int i = 0; i < ai.worktopGroup.appliance.Length; i++)
        {
            //Debug.Log("Index: " + i + " Item: " + ai.worktopGroup.item[i]);
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

    private void ExtractIngredients()
    {
        for (int i = 0; i < ingredient.Length; i++)
        {
            ingredient[i] = recipe.ingredients[i];
        }
    }

    private bool GetIngredientLocations()
    {
        for (int j = 0; j < ingredient.Length; j++)
        {
            ingredientPresent[j] = 0;
            Vector4 ingredientLocationAndIndex = ai.worktopGroup.FindIngredient(ingredient[j]);
            ingredientLocation[j] = ingredientLocationAndIndex;
            int currentWorktopIndex = (int)ingredientLocationAndIndex.w;
            for (int i = 0; i < ai.inv.inventory.Length; i++)
            {
                if (ai.inv.inventory[i] is Ingredient && SameIngredient(ingredient[j], (Ingredient)ai.inv.inventory[i]))
                {
                    ingredientPresent[j] = 1;
                    break;
                }
            }
            if (ingredientLocation[j] != new Vector3(0, 0, -1) && ingredientPresent[j] == 0)
            {
                ingredientPresent[j] = 2;
                ai.worktopGroup.searching[currentWorktopIndex] = true;
                worktopIndexes.Add(currentWorktopIndex);
            }
        }
        for (int i = 0; i < ingredient.Length; i++)
        {
            if (ingredientPresent[i] == 0) return false;
        }
        SortClosestIngredients();
        return true;
    }

    void SortClosestIngredients()
    {
        int swaps = 0;
        do
        {
            swaps = 0;
            for (int i = 0; i < ingredient.Length - 1; i++)
            {
                if (GetDistance(transform.position, ingredientLocation[i + 1]) < GetDistance(transform.position, ingredientLocation[i]))
                {
                    SwapIngredientVariables(i + 1, i);
                    swaps++;
                }
            }
        } while (swaps > 0);
    }

    void SwapIngredientVariables(int a, int b)
    {
        Swap(ref ingredientLocation[a], ref ingredientLocation[b]);
        Swap(ref ingredient[a], ref ingredient[b]);
        Swap(ref ingredientPresent[a], ref ingredientPresent[b]);
        Swap(ref worktopIndexes, a, b);
        Swap(ref ingredientsInCooker[a], ref ingredientsInCooker[b]);
    }

    void Swap(ref Vector3 a, ref Vector3 b)
    {
        Vector3 temp = a;
        a = b;
        b = temp;
    }

    void Swap(ref Ingredient a, ref Ingredient b)
    {
        Ingredient temp = a;
        a = b;
        b = temp;
    }

    void Swap(ref int a, ref int b)
    {
        int temp = a;
        a = b;
        b = temp;
    }

    void Swap(ref List<int> l, int a, int b)
    {
        if (a < l.Count && b < l.Count)
        {
            int temp = l[a];
            l[a] = l[b];
            l[b] = temp;
        }
    }

    void Swap(ref bool a, ref bool b)
    {
        bool temp = a;
        a = b;
        b = temp;
    }

    float GetDistance(Vector3 a, Vector3 b)
    {
        return Mathf.Abs(a.y - b.y) + Mathf.Abs(a.x - b.x);
    }

    private void MoveToIngredients()
    {
        if (ingredientPresent[currentIngredient] == 2)
        {
            ai.um.Move(ingredientLocation[currentIngredient]);
        }
    }

    private void MoveToCooker()
    {
        ai.um.Move(cookerLocation);
    }

    private void MoveToPlace()
    {
        ai.um.Move(placeLocation);
    }

    private bool PutIngredientsInCooker()
    {
        for (int j = 0; j < ingredient.Length; j++)
        {
            for (int i = 0; i < ai.inv.inventory.Length; i++)
            {
                if (ai.inv.inventory[i] is Ingredient && SameIngredient(ingredient[j], (Ingredient)ai.inv.inventory[i]) && !ingredientsInCooker[j])
                {
                    ai.inv.ToggleSwap(i);
                    return true;
                }
            }
        }
        return false;
    }

    private int IngredientsInInventory()
    {
        bool[] present = new bool[ingredient.Length];
        int numOfIngredients = 0;
        for (int j = 0; j < ingredient.Length; j++)
        {
            for (int i = 0; i < ai.inv.inventory.Length; i++)
            {
                if (ai.inv.inventory[i] is Ingredient && SameIngredient(ingredient[j], (Ingredient)ai.inv.inventory[i]))
                {
                    present[j] = true;
                    numOfIngredients++;
                    break;
                }
            }
        }
        return numOfIngredients;
    }

    private int IngredientsInCooker(Cooker cooker)
    {
        //return -1 - there is an incorrect ingredient in the cooker
        //return 0 or greater - number of correct ingredients in cooker
        int present = 0;
        for (int j = 0; j < ingredient.Length; j++)
        {
            for (int i = 0; i < cooker.ingredients.Length; i++)
            {
                if (cooker.ingredients[i] != null && SameIngredient(ingredient[j], cooker.ingredients[i]))
                {
                    ingredientsInCooker[j] = true;
                    break;
                }
            }
            if (ingredientsInCooker[j]) present++;
        }

        for (int j = 0; j < cooker.ingredients.Length; j++)
        {
            int wrongIngredient = 0;
            for (int i = 0; i < ingredient.Length; i++)
            {
                if (cooker.ingredients[i] != null && !SameIngredient(ingredient[i], cooker.ingredients[j]))
                {
                    wrongIngredient++;
                }
            }
            if (wrongIngredient >= 3) return -1;
        }
        return present;
    }

    private bool SameIngredient(Ingredient a, Ingredient b)
    {
        if (a != null && b != null)
        {
            if (a.name == b.name && a.seasoning == b.seasoning && a.chop == b.chop)
            {
                return true;
            }

        }
        return false;
    }
}
