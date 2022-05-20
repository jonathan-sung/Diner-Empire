using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{

    public bool selected; //detect unit selection
    public bool overAppliance;
    public GameObject appliance;
    public Canvas inventoryCanvas;
    public Text nameTag;
    public string unitName;
    private Inventory inv;
    public UnitMovement um;
    private AIController ai;

    protected SpriteRenderer spriteRenderer;

    public float baseCookingSkill;
    public float baseMovementSpeed;
    public float maxMovementSpeed;
    public float maxChopSpeed;
    public int expPerLevel;
    public float cookingSkill;
    public float movementSpeed;
    public float chopSpeed;
    public int level;
    public int exp;
    public int upgradePoints;

    public int cookingScore;
    public int movementScore;
    public int prepScore;

    public Text levelText;
    public Text expText;
    public Text pointsText;
    public Slider expSlider;

    public BeadDisplay beadDisplay;

    public bool cooking;
    public bool chopping;
    public bool seasoning;

    protected Animator animator;

    public static Vector3 activeOffset = new Vector3(0.5f, 1.35f, 0);

    public Canvas keyCanvas;


    // Use this for initialization
    void Start()
    {
        selected = false;
        overAppliance = false;
        inventoryCanvas.enabled = false;
        nameTag.text = unitName;
        inv = GetComponent<Inventory>();
        um = GetComponent<UnitMovement>();
        ai = GetComponent<AIController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        beadDisplay = GetComponent<BeadDisplay>();
        CalculateSkill();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateKeyCanvas();
        if (overAppliance && appliance != null)
        {
            if (selected && ai != null && !ai.instructionInProgress)
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    if (appliance.GetComponent<Appliance>().u == this)
                    {
                        appliance.GetComponent<Appliance>().ToggleActivate();
                    }
                }
            }
        }
        UpdateAnimatorVariables();
        if (selected)
        {
            inventoryCanvas.enabled = true;
            if (Input.GetKeyDown(KeyCode.B))
            {
                inv.ClearInventory();
            }
        }
        else
        {
            inventoryCanvas.enabled = false;
        }
        spriteRenderer.sortingOrder = -(Mathf.FloorToInt(transform.position.y));
    }

    void UpdateKeyCanvas()
    {
        if (selected && appliance)
        {
            if (!ai || !ai.instructionInProgress)
            {
                keyCanvas.enabled = true;
            }
        }
        else
        {
            keyCanvas.enabled = false;
        }
    }

    private void UpdateAnimatorVariables()
    {
        if (animator == null) return;
        animator.SetBool("Cooking", cooking);
        animator.SetBool("Chopping", chopping);
        animator.SetBool("Seasoning", seasoning);
    }

    public void GainExp()
    {
        exp++;
        LevelUp();
    }

    public void LevelUp()
    {
        if (exp >= expPerLevel)
        {
            level++;
            upgradePoints++;
            exp = expPerLevel - exp;
        }
    }

    public void UpgradeSkill(int skill)
    {
        //1 - cooking, 2 - movement, 3 - prep
        if (upgradePoints > 0)
        {
            if (skill == 1)
            {
                if (cookingScore < 10)
                {
                    cookingScore++;
                    upgradePoints--;
                }
            }
            else if (skill == 2)
            {
                if (movementScore < 10)
                {
                    movementScore++;
                    upgradePoints--;
                }
            }
            else if (skill == 3)
            {
                if (prepScore < 10)
                {
                    prepScore++;
                    upgradePoints--;
                }
            }
            CalculateSkill();
        }
    }

    public void CalculateSkill()
    {
        //cooking skill
        cookingSkill = baseCookingSkill + ((cookingScore / 10.0f) * baseCookingSkill);
        movementSpeed = baseMovementSpeed + ((movementScore / 10.0f) * (maxMovementSpeed - baseMovementSpeed));
        chopSpeed = maxChopSpeed - ((prepScore / 10.0f) * maxChopSpeed) + 0.5f;
        um.speed = movementSpeed;
    }

    public void UpdateStatsUI()
    {
        beadDisplay.UpdateBeads(cookingScore, movementScore, prepScore);
        levelText.text = "Level " + level.ToString();
        expText.text = "Exp: " + exp.ToString() + "/" + expPerLevel.ToString();
        pointsText.text = upgradePoints.ToString();
        expSlider.value = (float)exp / expPerLevel;
    }
}
