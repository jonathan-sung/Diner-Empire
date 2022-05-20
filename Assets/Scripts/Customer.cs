using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Customer : Unit
{
    public Dish dish;
    public float waitTime;
    public float currentTime;
    Vector3 seatLocation;//transform coord
    Vector3 exitLocation;
    Reception reception;
    public int seat;
    public int stage;
    public int orderNumber;
    float rating;
    //public SpriteRenderer background;

    float middleRatingBand = 0.25f;
    float lowerRatingBand = 0.125f;

    public Image backPanel;
    public Image background;
    public Image dishImage;
    bool checkedSeat;

    public RestaurantManager rm;

    public InfoDisplay infoDisplay;
    public bool showInfo;

    public int sitting;
    public int eating;

    float timeRating;
    float eatingTime = 10;

    float customerWaitTime = 6;

    public ParticleSystem particle_system;

    float tippingPercentage = 0.15f;

    void Start()
    {
        reception = GameObject.Find("Reception").GetComponent<Reception>();
        exitLocation = GameObject.Find("Exit").transform.position;
        infoDisplay = GameObject.Find("Player").GetComponent<InfoDisplay>();
        um = GetComponent<UnitMovement>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        selected = false;
        overAppliance = false;
        transform.position = exitLocation;
        checkedSeat = false;
        backPanel.enabled = false;
        background.enabled = false;
        dishImage.enabled = false;
        stage = 0;
    }

    void Update()
    {
        CheckBackgroundColour();
        UpdateInfoDisplay();
        if (stage == 0 && !checkedSeat)
        {
            seat = reception.RequestSeat(this);
            SetSeat(seat);
            checkedSeat = true;
        }
        else if (stage == 1)
        {
            //go to seat
            if ((transform.position + new Vector3(-0.5f, -0.5f, 0)) == seatLocation)
            {
                stage = 2;
            }
            else if (!um.move)
            {
                um.Move(seatLocation);
            }
        }
        if (stage == 2)
        {
            //look for dish
            SetSitting();
            if (reception.tableGroup.item[seat] != null)
            {
                if (reception.tableGroup.item[seat] is Dish && SameDish((Dish)reception.tableGroup.item[seat]))
                {
                    timeRating = (waitTime - currentTime) / waitTime;
                    stage = 3;
                    eating = sitting;
                    sitting = 0;
                    currentTime = 0;
                    return;
                }
                else
                {
                    GiveRating(0, this);
                    reception.LeaveSeat(this);
                    stage = 4;
                }
            }
            currentTime += Time.deltaTime;
            background.fillAmount = (waitTime - currentTime) / waitTime;
            if (currentTime >= waitTime)
            {
                GiveRating(0, this);
                reception.LeaveSeat(this);
                stage = 4;
            }
        }
        if (stage == 3)
        {
            //eat and give rating
            DisableBubble();
            if (reception.tableGroup.item[seat] == null)
            {
                GiveRating(0, this);
                reception.LeaveSeat(this);
                stage = 4;
            }
            currentTime += Time.deltaTime;
            if (currentTime >= eatingTime)
            {
                if (reception.tableGroup.item[seat] != null)
                {
                    if (timeRating >= middleRatingBand)
                    {
                        GiveRating(((Dish)reception.tableGroup.item[seat]).quality, this);
                    }
                    else if (timeRating >= lowerRatingBand)
                    {
                        GiveRating(((Dish)reception.tableGroup.item[seat]).quality * 0.7f, this);
                    }
                    else
                    {
                        GiveRating(((Dish)reception.tableGroup.item[seat]).quality * 0.5f, this);
                    }
                }
                else
                {
                    GiveRating(0, this);
                }
                reception.LeaveSeat(this);
                stage = 4;
            }
            if (!particle_system.isPlaying) particle_system.Play();
        }
        if (stage == 4)
        {
            //go to exit
            if (particle_system.isPlaying) particle_system.Stop();
            sitting = 0;
            eating = 0;
            if ((transform.position + new Vector3(-0.5f, -0.5f, 0)) == exitLocation)
            {
                Destroy(gameObject);
            }
            else if (!um.move)
            {
                DisableBubble();
                um.Move(exitLocation);
            }
        }
        UpdateSittingLocation();
        UpdateAnimatorVariables();
        UpdateLayer();
    }

    void DisableBubble()
    {
        backPanel.enabled = false;
        background.enabled = false;
        dishImage.enabled = false;
    }

    void UpdateSittingLocation()
    {
        if (sitting == 1 || eating == 1)
        {
            transform.position = seatLocation + new Vector3(0.5f, 0.8f, 0);
            backPanel.gameObject.transform.localPosition = new Vector3(0, 1.2f, 0);
            background.gameObject.transform.localPosition = new Vector3(0, 1.2f, 0);
            dishImage.gameObject.transform.localPosition = new Vector3(0, 1.2f, 0);
        }
    }

    void UpdateLayer()
    {
        if (sitting == 2 || eating == 2)
        {
            particle_system.GetComponent<Renderer>().sortingLayerName = "Items 2";
            spriteRenderer.sortingLayerName = "Items 2";
            spriteRenderer.sortingOrder = 1;
        }
        else
        {
            spriteRenderer.sortingLayerName = "Player";
            spriteRenderer.sortingOrder = -(Mathf.FloorToInt(transform.position.y));
        }
    }

    void GiveRating(float rating, Customer customer)
    {
        float money = dish.price + (dish.price * tippingPercentage * rating);
        this.rating = rating;
        if (rm != null)
        {
            rm.AddRating(rating, money, customer);
            DisplayPopUp();
        }
    }

    void DisplayPopUp()
    {
        GameObject g = (GameObject)Instantiate(Resources.Load("Prefabs/Pop Up"));
        g.transform.position = gameObject.transform.position + new Vector3(0, 2, 0);
        g.GetComponent<PopUp>().rating = rating;
        g.GetComponent<PopUp>().StartPopUp();
    }

    public void SetSeat(int seat)
    {
        if (seat >= 0)
        {
            if (!rm) rm = GameObject.Find("Restaurant Manager").GetComponent<RestaurantManager>();
            seatLocation = reception.tableGroup.appliance[seat].standingLocation;
            if (dish == null) dish = rm.SelectRandomDish();
            waitTime = Random.Range(customerWaitTime, customerWaitTime) * 60;
            dishImage.sprite = dish.sprite;
            backPanel.enabled = true;
            background.enabled = true;
            dishImage.enabled = true;
            rm.AddCustomer(this);
            orderNumber = rm.orders;
            stage = 1;
        }
    }

    void SetSitting()
    {
        if (((Table)reception.tableGroup.appliance[seat]).foregroundType)
        {
            sitting = 2;
        }
        else
        {
            sitting = 1;
        }
    }

    void UpdateAnimatorVariables()
    {
        if (animator == null) return;
        animator.SetInteger("Sitting", sitting);
        animator.SetInteger("Eating", eating);
    }

    private void CheckBackgroundColour()
    {
        float waitPercentage = (waitTime - currentTime) / waitTime;
        if (waitPercentage >= middleRatingBand)
        {
            background.color = new Color32(55, 148, 110, 215);
        }
        else if (waitPercentage >= lowerRatingBand)
        {
            background.color = new Color32(227, 183, 50, 215);
        }
        else
        {
            background.color = new Color32(227, 61, 45, 215);
        }
    }

    private bool SameDish(Dish d)
    {
        if (d.name == dish.name)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void UpdateInfoDisplay()
    {
        if (showInfo)
        {
            infoDisplay.ReadCustomer(this);
        }
    }

    public int priorityInQueue
    {
        get
        {
            return rm.customers.IndexOf(this) + 1;
        }
    }
}
