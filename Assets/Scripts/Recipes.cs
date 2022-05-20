using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Recipes
{

    public static Recipe[] recipes;
    public static Recipe[] americanRecipes;
    public static Recipe[] britishRecipes;
    public static Recipe[] chineseRecipes;
    public static Recipe[] italianRecipes;
    public static Recipe[] mexicanRecipes;

    public static Ingredient[] americanIngredientList = { Items.tomato, Items.parsley, Items.onion, Items.potato, Items.garlic, Items.chicken, Items.beef, Items.pork, Items.cheese, Items.bread };
    public static Ingredient[] britishIngredientList = { Items.tomato, Items.lettuce, Items.carrot, Items.cucumber, Items.potato, Items.coriander, Items.peas, Items.egg, Items.fish, Items.chicken, Items.beef, Items.pork };
    public static Ingredient[] chineseIngredientList = { Items.noodles, Items.rice, Items.ginger, Items.garlic, Items.spring_onion, Items.onion, Items.dough, Items.fish, Items.tofu, Items.chicken, Items.beef, Items.pork };
    public static Ingredient[] italianIngredientList = { Items.pasta, Items.dough, Items.bread, Items.tomato, Items.mushroom, Items.cucumber, Items.beef, Items.pork, Items.cheese, Items.rice, };
    public static Ingredient[] mexicanIngredientList = { Items.taco, Items.dough, Items.cheese, Items.coriander, Items.tomato, Items.avocado, Items.chicken, Items.beef, Items.pork, Items.onion };

    static Recipes()
    {
        recipes = new Recipe[35];
        recipes[0] = new Recipe(new Ingredient(Items.chicken, Ingredient.Seasoning.Salt, Ingredient.Chop.Chopped), new Ingredient(Items.parsley, Ingredient.Seasoning.None, Ingredient.Chop.Chopped), new Ingredient(Items.tomato, Ingredient.Seasoning.None, Ingredient.Chop.Diced), Items.chicken_soup);
        recipes[1] = new Recipe(new Ingredient(Items.chicken, Ingredient.Seasoning.Chilli, Ingredient.Chop.None), new Ingredient(Items.parsley, Ingredient.Seasoning.None, Ingredient.Chop.Chopped), new Ingredient(Items.onion, Ingredient.Seasoning.Pepper, Ingredient.Chop.Chopped), Items.fried_chicken);
        recipes[2] = new Recipe(new Ingredient(Items.beef, Ingredient.Seasoning.Pepper, Ingredient.Chop.None), new Ingredient(Items.cheese, Ingredient.Seasoning.None, Ingredient.Chop.Minced), new Ingredient(Items.potato, Ingredient.Seasoning.Salt, Ingredient.Chop.Chopped), Items.steak_and_chips);
        recipes[3] = new Recipe(new Ingredient(Items.pork, Ingredient.Seasoning.Chilli, Ingredient.Chop.Chopped), new Ingredient(Items.tomato, Ingredient.Seasoning.Pepper, Ingredient.Chop.Minced), new Ingredient(Items.garlic, Ingredient.Seasoning.Salt, Ingredient.Chop.Minced), Items.bbq_spare_ribs);
        recipes[4] = new Recipe(new Ingredient(Items.pork, Ingredient.Seasoning.Pepper, Ingredient.Chop.Diced), new Ingredient(Items.bread, Ingredient.Seasoning.None, Ingredient.Chop.Chopped), new Ingredient(Items.onion, Ingredient.Seasoning.Salt, Ingredient.Chop.Diced), Items.sloppy_joe);
        recipes[5] = new Recipe(new Ingredient(Items.chicken, Ingredient.Seasoning.Pepper, Ingredient.Chop.Minced), new Ingredient(Items.bread, Ingredient.Seasoning.None, Ingredient.Chop.Chopped), new Ingredient(Items.cheese, Ingredient.Seasoning.Salt, Ingredient.Chop.Minced), Items.chicken_burger);
        recipes[6] = new Recipe(new Ingredient(Items.beef, Ingredient.Seasoning.Pepper, Ingredient.Chop.Minced), new Ingredient(Items.bread, Ingredient.Seasoning.None, Ingredient.Chop.Chopped), new Ingredient(Items.cheese, Ingredient.Seasoning.Salt, Ingredient.Chop.Minced), Items.beef_burger);
        recipes[7] = new Recipe(new Ingredient(Items.tomato, Ingredient.Seasoning.Salt, Ingredient.Chop.Diced), new Ingredient(Items.lettuce, Ingredient.Seasoning.None, Ingredient.Chop.Chopped), new Ingredient(Items.cucumber, Ingredient.Seasoning.None, Ingredient.Chop.Diced), Items.garden_salad);
        recipes[8] = new Recipe(new Ingredient(Items.potato, Ingredient.Seasoning.Pepper, Ingredient.Chop.Chopped), new Ingredient(Items.fish, Ingredient.Seasoning.Salt, Ingredient.Chop.Chopped), new Ingredient(Items.peas, Ingredient.Seasoning.None, Ingredient.Chop.Minced), Items.fish_and_chips);
        recipes[9] = new Recipe(new Ingredient(Items.chicken, Ingredient.Seasoning.Pepper, Ingredient.Chop.Chopped), new Ingredient(Items.carrot, Ingredient.Seasoning.Salt, Ingredient.Chop.Chopped), new Ingredient(Items.peas, Ingredient.Seasoning.None, Ingredient.Chop.Chopped), Items.chicken_and_veg);
        recipes[10] = new Recipe(new Ingredient(Items.beef, Ingredient.Seasoning.Pepper, Ingredient.Chop.Chopped), new Ingredient(Items.carrot, Ingredient.Seasoning.Salt, Ingredient.Chop.Chopped), new Ingredient(Items.peas, Ingredient.Seasoning.None, Ingredient.Chop.Chopped), Items.beef_and_veg);
        recipes[11] = new Recipe(new Ingredient(Items.pork, Ingredient.Seasoning.Pepper, Ingredient.Chop.Chopped), new Ingredient(Items.carrot, Ingredient.Seasoning.Salt, Ingredient.Chop.Chopped), new Ingredient(Items.peas, Ingredient.Seasoning.None, Ingredient.Chop.Chopped), Items.pork_and_veg);
        recipes[12] = new Recipe(new Ingredient(Items.egg, Ingredient.Seasoning.Pepper, Ingredient.Chop.Chopped), new Ingredient(Items.pork, Ingredient.Seasoning.None, Ingredient.Chop.Chopped), new Ingredient(Items.tomato, Ingredient.Seasoning.None, Ingredient.Chop.Chopped), Items.english_breakfast);
        recipes[13] = new Recipe(new Ingredient(Items.potato, Ingredient.Seasoning.Pepper, Ingredient.Chop.Minced), new Ingredient(Items.pork, Ingredient.Seasoning.Salt, Ingredient.Chop.Chopped), new Ingredient(Items.coriander, Ingredient.Seasoning.None, Ingredient.Chop.Chopped), Items.bangers_and_mash);
        recipes[14] = new Recipe(new Ingredient(Items.fish, Ingredient.Seasoning.Salt, Ingredient.Chop.None), new Ingredient(Items.ginger, Ingredient.Seasoning.None, Ingredient.Chop.Diced), new Ingredient(Items.spring_onion, Ingredient.Seasoning.None, Ingredient.Chop.Diced), Items.steamed_fish);
        recipes[15] = new Recipe(new Ingredient(Items.chicken, Ingredient.Seasoning.None, Ingredient.Chop.Diced), new Ingredient(Items.rice, Ingredient.Seasoning.None, Ingredient.Chop.None), new Ingredient(Items.spring_onion, Ingredient.Seasoning.None, Ingredient.Chop.Diced), Items.chicken_fried_rice);
        recipes[16] = new Recipe(new Ingredient(Items.beef, Ingredient.Seasoning.None, Ingredient.Chop.Diced), new Ingredient(Items.rice, Ingredient.Seasoning.None, Ingredient.Chop.None), new Ingredient(Items.spring_onion, Ingredient.Seasoning.None, Ingredient.Chop.Diced), Items.beef_fried_rice);
        recipes[17] = new Recipe(new Ingredient(Items.chicken, Ingredient.Seasoning.Pepper, Ingredient.Chop.Diced), new Ingredient(Items.noodles, Ingredient.Seasoning.None, Ingredient.Chop.None), new Ingredient(Items.onion, Ingredient.Seasoning.Salt, Ingredient.Chop.Diced), Items.chicken_chow_mein);
        recipes[18] = new Recipe(new Ingredient(Items.beef, Ingredient.Seasoning.Pepper, Ingredient.Chop.Diced), new Ingredient(Items.noodles, Ingredient.Seasoning.None, Ingredient.Chop.None), new Ingredient(Items.onion, Ingredient.Seasoning.Salt, Ingredient.Chop.Diced), Items.beef_chow_mein);
        recipes[19] = new Recipe(new Ingredient(Items.tofu, Ingredient.Seasoning.None, Ingredient.Chop.Chopped), new Ingredient(Items.beef, Ingredient.Seasoning.Chilli, Ingredient.Chop.Diced), new Ingredient(Items.spring_onion, Ingredient.Seasoning.None, Ingredient.Chop.Diced), Items.ma_po_tofu);
        recipes[20] = new Recipe(new Ingredient(Items.pork, Ingredient.Seasoning.Pepper, Ingredient.Chop.Minced), new Ingredient(Items.dough, Ingredient.Seasoning.None, Ingredient.Chop.Chopped), new Ingredient(Items.ginger, Ingredient.Seasoning.None, Ingredient.Chop.Minced), Items.won_ton);
        //recipes[21] = new Recipe(new Ingredient(Items.dough, Ingredient.Seasoning.None, Ingredient.Chop.Chopped), new Ingredient(Items.lettuce, Ingredient.Seasoning.Salt, Ingredient.Chop.Chopped), new Ingredient(Items.carrot, Ingredient.Seasoning.Pepper, Ingredient.Chop.Minced), Items.spring_rolls);
        recipes[21] = new Recipe(new Ingredient(Items.chicken, Ingredient.Seasoning.Chilli, Ingredient.Chop.Chopped), new Ingredient(Items.garlic, Ingredient.Seasoning.Salt, Ingredient.Chop.Minced), new Ingredient(Items.spring_onion, Ingredient.Seasoning.None, Ingredient.Chop.Chopped), Items.kung_pao_chicken);
        recipes[22] = new Recipe(new Ingredient(Items.dough, Ingredient.Seasoning.None, Ingredient.Chop.None), new Ingredient(Items.cheese, Ingredient.Seasoning.Pepper, Ingredient.Chop.Minced), new Ingredient(Items.tomato, Ingredient.Seasoning.Salt, Ingredient.Chop.Minced), Items.margherita);
        recipes[23] = new Recipe(new Ingredient(Items.pasta, Ingredient.Seasoning.Salt, Ingredient.Chop.None), new Ingredient(Items.tomato, Ingredient.Seasoning.None, Ingredient.Chop.Minced), new Ingredient(Items.beef, Ingredient.Seasoning.Pepper, Ingredient.Chop.Minced), Items.spaghetti_bolognese);
        recipes[24] = new Recipe(new Ingredient(Items.beef, Ingredient.Seasoning.Pepper, Ingredient.Chop.Minced), new Ingredient(Items.cheese, Ingredient.Seasoning.None, Ingredient.Chop.Minced), new Ingredient(Items.tomato, Ingredient.Seasoning.Salt, Ingredient.Chop.Minced), Items.lasagne);
        recipes[25] = new Recipe(new Ingredient(Items.pasta, Ingredient.Seasoning.None, Ingredient.Chop.None), new Ingredient(Items.cheese, Ingredient.Seasoning.None, Ingredient.Chop.Minced), new Ingredient(Items.pork, Ingredient.Seasoning.Pepper, Ingredient.Chop.Minced), Items.carbonara);
        recipes[26] = new Recipe(new Ingredient(Items.rice, Ingredient.Seasoning.None, Ingredient.Chop.None), new Ingredient(Items.mushroom, Ingredient.Seasoning.Pepper, Ingredient.Chop.Minced), new Ingredient(Items.cucumber, Ingredient.Seasoning.Salt, Ingredient.Chop.Diced), Items.mushroom_risotto);
        recipes[27] = new Recipe(new Ingredient(Items.bread, Ingredient.Seasoning.None, Ingredient.Chop.Chopped), new Ingredient(Items.cheese, Ingredient.Seasoning.None, Ingredient.Chop.Minced), new Ingredient(Items.tomato, Ingredient.Seasoning.Salt, Ingredient.Chop.Diced), Items.bruschetta);
        recipes[28] = new Recipe(new Ingredient(Items.beef, Ingredient.Seasoning.Chilli, Ingredient.Chop.Minced), new Ingredient(Items.taco, Ingredient.Seasoning.None, Ingredient.Chop.None), new Ingredient(Items.cheese, Ingredient.Seasoning.Pepper, Ingredient.Chop.Minced), Items.beef_taco);
        recipes[29] = new Recipe(new Ingredient(Items.pork, Ingredient.Seasoning.Chilli, Ingredient.Chop.Minced), new Ingredient(Items.taco, Ingredient.Seasoning.None, Ingredient.Chop.None), new Ingredient(Items.cheese, Ingredient.Seasoning.Pepper, Ingredient.Chop.Minced), Items.pork_taco);
        recipes[30] = new Recipe(new Ingredient(Items.tomato, Ingredient.Seasoning.Salt, Ingredient.Chop.Diced), new Ingredient(Items.onion, Ingredient.Seasoning.None, Ingredient.Chop.Diced), new Ingredient(Items.coriander, Ingredient.Seasoning.None, Ingredient.Chop.Chopped), Items.salsa);
        recipes[31] = new Recipe(new Ingredient(Items.chicken, Ingredient.Seasoning.Chilli, Ingredient.Chop.Minced), new Ingredient(Items.dough, Ingredient.Seasoning.None, Ingredient.Chop.Chopped), new Ingredient(Items.cheese, Ingredient.Seasoning.Pepper, Ingredient.Chop.Minced), Items.chicken_burrito);
        recipes[32] = new Recipe(new Ingredient(Items.pork, Ingredient.Seasoning.Chilli, Ingredient.Chop.Minced), new Ingredient(Items.dough, Ingredient.Seasoning.None, Ingredient.Chop.Chopped), new Ingredient(Items.cheese, Ingredient.Seasoning.Pepper, Ingredient.Chop.Minced), Items.pork_burrito);
        recipes[33] = new Recipe(new Ingredient(Items.avocado, Ingredient.Seasoning.Pepper, Ingredient.Chop.Minced), new Ingredient(Items.tomato, Ingredient.Seasoning.Salt, Ingredient.Chop.Diced), new Ingredient(Items.coriander, Ingredient.Seasoning.None, Ingredient.Chop.Minced), Items.guacamole);
        recipes[34] = new Recipe(new Ingredient(Items.taco, Ingredient.Seasoning.None, Ingredient.Chop.None), new Ingredient(Items.cheese, Ingredient.Seasoning.None, Ingredient.Chop.Minced), new Ingredient(Items.tomato, Ingredient.Seasoning.None, Ingredient.Chop.Minced), Items.nachos);
        SetupRecipeList(ref americanRecipes, 0, 6);
        SetupRecipeList(ref britishRecipes, 7, 13);
        SetupRecipeList(ref chineseRecipes, 14, 21);
        SetupRecipeList(ref italianRecipes, 22, 27);
        SetupRecipeList(ref mexicanRecipes, 28, 34);
    }

    static void SetupRecipeList(ref Recipe[] recipeList, int startingIndex, int endingIndex)
    {
        recipeList = new Recipe[endingIndex - startingIndex + 1];
        for (int i = startingIndex; i <= endingIndex; i++)
        {
            recipeList[i - startingIndex] = recipes[i];
        }
    }
}
