using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrepTable : Appliance
{

    public Ingredient slot;
    public Image ingredientSprite;
    public SpriteRenderer spriteRenderer;

    public float chopTime;

    public Slider slider;
    public Slider worldSlider;
    private float currentChopTime;
    public bool chopping;
    public bool seasoning;
    public float currentSeasonTime;
    public float seasonTime = 2;

    public int choosenSeasoning;

    public Canvas worldCanvas;
    public CanvasScaler worldCanvasScaler;

    public Button[] button = new Button[5];

    public ParticleSystem particle_system;

    Material salt_material;
    Material pepper_material;
    Material chilli_material;
    Material food_material;

    // Use this for initialization
    new void Start()
    {
        salt_material = Resources.Load<Material>("Materials/Salt");
        pepper_material = Resources.Load<Material>("Materials/Pepper");
        chilli_material = Resources.Load<Material>("Materials/Chilli");
        food_material = Resources.Load<Material>("Materials/Food");
        transform.localScale = new Vector3(0.5f, 0.5f, 1);
        worldCanvas.gameObject.transform.localScale = new Vector3(2, 2, 1);
        ResetChop();
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        CheckCollision();
        CheckAI();
        if (activated)
        {
            UpdateSprites();
            CheckCanvas();
            UpdateUnit();
            CheckChopping();
            CheckSeasoning();
            UpdateParticles();
            UnitPosition(chopping, seasoning);
        }
    }



    private void UpdateUnit()
    {
        if (u == null) return;

        u.chopping = chopping;
        u.seasoning = seasoning;
    }

    void UpdateParticles()
    {
        if (chopping && u)
        {
            particle_system.GetComponent<Renderer>().material = food_material;
            if (!particle_system.isPlaying) particle_system.Play();
        }
        else if (seasoning && u)
        {
            particle_system.GetComponent<Renderer>().material = SelectSeasoningParticle();
            if (!particle_system.isPlaying) particle_system.Play();
        }
        else
        {
            if (particle_system.isPlaying) particle_system.Stop();
        }
    }

    Material SelectSeasoningParticle()
    {
        if (choosenSeasoning == 1)
        {
            return salt_material;
        }
        else if (choosenSeasoning == 2)
        {
            return pepper_material;
        }
        else
        {
            return chilli_material;
        }
    }

    void CheckChopping()
    {
        if (chopping) Chop();
    }

    void CheckSeasoning()
    {
        if (seasoning && u != null && u.GetComponent<AIController>() != null && !u.GetComponent<AIController>().instructionInProgress) Season(choosenSeasoning);
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


    public void PopIngredient()
    {
        if (slot != null)
        {
            if (u.GetComponent<Inventory>().AddItem(slot))
            {
                slot = null;
                ResetChop();
            }
            else if (u.GetComponent<Inventory>().selectedSlot != -1) //full inventory
            {
                u.GetComponent<Inventory>().Swap(ref slot);
                ResetChop();
            }
        }
    }

    public bool AIPopIngredient(Ingredient ingredient)
    {
        if (slot != null)
        {
            if (slot is Ingredient && SameIngredient(slot, ingredient) && u.GetComponent<Inventory>().AddItem(slot))
            {
                slot = null;
                ResetChop();
                return true;
            }
        }
        return false;
    }

    public bool AIPopIngredient()
    {
        if (slot != null)
        {
            if (u.GetComponent<Inventory>().AddItem(slot))
            {
                slot = null;
                ResetChop();
                return true;
            }
        }
        return false;
    }

    public bool AddItem(Ingredient ingredient) //return value determines whether the transaction was successful
    {
        if (slot == null)
        {
            slot = ingredient;
            return true;
        }
        else
        {
            return false;
        }
    }

    public void UpdateSprites()
    {
        if (slot != null)
        {
            ingredientSprite.sprite = slot.sprite;
            ingredientSprite.color = new Color(1, 1, 1, 1);
            spriteRenderer.sprite = slot.sprite;
            spriteRenderer.color = new Color(1, 1, 1, 1);
        }
        else
        {
            ingredientSprite.sprite = null;
            ingredientSprite.color = new Color(1, 1, 1, 0);
            spriteRenderer.sprite = null;
            spriteRenderer.color = new Color(1, 1, 1, 0);
        }
    }

    public void Season(int i)
    {
        if (slot == null) ResetSeason();
        ResetChop();
        seasoning = true;
        worldCanvas.enabled = true;
        worldCanvasScaler.enabled = true;
        currentSeasonTime += Time.deltaTime;
        SetSliderValue(currentSeasonTime, seasonTime);
        if (currentSeasonTime >= seasonTime)
        {
            if (slot != null)
            {
                if (i == 1)
                {
                    slot.Season(Ingredient.Seasoning.Salt);
                }
                else if (i == 2)
                {
                    slot.Season(Ingredient.Seasoning.Pepper);
                }
                else if (i == 3)
                {
                    slot.Season(Ingredient.Seasoning.Chilli);
                }
                if (u != null) u.GainExp();
            }
            ResetSeason();
        }
    }

    public void StartSeason(int s)
    {
        if (!seasoning && slot != null)
        {
            seasoning = true;
            currentSeasonTime = 0;
            choosenSeasoning = s;
        }
        else
        {
            ResetSeason();
        }
    }

    private void ResetSeason()
    {
        seasoning = false;
        currentSeasonTime = 0;
        slider.value = 0;
        worldSlider.value = 0;
        worldCanvas.enabled = false;
        worldCanvasScaler.enabled = false;
    }

    public void StartChop()
    {
        if (!chopping && slot != null)
        {
            chopping = true;
            currentChopTime = 0;
            chopTime = u.chopSpeed;
        }
        else
        {
            ResetChop();
            ResetSeason();
        }
    }

    public void Chop()
    {
        if (slot == null) ResetChop();
        chopping = true;
        worldCanvas.enabled = true;
        worldCanvasScaler.enabled = true;
        chopTime = u.chopSpeed;
        currentChopTime += Time.deltaTime;
        SetSliderValue(currentChopTime, chopTime);
        if (currentChopTime >= chopTime)
        {
            if (slot != null)
            {
                slot.ChopIngredient();
                if (u != null) u.GainExp();
            }
            ResetChop();
        }
    }

    private void SetSliderValue(float currentTime, float totalTime)
    {
        slider.value = currentTime / totalTime;
        worldSlider.value = currentTime / totalTime;
    }

    private void ResetChop()
    {
        chopping = false;
        currentChopTime = 0;
        slider.value = 0;
        worldSlider.value = 0;
        worldCanvas.enabled = false;
        worldCanvasScaler.enabled = false;
    }


}
