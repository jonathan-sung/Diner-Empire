using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeBook : MonoBehaviour
{

    public Text dishName;
    public Text[] ingredientText;
    public Text pageText;
    public Text coverTitle;
    public Image dishImage;

    public GameObject dishMenu;
    public GameObject cover;

    public int currentPage;
    public RestaurantManager rm;

    public Canvas canvas;
    public CanvasScaler canvasScaler;

    public bool display;

    void Start()
    {
        rm = GameObject.Find("Restaurant Manager").GetComponent<RestaurantManager>();
        coverTitle.text = rm.GetCuisineName() + " Recipe Book";
        canvas.enabled = true;
        canvasScaler.enabled = true;
        DisplayCover();
    }

    void Update()
    {
        if (display)
        {
            canvas.enabled = true;
            canvasScaler.enabled = true;
            UpdatePage(0);
        }
        else
        {
            canvas.enabled = false;
            canvasScaler.enabled = false;
        }
    }

    public void TurnPage(int pageTurns)
    {
        currentPage += pageTurns;
        UpdatePage(pageTurns);
    }

    void UpdatePage(int pageTurns)
    {
        if (currentPage < 1 || currentPage > (rm.recipeList.Length + 1)) currentPage -= pageTurns;
        pageText.text = "Page " + currentPage + "/" + (rm.recipeList.Length + 1).ToString();
        if (currentPage > 1)
        {
            ReadRecipe(rm.recipeList[currentPage - 2]);
            DisplayMenu();
        }
        else if (currentPage == 1)
        {
            DisplayCover();
        }
    }

    public void ToggleDisplay()
    {
        display = (display) ? false : true;
    }

    void DisplayMenu()
    {
        dishMenu.SetActive(true);
        cover.SetActive(false);
    }

    void DisplayCover()
    {
        dishMenu.SetActive(false);
        cover.SetActive(true);
    }

    public void ReadRecipe(Recipe recipe)
    {
        dishName.text = recipe.dish.name;
        dishImage.sprite = recipe.dish.sprite;
        for (int i = 0; i < ingredientText.Length; i++)
        {
            ReadIngredient(recipe.ingredients[i], i);
        }
    }

    public void ReadIngredient(Ingredient ingredient, int textIndex)
    {
        string text = "";
        if (ingredient.chop != Ingredient.Chop.None) text = text + ingredient.chop.ToString();
        text = text + "<color=#008000ff> " + ingredient.name.ToString() + " </color>";
        if (ingredient.seasoning != Ingredient.Seasoning.None) text = text + "seasoned with " + ingredient.seasoning.ToString();
        ingredientText[textIndex].text = text;
    }
}
