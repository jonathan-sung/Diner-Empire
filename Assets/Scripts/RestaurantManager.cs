using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RestaurantManager : MonoBehaviour
{
    public string restaurantName;
    float restaurantBalance;
    float ageOfBusiness;
    int totalSales;
    public float rushTime;
    public float openTime; //opening time of the restaurant, minutes * seconds = 60 * 60 = 3600, so 3600 seconds = 1 hour
    public float closeTime; //time that the restaurant closes
    public int cuisine; //1 - american, 2 - british, 3 - italian, 4 - chinese, 5 - mexican
    public int customerSpawnGrouping;
    public float minSpawnRate;
    public float maxSpawnRate; //one customer generated per *maxSpawnRate* seconds
    public float rushHourSpawnFactor; //how many times more customers than usual; 2 means double customers

    List<float> ratings = new List<float>();
    int ratingCap = 100; //counts 100 most recent ratings
    public float restaurantRating; //average of 100 most recent customer ratings
    public float startDayRating;

    public float time; //3600 is one hour
    public float timeScaleFactor = 60;

    public int activeCustomers;
    int idleWorkers;

    int todayCustomersServed;
    float todayIncome;
    float todayExpenditure;

    public Text balanceText;
    public Text activeCustomersText;
    public Text idleText;

    public Item[] item = new Item[4];

    Clock clock;
    CustomerGenerator customerGenerator;
    FiveStarDisplay starDisplay;

    public bool rushHour;

    public List<AIController> ai = new List<AIController>();
    public List<Customer> customers = new List<Customer>();

    public int orders;

    public Recipe[] recipeList;
    public Ingredient[] ingredientList;

    bool open; //if the restaurant is open or closed

    public GameObject summary;

    public FiveStarDisplay summaryStarDisplay;
    public Text restaurantNameText;
    public Text incomeText;
    public Text expenditureText;
    public Text profitText;
    public Text ratingChangeText;
    public Text numberOfSalesText;
    public Text treasuryText;
    public Text totalSalesText;
    public Text ageText;
    public Text employeesText;
    public Text employeeSkillText;

    void Start()
    {
        SelectCuisine();
        GameObject g = (GameObject)Instantiate(Resources.Load("Prefabs/Clock"));
        clock = g.GetComponent<Clock>();
        customerGenerator = GetComponent<CustomerGenerator>();
        starDisplay = GetComponent<FiveStarDisplay>();
        time = openTime;
        CalculateRestaurantRating();
        startDayRating = restaurantRating;
        InitUI();
        ResetToday();
        customerGenerator.UpdateFrequency();
        customerGenerator.SpawnCustomer(customerSpawnGrouping);
        summary.SetActive(false);
    }

    void Update()
    {
        if (open)
        {
            UpdateUI();
            UpdateActiveCustomers();
            CheckRushHour();
            time += Time.deltaTime * timeScaleFactor;
            clock.UpdateClock(time);
            clock.UpdateRush(time / timeScaleFactor, rushHour);
            starDisplay.UpdateDisplay(restaurantRating);
            item[3].name = "Restaurant Rating: " + restaurantRating.ToString("P1");
            CheckEndOfDay();
        }
    }

    void ResetToday()
    {
        todayCustomersServed = 0;
        todayIncome = 0;
        todayExpenditure = 0;
        open = true;
    }

    void UpdateIdle()
    {
        idleWorkers = 0;
        for (int i = 0; i < ai.Count; i++)
        {
            if (!ai[i].instructionInProgress && !ai[i].um.move && !ai[i].unit.chopping && !ai[i].unit.seasoning && !ai[i].unit.cooking)
            {
                idleWorkers++;
            }
        }
    }

    public void SelectCuisine()
    {
        if (cuisine == 1)
        {
            recipeList = Recipes.americanRecipes;
            ingredientList = Recipes.americanIngredientList;
        }
        else if (cuisine == 2)
        {
            recipeList = Recipes.britishRecipes;
            ingredientList = Recipes.britishIngredientList;
        }
        else if (cuisine == 3)
        {
            recipeList = Recipes.chineseRecipes;
            ingredientList = Recipes.chineseIngredientList;
        }
        else if (cuisine == 4)
        {
            recipeList = Recipes.italianRecipes;
            ingredientList = Recipes.italianIngredientList;
        }
        else if (cuisine == 5)
        {
            recipeList = Recipes.mexicanRecipes;
            ingredientList = Recipes.mexicanIngredientList;
        }
        else
        {
            recipeList = Recipes.americanRecipes;
            ingredientList = Recipes.americanIngredientList;
        }
    }

    public string GetCuisineName()
    {
        if (cuisine == 1)
        {
            return "American";
        }
        else if (cuisine == 2)
        {
            return "British";
        }
        else if (cuisine == 3)
        {
            return "Chinese";
        }
        else if (cuisine == 4)
        {
            return "Italian";
        }
        else if (cuisine == 5)
        {
            return "Mexican";
        }
        else
        {
            return "No Cuisine";
        }
    }

    void InitUI()
    {
        item[0] = new Item("Restaurant Balance", null);
        item[1] = new Item("Number of active customers", null);
        item[2] = new Item("Number of idle workers", null);
        item[3] = new Item("Restaurant Rating: " + restaurantRating.ToString("P1"), null);
    }

    void CheckRushHour()
    {
        if (CheckTime(rushTime))
        {
            StartRushHour();
        }
        else if (CheckTime(rushTime + 3600))
        {
            EndRushHour();
        }
    }

    bool CheckTime(float t)
    {
        if (time >= t && time <= t + timeScaleFactor)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void EndRushHour()
    {
        if (rushHour)
        {
            Instantiate(Resources.Load("Prefabs/EndRushMessage"));
            rushHour = false;
        }
    }

    void StartRushHour()
    {
        if (!rushHour)
        {
            Instantiate(Resources.Load("Prefabs/RushMessage"));
            rushHour = true;
        }
    }

    public void AddRating(float r, float m, Customer customer)
    {
        ratings.Add(r);
        CalculateRestaurantRating();
        customerGenerator.UpdateFrequency();
        AddMoney(m);
        todayCustomersServed++;
        RemoveCustomer(customer);
    }

    public void RemoveCustomer(Customer customer)
    {
        customers.Remove(customer);
        UpdateActiveCustomers();
        if (activeCustomers == 0) customerGenerator.SpawnCustomer();
    }

    public void AddCustomer(Customer customer)
    {
        customers.Add(customer);
        UpdateActiveCustomers();
        orders++;
    }

    void UpdateActiveCustomers()
    {
        activeCustomers = customers.Count;
    }

    public void AddMoney(float m)
    {
        restaurantBalance += m;
        if (m > 0)
        {
            todayIncome += m;
        }
        else
        {
            todayExpenditure -= m;
        }

    }

    void UpdateUI()
    {
        balanceText.text = "$" + restaurantBalance.ToString("n2");
        activeCustomersText.text = activeCustomers.ToString();
        idleText.text = idleWorkers.ToString();
        UpdateIdle();
    }

    void CalculateRestaurantRating()
    {
        if (ratings.Count == 0)
        {
            restaurantRating = 0;
            return;
        }
        int ratingLength = ratings.Count;
        if (ratingLength < ratingCap)
        {
            float totalRating = 0;
            foreach (float r in ratings)
            {
                totalRating += r;
            }
            restaurantRating = totalRating / ratings.Count;
        }
        else
        {
            float totalRating = 0;
            for (int i = ratings.Count - 1; i > (ratings.Count - ratingCap); i--)
            {
                totalRating += ratings[i];
            }
            restaurantRating = totalRating / ratingCap;
        }
    }

    public Dish SelectRandomDish()
    {
        bool found = false;
        do
        {
            Recipe r = recipeList[Mathf.RoundToInt(Random.Range(0, recipeList.Length))];
            if (r != null)
            {
                found = true;
                return r.dish;
            }
        } while (!found);
        return null;
    }

    void CheckEndOfDay()
    {
        if (CheckTime(closeTime))
        {
            open = false;
            customerGenerator.activated = false;
            UpdateSummary();
            summaryStarDisplay.UpdateDisplay(restaurantRating);
            summary.SetActive(true);
        }
    }

    void UpdateSummary()
    {
        restaurantNameText.text = restaurantName;
        incomeText.text = "Income: " + ChangeText(todayIncome, "C");
        expenditureText.text = "Expenditure: " + ChangeText(-todayExpenditure, "C");
        profitText.text = "Profit: " + ChangeText(todayIncome - todayExpenditure, "C");
        ratingChangeText.text = "Rating Change: " + ChangeText((restaurantRating - startDayRating), "P");
        numberOfSalesText.text = "Number of Sales: " + todayCustomersServed.ToString();
        treasuryText.text = "Treasury: " + ChangeText(restaurantBalance, "C");
        totalSalesText.text = "Total Sales: " + totalSales.ToString();
        ageText.text = "Age: " + ageOfBusiness.ToString() + " days";
        employeesText.text = "Employees: " + ai.Count.ToString();
        employeeSkillText.text = "Employee Skill: " + CalculateAverageUnitSkill().ToString("P");
    }

    float CalculateAverageUnitSkill()
    {
        float totalSkill = 0;
        for (int i = 0; i < ai.Count; i++)
        {
            totalSkill += ai[i].unit.cookingScore;
            totalSkill += ai[i].unit.movementScore;
            totalSkill += ai[i].unit.prepScore;
        }
        return totalSkill / (10f * ai.Count);
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    string ChangeText(float a, string type)
    {
        string text = a.ToString();
        if (a >= 0)
        {
            text = "<color=#64f20cff>" + a.ToString(type) + "</color>";
        }
        else
        {
            text = "<color=#d12727ff>" + a.ToString(type) + "</color>";
        }
        return text;
    }

}
