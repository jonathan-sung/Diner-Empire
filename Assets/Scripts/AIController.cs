using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIController : MonoBehaviour
{
    public GameObject[] menu;
    /*
    0 - select instruction
    1 - dishes
    2 - cooker
    3 - location
    4 - confirm
    5 - ingredients
    6 - chop
    7 - seasoning
    8 - progress
    9 - prep table
    10 - quantity
    11 - settings
    12 - idle
    13 - stats
    14 - from location (obsolete)
    15 - instruction list
        */

    public UnitMovement um;
    public Unit unit;
    public Inventory inv;
    public Canvas canvas;
    public CanvasScaler canvasScaler;

    public ApplianceGroup worktopGroup;
    public ApplianceGroup cookerGroup;
    public ApplianceGroup prepTableGroup;
    public ApplianceGroup fridgeGroup;

    public int currentMenu;
    public int currentTask; //0 - nothing, 1 - prep, 2 - cook, 3 - serve
    public int nextMenu;
    public GameObject arrow;

    public bool instructionInProgress;
    public bool selectCooker;
    public bool selectPrepTable;
    public bool selectLocation;
    private bool selectIdle;
    private bool confirm;

    private Recipe selectedDish;
    private Ingredient selectedIngredient;
    private Vector3 selectedCooker;
    private Vector3 selectedPrepTable;
    private Vector3 selectedLocation;
    public Vector3 selectedIdle;

    private bool cookerSelected = false;
    private bool prepTableSelected = false;
    private bool locationSelected = false;
    private bool idleSelected = false;

    public string task;
    public string subtask;
    public string problems;

    public Text taskText;
    public Text subtaskText;
    public Text problemsText;

    public Image play_icon;
    public Image play_light;

    private Cook cook;
    private Prep prep;
    private Serve serve;

    private Ingredient.Chop chop;
    private Ingredient.Seasoning seasoning;
    private int quantity;

    public Button[] inventoryButton = new Button[4];

    public Ingredient[] contents = new Ingredient[12];
    public Image[] ingredients = new Image[12];
    public Image[] dishes = new Image[9];
    public Text quantityText;
    public SpriteRenderer problem_sign;

    public RestaurantManager rm;

    bool appliancesSelected;

    public GameObject instructionMenu;
    public GameObject minimisedTab;
    bool minimised;
    bool hasRestaurant;

    bool paused;

    List<Instruction> instruction = new List<Instruction>();
    public Text[] instructionDescription;

    public GameObject tileOverlay;

    public Text numberOfInstructions;

    void Start()
    {
        worktopGroup = GameObject.Find("Worktop Group").GetComponent<ApplianceGroup>();
        cookerGroup = GameObject.Find("Cooker Group").GetComponent<ApplianceGroup>();
        prepTableGroup = GameObject.Find("Prep Table Group").GetComponent<ApplianceGroup>();
        fridgeGroup = GameObject.Find("Fridge Group").GetComponent<ApplianceGroup>();
        um = GetComponent<UnitMovement>();
        unit = GetComponent<Unit>();
        cook = GetComponent<Cook>();
        prep = GetComponent<Prep>();
        serve = GetComponent<Serve>();
        inv = GetComponent<Inventory>();
        canvasScaler.enabled = true;
        instructionInProgress = false;
        currentMenu = -1;
        SelectMenu(15);
        canvas.enabled = false;
        selectedCooker = FindClosestAppliance(cookerGroup);
        selectedPrepTable = FindClosestAppliance(prepTableGroup);
        selectedIdle = transform.position;
        tileOverlay.SetActive(false);
    }

    void Update()
    {
        if (!hasRestaurant)
        {
            rm = GameObject.Find("Restaurant Manager").GetComponent<RestaurantManager>();
            if (rm)
            {
                hasRestaurant = true;
                rm.ai.Add(this);
                SetupIngredients();
                SetupDishes();
            }
        }
        if (!appliancesSelected && TilemapComponent.tilesInitialised)
        {
            selectedCooker = FindClosestAppliance(cookerGroup);
            selectedPrepTable = FindClosestAppliance(prepTableGroup);
            appliancesSelected = true;
        }
        if (unit.selected)
        {
            canvas.enabled = true;
            UpdateMinimised();
        }
        else
        {
            canvas.enabled = false;
        }
        if (currentTask == 0)
        {
            if (selectCooker && unit.selected) SelectCooker();
            if (selectPrepTable && unit.selected) SelectPrepTable();
            if (selectLocation && unit.selected) SelectLocation();
            if (selectIdle && unit.selected) SelectIdle();
        }
        else if (currentTask == 3)
        {
            if (selectLocation && unit.selected) SelectLocation();
        }
        if (prep.problemNum != 0 || cook.problemNum != 0 || serve.problemNum != 0)
        {
            problem_sign.color = new Color(1, 1, 1, 1);
        }
        else
        {
            problem_sign.color = new Color(1, 1, 1, 0);
        }

        if (currentMenu == 10)
        {
            quantityText.text = quantity.ToString();
        }
        if (currentMenu == 13)
        {
            unit.UpdateStatsUI();
        }
        if (currentMenu == 15)
        {
            UpdateInstructionText();
            UpdatePauseSprite();
        }
        if (currentMenu == 8)
        {
            if (instruction.Count != 0 && instruction[0] is PrepInstruction)
            {
                if (prep.targetIngredient != null) task = prep.targetIngredient.chop + " " + prep.targetIngredient.name + " : " + prep.targetIngredient.seasoning;
                if (prep.subtask != null) subtask = prep.subtask;
                if (prep.problem != null) problems = prep.problem;
            }
            else if (instruction.Count != 0 && instruction[0] is CookInstruction)
            {
                if (cook.recipe.dish != null) task = cook.recipe.dish.name;
                if (cook.subtask != null) subtask = cook.subtask;
                if (cook.problem != null) problems = cook.problem;
            }
            else if (instruction.Count != 0 && instruction[0] is ServeInstruction)
            {
                if (serve.dish != null) task = "Serving: " + serve.dish.name;
                if (serve.subtask != null) subtask = serve.subtask;
                if (serve.problem != null) problems = serve.problem;
            }
            taskText.text = task;
            subtaskText.text = subtask;
            problemsText.text = problems;
        }
        if (currentMenu != nextMenu)
        {
            MenuEnable(nextMenu);
            CheckMenuOptions();
            ArrowEnable();
        }
        if (instructionInProgress)
        {
            for (int i = 0; i < inventoryButton.Length; i++)
            {
                inventoryButton[i].interactable = false;
            }
        }
        else
        {
            for (int i = 0; i < inventoryButton.Length; i++)
            {
                inventoryButton[i].interactable = true;
            }
        }
        StartNextInstruction();
        if ((selectCooker || selectPrepTable || selectLocation || selectIdle) && unit.selected) { tileOverlay.SetActive(true); } else { tileOverlay.SetActive(false); }
        numberOfInstructions.text = instructionLength.ToString();
    }

    void UpdateMinimised()
    {
        if (minimised)
        {
            minimisedTab.SetActive(true);
            instructionMenu.SetActive(false);
        }
        else
        {
            minimisedTab.SetActive(false);
            instructionMenu.SetActive(true);
        }
    }

    public void ToggleMinimise()
    {
        minimised = (minimised) ? false : true;
    }

    public void SetupIngredients()
    {
        for (int i = 0; i < rm.ingredientList.Length; i++)
        {
            if (i >= contents.Length) break;
            contents[i] = rm.ingredientList[i];
        }
        for (int i = 0; i < ingredients.Length; i++)
        {
            if (contents[i] != null && ingredients[i] != null)
            {
                ingredients[i].sprite = contents[i].sprite;
                ingredients[i].color = new Color(1, 1, 1, 1);
            }
        }
    }

    public void SetupDishes()
    {
        for (int i = 0; i < dishes.Length; i++)
        {
            if (i >= rm.recipeList.Length) break;
            if (rm.recipeList[i] != null && dishes[i] != null)
            {
                dishes[i].sprite = rm.recipeList[i].dish.sprite;
                dishes[i].color = new Color(1, 1, 1, 1);
            }
        }
    }

    public void SelectMenu(int index)
    {
        nextMenu = index;
    }

    public void SelectDish(int dish, int type)
    {
        if (dish >= 0 && dish < rm.recipeList.Length && rm.recipeList[dish] != null)
        {
            selectedDish = rm.recipeList[dish];
            if (type == 2)
            {
                SelectMenu(10);
                ResetQuantity();
            }
            else if (type == 3)
            {
                SelectMenu(3);
            }

        }
    }

    public void SelectIngredient(int ingredient)
    {
        if (contents[ingredient - 1] != null)
        {
            selectedIngredient = contents[ingredient - 1];
            SelectMenu(6);
        }
    }
    public void SelectChop(int c)
    {
        //0 none, 1 chopped, 2 diced, 3 minced
        switch (c)
        {
            case 0:
                chop = Ingredient.Chop.None;
                SelectMenu(7);
                break;
            case 1:
                chop = Ingredient.Chop.Chopped;
                SelectMenu(7);
                break;
            case 2:
                chop = Ingredient.Chop.Diced;
                SelectMenu(7);
                break;
            case 3:
                chop = Ingredient.Chop.Minced;
                SelectMenu(7);
                break;
        }
    }

    public void SelectSeasoning(int s)
    {
        //0 none, 1 chopped, 2 diced, 3 minced
        switch (s)
        {
            case 0:
                seasoning = Ingredient.Seasoning.None;
                SelectMenu(10);
                ResetQuantity();
                break;
            case 1:
                seasoning = Ingredient.Seasoning.Salt;
                SelectMenu(10);
                ResetQuantity();
                break;
            case 2:
                seasoning = Ingredient.Seasoning.Pepper;
                SelectMenu(10);
                ResetQuantity();
                break;
            case 3:
                seasoning = Ingredient.Seasoning.Chilli;
                SelectMenu(10);
                ResetQuantity();
                break;
        }
    }

    public void AddQuantity()
    {
        if (quantity < 9) quantity++;

    }

    public void SubtractQuantity()
    {
        if (quantity > 1) quantity--;
    }

    private void ResetQuantity()
    {
        quantity = 1;
    }

    private void SelectCooker()
    {
        //Debug.Log("Select cooker");
        if (cookerSelected)
        {
            cookerSelected = false;
            SelectMenu(11);
        }
        int i = AtLocation(cookerGroup);
        if (Input.GetButtonDown("Fire2"))
        {
            if (i >= 0)
            {
                selectedCooker = cookerGroup.appliance[i].standingLocation;
                cookerSelected = true;
            }
        }
    }

    private void SelectPrepTable()
    {
        if (prepTableSelected)
        {
            prepTableSelected = false;
            SelectMenu(11);
        }
        int i = AtLocation(prepTableGroup);
        if (Input.GetButtonDown("Fire2"))
        {
            if (i >= 0)
            {
                selectedPrepTable = prepTableGroup.appliance[i].standingLocation;
                prepTableSelected = true;
            }
        }
    }

    int AtLocation(ApplianceGroup ag)
    {
        Vector3 tileSelection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - new Vector3(0, 0, Camera.main.transform.position.z);
        tileSelection += new Vector3(-1, -1, 0);
        tileOverlay.transform.position = Vector3Int.CeilToInt(tileSelection) + new Vector3(0.5f, 0.5f, 0);
        int i = ag.AtLocation(Vector3Int.CeilToInt(tileSelection));
        if (i >= 0)
        {
            tileOverlay.GetComponent<SpriteRenderer>().sprite = Items.tile_overlay_green;
        }
        else
        {
            tileOverlay.GetComponent<SpriteRenderer>().sprite = Items.tile_overlay_red;
        }
        return i;
    }

    private void SelectLocation()
    {
        if (locationSelected)
        {
            locationSelected = false;
            ConfirmInstruction();
        }
        int i = AtLocation(worktopGroup);
        if (Input.GetButtonDown("Fire2"))
        {
            if (i >= 0)
            {
                selectedLocation = worktopGroup.appliance[i].standingLocation;
                locationSelected = true;
            }
        }
    }

    private void SelectIdle()
    {
        if (idleSelected)
        {
            idleSelected = false;
            SelectMenu(11);
        }
        Vector3 tileSelection = Camera.main.ScreenToWorldPoint(Input.mousePosition) - new Vector3(0, 0, Camera.main.transform.position.z);
        tileSelection += new Vector3(-1, -1, 0);
        tileOverlay.transform.position = Vector3Int.CeilToInt(tileSelection) + new Vector3(0.5f, 0.5f, 0);
        tileOverlay.GetComponent<SpriteRenderer>().sprite = Items.tile_overlay_green;
        if (Input.GetButtonDown("Fire2"))
        {
            selectedIdle = Vector3Int.CeilToInt(tileSelection) + new Vector3(0.5f, 0.5f, 0);
            idleSelected = true;
        }
    }

    public Vector3 FindClosestAppliance(ApplianceGroup a)
    {
        if (a.appliance != null && a.appliance.Length > 0)
        {
            int index = 0;
            float lowestDistance = (transform.position - a.appliance[index].standingLocation).magnitude;
            for (int i = 1; i < a.appliance.Length; i++)
            {
                if ((transform.position - a.appliance[i].standingLocation).magnitude < lowestDistance)
                {
                    lowestDistance = (transform.position - a.appliance[i].standingLocation).magnitude;
                    index = i;
                }
            }
            return a.appliance[index].standingLocation;
        }
        else
        {
            return new Vector3(0, 0, -1);
        }

    }

    public void EndInstruction()
    {
        if (instruction.Count != 0)
        {
            ResetMenu();
            RemoveInstruction(0);
            if (instruction.Count == 0) um.Move(selectedIdle);
        }
    }

    public void ConfirmInstruction()
    {
        if (currentTask == 1)
        {
            for (int i = 0; i < quantity; i++)
            {
                Instruction instr = new PrepInstruction(selectedIngredient, chop, seasoning, selectedPrepTable);
                instruction.Add(instr);
            }
            SelectMenu(15);
        }
        else if (currentTask == 2)
        {
            for (int i = 0; i < quantity; i++)
            {
                Instruction instr = new CookInstruction(selectedDish, selectedCooker);
                instruction.Add(instr);
            }
            SelectMenu(15);
        }
        else if (currentTask == 3)
        {
            Instruction instr = new ServeInstruction(selectedDish.dish, selectedLocation);
            instruction.Add(instr);
            SelectMenu(15);
        }
    }

    void StartInstruction()
    {
        if (instruction.Count != 0)
        {
            if (instruction[0] is PrepInstruction)
            {
                if (prep.StartPrep(instruction[0] as PrepInstruction))
                {
                    instructionInProgress = true;
                    instruction[0].StartInstruction();
                }
                else
                {
                    Abort();
                }
            }
            else if (instruction[0] is CookInstruction)
            {
                if (cook.StartCook(instruction[0] as CookInstruction))
                {
                    instructionInProgress = true;
                    instruction[0].StartInstruction();
                }
                else
                {
                    Abort();
                }
            }
            else if (instruction[0] is ServeInstruction)
            {
                if (serve.StartServe(instruction[0] as ServeInstruction))
                {
                    instructionInProgress = true;
                    instruction[0].StartInstruction();
                }
                else
                {
                    Abort();
                }
            }
        }
    }

    public void ResetMenu()
    {
        instructionInProgress = false;
        paused = false;
    }

    void StartNextInstruction()
    {
        if (instruction.Count != 0 && !instructionInProgress && !paused && !instruction[0].started)
        {
            //Debug.Log("Start Instruction");
            StartInstruction();
        }
    }

    public void Abort()
    {
        if (instruction.Count != 0)
        {
            if (instruction[0] is PrepInstruction)
            {
                prep.Reset();
            }
            else if (instruction[0] is CookInstruction)
            {
                cook.Reset();
            }
            else if (instruction[0] is ServeInstruction)
            {
                serve.Reset();
            }
            instruction.RemoveAt(0);
            ResetMenu();
        }
    }

    public void Pause()
    {
        ToggleInstructionInProgress();
        prep.pause = (instructionInProgress) ? false : true;
        cook.pause = (instructionInProgress) ? false : true;
        serve.pause = (instructionInProgress) ? false : true;
    }

    private void ToggleInstructionInProgress()
    {
        if (instruction.Count != 0 || (instructionInProgress && instruction.Count == 0))
        {
            instructionInProgress = (instructionInProgress) ? false : true;
        }
    }

    void UpdatePauseSprite()
    {
        play_icon.sprite = (instructionInProgress) ? Items.pause : Items.play;
        play_light.sprite = (instructionInProgress) ? Items.slider_fill : Items.slider_background;
    }

    public void MenuEnable(int index)
    {
        currentMenu = index;
        if (currentMenu == 0) currentTask = 0;
        for (int i = 0; i < menu.Length; i++)
        {
            if (i != index)
            {
                menu[i].SetActive(false);
            }
            else
            {
                menu[i].SetActive(true);
            }
        }
    }

    private void CheckMenuOptions()
    {
        selectCooker = (currentMenu == 2) ? true : false;
        selectPrepTable = (currentMenu == 9) ? true : false;
        selectLocation = (currentMenu == 3) ? true : false;
        selectIdle = (currentMenu == 12) ? true : false;
    }

    public void DishSelector(int dish)
    {
        if (currentTask == 2)
        {
            SelectDish(dish, 2);
        }
        else if (currentTask == 3)
        {
            SelectDish(dish, 3);
        }
    }

    public void UpdateCurrentTask(int task)
    {
        currentTask = task;
    }

    private void ArrowEnable()
    {
        if (currentMenu == 13 || currentMenu == 15)
        {
            arrow.SetActive(false);
        }
        else
        {
            arrow.SetActive(true);
        }
    }

    public void PreviousMenu()
    {
        switch (currentMenu)
        {
            case 0:
                SelectMenu(15);
                break;
            case 1:
                SelectMenu(0);
                break;
            case 2:
                SelectMenu(11);
                break;
            case 3:
                switch (currentTask)
                {
                    case 1:
                        SelectMenu(9);
                        break;
                    case 2:
                        SelectMenu(2);
                        break;
                    case 3:
                        SelectMenu(1);
                        break;
                }
                break;
            case 4:
                SelectMenu(3);
                break;
            case 5:
                SelectMenu(0);
                break;
            case 6:
                SelectMenu(5);
                break;
            case 7:
                SelectMenu(6);
                break;
            case 8:
                SelectMenu(15);
                break;
            case 9:
                SelectMenu(11);
                break;
            case 10:
                switch (currentTask)
                {
                    case 1:
                        SelectMenu(7);
                        break;
                    case 2:
                        SelectMenu(1);
                        break;
                }
                break;
            case 11:
                SelectMenu(15);
                break;
            case 12:
                SelectMenu(11);
                break;
            case 13:
                SelectMenu(15);
                break;
            case 14:
                SelectMenu(0);
                break;
        }
    }

    public void SwapInstruction(int index)
    {
        int i = Mathf.FloorToInt(index / 2);
        if (i < instruction.Count)
        {
            if (index % 2 == 0)
            {
                //up arrow
                if (i - 1 > 0 && i - 1 < instruction.Count)
                {
                    Instruction temp = instruction[i - 1];
                    instruction[i - 1] = instruction[i];
                    instruction[i] = temp;
                }
            }
            else
            {
                //down arrow
                if (i + 1 < instruction.Count)
                {
                    Instruction temp = instruction[i + 1];
                    instruction[i + 1] = instruction[i];
                    instruction[i] = temp;
                }
            }
        }
    }

    void UpdateInstructionText()
    {
        for (int i = 0; i < instructionDescription.Length; i++)
        {
            if (i < instruction.Count)
            {
                instructionDescription[i].text = ReadInstruction(instruction[i]);
            }
            else
            {
                instructionDescription[i].text = "Add Instruction (Click Add Icon)";
            }
        }
    }

    string ReadInstruction(Instruction instr)
    {
        string str = "";
        if (instr is PrepInstruction)
        {
            PrepInstruction pInstr = instr as PrepInstruction;
            str = pInstr.ingredient.name.ToString() + " : " + pInstr.chop.ToString() + " : " + pInstr.seasoning.ToString();
        }
        else if (instr is CookInstruction)
        {
            CookInstruction cInstr = instr as CookInstruction;
            str = "Cook " + cInstr.recipe.dish.name.ToString();
        }
        else if (instr is ServeInstruction)
        {
            ServeInstruction sInstr = instr as ServeInstruction;
            str = "Serving " + sInstr.dish.name;
        }
        else
        {
            str = "An instruction";
        }
        return str;
    }

    public void RemoveInstruction(int index)
    {
        //Debug.Log("Remove Instruction: " + index);
        if (index < instruction.Count && instruction[index] != null)
        {
            if (index == 0)
            {
                Abort();
            }
            else
            {
                instruction.RemoveAt(index);
            }
        }
    }

    public void AddInstruction()
    {
        SelectMenu(0);
    }

    public int instructionLength
    {
        get
        {
            return instruction.Count;
        }
    }

}
