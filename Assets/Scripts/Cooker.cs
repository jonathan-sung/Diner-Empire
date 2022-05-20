using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cooker : Appliance
{
    public Ingredient[] ingredients = new Ingredient[3];
    public Image[] image = new Image[3];
    public Image[] slot = new Image[3];
    public Dish output;
    public Image outputImage;
    public Image outputSlot;

    public float cookTime;

    public Slider slider;
    public Slider worldSlider;
    private float currentCookTime;
    public bool cooking;

    public Canvas worldCanvas;
    public CanvasScaler worldCanvasScaler;

    public Button[] button = new Button[5];

    public int selectedSlot = -1; // -1 for no slot, otherwise index

    public ParticleSystem particle_system;
    // Use this for initialization
    new void Start()
    {
        ResetCook();
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        CheckCollision();
        CheckAI();
        if (activated)
        {
            CheckSwap();
            UpdateSprites();
            CheckCanvas();
            UpdateUnit();
            CheckCook();
            UnitPosition(cooking, false);
        }
        //Debug.Log(gameObject.name + ": " + (u != null));

    }

    private void UpdateUnit()
    {
        if (u == null) return;
        u.cooking = cooking;
    }

    void CheckCook()
    {
        if (cooking && u)
        {
            Cook();
            if (!particle_system.isEmitting) particle_system.Play();
        }
        else
        {
            if (particle_system.isEmitting) particle_system.Stop();
        }

    }

    private void CheckAI()
    {
        if (u != null && u.GetComponent<AIController>() != null && u.GetComponent<AIController>().instructionInProgress)
        {
            for (int i = 0; i < button.Length; i++)
            {
                button[i].interactable = false;
            }
        }
        else
        {
            for (int i = 0; i < button.Length; i++)
            {
                button[i].interactable = true;
            }
        }
    }

    private void CheckSwap()
    {
        for (int i = 0; i <= ingredients.Length; i++)
        {
            if ((i == selectedSlot) && (i != ingredients.Length))
            {
                if (u.GetComponent<Inventory>().AddItem(ingredients[i]))
                {
                    //Debug.Log("Check swap ingredient");
                    RemoveItem(ref ingredients[i]);
                    ResetCook();
                    selectedSlot = -1;
                }
                else if (u.GetComponent<Inventory>().selectedSlot != -1) //full inventory
                {
                    //Debug.Log("Full swap ingredient");
                    u.GetComponent<Inventory>().Swap(ref ingredients[i]);
                    ResetCook();
                    selectedSlot = -1;
                }
            }
            else if ((i == selectedSlot) && (i == ingredients.Length))
            {
                if (u.GetComponent<Inventory>().AddItem(output))
                {
                    //Debug.Log("Added dish");
                    output = null;
                    selectedSlot = -1;
                }
            }
        }
    }

    public void ToggleSwap(int i)
    {
        if (i == selectedSlot)
        {
            selectedSlot = -1;
        }
        else
        {
            selectedSlot = i;
        }
    }

    public bool AddItem(Ingredient ingredient) //return value determines whether the transaction was successful
    {
        for (int i = 0; i < ingredients.Length; i++)
        {
            if (ingredients[i] == null)
            {
                ingredients[i] = ingredient;
                return true;
            }
        }
        return false;
    }

    public void UpdateSprites()
    {
        for (int i = 0; i < image.Length; i++)
        {
            if (ingredients[i] != null)
            {
                image[i].color = new Color(1, 1, 1, 1);
                image[i].sprite = ingredients[i].sprite;
            }
            else
            {
                image[i].color = new Color(1, 1, 1, 0);
                image[i].sprite = null;
            }
            if (i == selectedSlot)
            {
                slot[i].sprite = Items.panel_3;
            }
            else
            {
                slot[i].sprite = Items.panel_4;
            }
        }
        if (output != null)
        {
            outputImage.color = new Color(1, 1, 1, 1);
            outputImage.sprite = output.sprite;
        }
        else
        {
            outputImage.color = new Color(1, 1, 1, 0);
            outputImage.sprite = null;
        }
        if (ingredients.Length == selectedSlot)
        {
            outputSlot.sprite = Items.panel_3;
        }
        else
        {
            outputSlot.sprite = Items.panel_4;
        }
    }

    public void StartCook()
    {
        if (!cooking)
        {
            cooking = true;
            currentCookTime = 0;
            cookTime = GetCookTime();
        }
        else
        {
            ResetCook();
        }
    }

    public void Cook()
    {
        worldCanvas.enabled = true;
        worldCanvasScaler.enabled = true;
        currentCookTime += Time.deltaTime;
        SetSliderValue();
        if (currentCookTime >= cookTime)
        {
            CheckRecipe();
            ResetCook();
        }
    }

    private void SetSliderValue()
    {
        slider.value = currentCookTime / cookTime;
        worldSlider.value = currentCookTime / cookTime;
    }

    public void ResetCook()
    {
        cooking = false;
        currentCookTime = 0;
        slider.value = 0;
        worldSlider.value = 0;
        worldCanvas.enabled = false;
        worldCanvasScaler.enabled = false;
    }

    public void CheckRecipe()
    {
        for (int i = 0; i < Recipes.recipes.Length; i++)
        {
            if (Recipes.recipes[i] == null) continue;
            if (CheckForDish(Recipes.recipes[i]))
            {
                output = new Dish(Recipes.recipes[i].dish, u.cookingSkill);
                ClearIngredients();
                if (u != null && output != null) u.GainExp();
                break;
            }
        }
        if (output == null)
        {
            //Debug.Log("Cook failed!");
        }
    }

    public float GetCookTime()
    {
        for (int i = 0; i < Recipes.recipes.Length; i++)
        {
            if (Recipes.recipes[i] == null) continue;
            if (CheckForDish(Recipes.recipes[i]))
            {
                return Recipes.recipes[i].dish.cookTime;
            }
        }
        return 0;
    }

    private void ClearIngredients()
    {
        for (int i = 0; i < ingredients.Length; i++)
        {
            ingredients[i] = null;
        }
    }

    private bool CheckForDish(Recipe r)
    {
        bool[] ingredientPresent = new bool[ingredients.Length];
        for (int j = 0; j < ingredients.Length; j++)
        {
            for (int i = 0; i < ingredients.Length; i++)
            {
                if (SameIngredient(ingredients[i], r.ingredients[j]))
                {
                    ingredientPresent[j] = true;
                }
            }
        }
        for (int i = 0; i < ingredients.Length; i++)
        {
            if (!ingredientPresent[i])
            {
                return false;
            }
        }
        return true;
    }
}
