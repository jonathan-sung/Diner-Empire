using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Items
{
    //Ingredients
    public static Sprite avocado_sprite = Resources.Load<Sprite>("Sprites/Food/Ingredients/avocado");
    public static Sprite beef_sprite = Resources.Load<Sprite>("Sprites/Food/Ingredients/beef");
    public static Sprite bread_sprite = Resources.Load<Sprite>("Sprites/Food/Ingredients/bread");
    public static Sprite carrot_sprite = Resources.Load<Sprite>("Sprites/Food/Ingredients/carrot");
    public static Sprite cheese_sprite = Resources.Load<Sprite>("Sprites/Food/Ingredients/cheese");
    public static Sprite chicken_sprite = Resources.Load<Sprite>("Sprites/Food/Ingredients/chicken");
    public static Sprite coriander_sprite = Resources.Load<Sprite>("Sprites/Food/Ingredients/coriander");
    public static Sprite cucumber_sprite = Resources.Load<Sprite>("Sprites/Food/Ingredients/cucumber");
    public static Sprite dough_sprite = Resources.Load<Sprite>("Sprites/Food/Ingredients/dough");
    public static Sprite egg_sprite = Resources.Load<Sprite>("Sprites/Food/Ingredients/egg");
    public static Sprite fish_sprite = Resources.Load<Sprite>("Sprites/Food/Ingredients/fish");
    public static Sprite garlic_sprite = Resources.Load<Sprite>("Sprites/Food/Ingredients/garlic");
    public static Sprite ginger_sprite = Resources.Load<Sprite>("Sprites/Food/Ingredients/ginger");
    public static Sprite lettuce_sprite = Resources.Load<Sprite>("Sprites/Food/Ingredients/lettuce");
    public static Sprite mushroom_sprite = Resources.Load<Sprite>("Sprites/Food/Ingredients/mushroom");
    public static Sprite noodles_sprite = Resources.Load<Sprite>("Sprites/Food/Ingredients/noodles");
    public static Sprite onion_sprite = Resources.Load<Sprite>("Sprites/Food/Ingredients/onion");
    public static Sprite parsley_sprite = Resources.Load<Sprite>("Sprites/Food/Ingredients/parsley");
    public static Sprite pasta_sprite = Resources.Load<Sprite>("Sprites/Food/Ingredients/pasta");
    public static Sprite peas_sprite = Resources.Load<Sprite>("Sprites/Food/Ingredients/peas");
    public static Sprite pork_sprite = Resources.Load<Sprite>("Sprites/Food/Ingredients/pork");
    public static Sprite potato_sprite = Resources.Load<Sprite>("Sprites/Food/Ingredients/potato");
    public static Sprite rice_sprite = Resources.Load<Sprite>("Sprites/Food/Ingredients/rice");
    public static Sprite spring_onion_sprite = Resources.Load<Sprite>("Sprites/Food/Ingredients/spring_onion");
    public static Sprite taco_sprite = Resources.Load<Sprite>("Sprites/Food/Ingredients/taco");
    public static Sprite tofu_sprite = Resources.Load<Sprite>("Sprites/Food/Ingredients/tofu");
    public static Sprite tomato_sprite = Resources.Load<Sprite>("Sprites/Food/Ingredients/tomato");

    public static Sprite tomato_chopped_sprite = Resources.Load<Sprite>("Sprites/Food/Ingredients/tomato_chopped");
    public static Sprite chicken_chopped_sprite = Resources.Load<Sprite>("Sprites/Food/Ingredients/chicken_chopped");
    public static Sprite chicken_diced_sprite = Resources.Load<Sprite>("Sprites/Food/Ingredients/chicken_diced");
    public static Sprite chicken_minced_sprite = Resources.Load<Sprite>("Sprites/Food/Ingredients/chicken_minced");

    //Dishes
    public static Sprite chicken_soup_sprite = Resources.Load<Sprite>("Sprites/Food/Dishes/chicken_soup");
    public static Sprite fried_chicken_sprite = Resources.Load<Sprite>("Sprites/Food/Dishes/fried_chicken");
    public static Sprite steak_and_chips_sprite = Resources.Load<Sprite>("Sprites/Food/Dishes/steak_and_chips");
    public static Sprite bbq_spare_ribs_sprite = Resources.Load<Sprite>("Sprites/Food/Dishes/bbq_spare_ribs");
    public static Sprite sloppy_joe_sprite = Resources.Load<Sprite>("Sprites/Food/Dishes/sloppy_joe");
    public static Sprite chicken_burger_sprite = Resources.Load<Sprite>("Sprites/Food/Dishes/chicken_burger");
    public static Sprite beef_burger_sprite = Resources.Load<Sprite>("Sprites/Food/Dishes/beef_burger");
    public static Sprite garden_salad_sprite = Resources.Load<Sprite>("Sprites/Food/Dishes/garden_salad");
    public static Sprite fish_and_chips_sprite = Resources.Load<Sprite>("Sprites/Food/Dishes/fish_and_chips");
    public static Sprite chicken_and_veg_sprite = Resources.Load<Sprite>("Sprites/Food/Dishes/chicken_and_veg");
    public static Sprite beef_and_veg_sprite = Resources.Load<Sprite>("Sprites/Food/Dishes/beef_and_veg");
    public static Sprite pork_and_veg_sprite = Resources.Load<Sprite>("Sprites/Food/Dishes/pork_and_veg");
    public static Sprite english_breakfast_sprite = Resources.Load<Sprite>("Sprites/Food/Dishes/english_breakfast");
    public static Sprite bangers_and_mash_sprite = Resources.Load<Sprite>("Sprites/Food/Dishes/bangers_and_mash");
    public static Sprite steamed_fish_sprite = Resources.Load<Sprite>("Sprites/Food/Dishes/steamed_fish");
    public static Sprite chicken_fried_rice_sprite = Resources.Load<Sprite>("Sprites/Food/Dishes/chicken_fried_rice");
    public static Sprite beef_fried_rice_sprite = Resources.Load<Sprite>("Sprites/Food/Dishes/beef_fried_rice");
    public static Sprite chicken_chow_mein_sprite = Resources.Load<Sprite>("Sprites/Food/Dishes/chicken_chow_mein");
    public static Sprite beef_chow_mein_sprite = Resources.Load<Sprite>("Sprites/Food/Dishes/beef_chow_mein");
    public static Sprite ma_po_tofu_sprite = Resources.Load<Sprite>("Sprites/Food/Dishes/ma_po_tofu");
    public static Sprite won_ton_sprite = Resources.Load<Sprite>("Sprites/Food/Dishes/won_ton");
    public static Sprite spring_rolls_sprite = Resources.Load<Sprite>("Sprites/Food/Dishes/chicken_soup");
    public static Sprite kung_pao_chicken_sprite = Resources.Load<Sprite>("Sprites/Food/Dishes/kung_pao_chicken");
    public static Sprite margherita_sprite = Resources.Load<Sprite>("Sprites/Food/Dishes/margherita");
    public static Sprite spaghetti_bolognese_sprite = Resources.Load<Sprite>("Sprites/Food/Dishes/spaghetti_bolognese");
    public static Sprite lasagne_sprite = Resources.Load<Sprite>("Sprites/Food/Dishes/lasagne");
    public static Sprite carbonara_sprite = Resources.Load<Sprite>("Sprites/Food/Dishes/carbonara");
    public static Sprite mushroom_risotto_sprite = Resources.Load<Sprite>("Sprites/Food/Dishes/mushroom_risotto");
    public static Sprite bruschetta_sprite = Resources.Load<Sprite>("Sprites/Food/Dishes/bruschetta");
    public static Sprite beef_taco_sprite = Resources.Load<Sprite>("Sprites/Food/Dishes/beef_taco");
    public static Sprite pork_taco_sprite = Resources.Load<Sprite>("Sprites/Food/Dishes/pork_taco");
    public static Sprite salsa_sprite = Resources.Load<Sprite>("Sprites/Food/Dishes/salsa");
    public static Sprite chicken_burrito_sprite = Resources.Load<Sprite>("Sprites/Food/Dishes/chicken_burrito");
    public static Sprite pork_burrito_sprite = Resources.Load<Sprite>("Sprites/Food/Dishes/pork_burrito");
    public static Sprite guacamole_sprite = Resources.Load<Sprite>("Sprites/Food/Dishes/guacamole");
    public static Sprite nachos_sprite = Resources.Load<Sprite>("Sprites/Food/Dishes/nachos");

    //UI
    public static Sprite slot_1 = Resources.Load<Sprite>("Sprites/slot_1");
    public static Sprite slot_2 = Resources.Load<Sprite>("Sprites/slot_2");

    public static Sprite panel_3 = Resources.Load<Sprite>("Sprites/panel_3");
    public static Sprite panel_4 = Resources.Load<Sprite>("Sprites/panel_4");
    public static Sprite panel_5 = Resources.Load<Sprite>("Sprites/panel_5");

    public static Sprite play = Resources.Load<Sprite>("Sprites/play");
    public static Sprite pause = Resources.Load<Sprite>("Sprites/pause");
    public static Sprite slider_background = Resources.Load<Sprite>("Sprites/slider_background");
    public static Sprite slider_fill = Resources.Load<Sprite>("Sprites/slider_fill");

    //World Canvas
    public static Sprite tile_overlay_green = Resources.Load<Sprite>("Sprites/tile_overlay_green");
    public static Sprite tile_overlay_red = Resources.Load<Sprite>("Sprites/tile_overlay_red");

    //Furniture
    public static Sprite normalChair = Resources.Load<Sprite>("Sprites/chair_2");
    public static Sprite sittingChair = Resources.Load<Sprite>("Sprites/chair_3");

    /*Objects*/
    //Ingredients
    public static Ingredient avocado = new Ingredient(avocado_sprite, "Avocado", Ingredient.Seasoning.None, Ingredient.Chop.None);
    public static Ingredient beef = new Ingredient(beef_sprite, "Beef", Ingredient.Seasoning.None, Ingredient.Chop.None);
    public static Ingredient bread = new Ingredient(bread_sprite, "Bread", Ingredient.Seasoning.None, Ingredient.Chop.None);
    public static Ingredient carrot = new Ingredient(carrot_sprite, "Carrot", Ingredient.Seasoning.None, Ingredient.Chop.None);
    public static Ingredient cheese = new Ingredient(cheese_sprite, "Cheese", Ingredient.Seasoning.None, Ingredient.Chop.None);
    public static Ingredient chicken = new Ingredient(chicken_sprite, "Chicken", Ingredient.Seasoning.None, Ingredient.Chop.None);
    public static Ingredient coriander = new Ingredient(coriander_sprite, "Coriander", Ingredient.Seasoning.None, Ingredient.Chop.None);
    public static Ingredient cucumber = new Ingredient(cucumber_sprite, "Cucumber", Ingredient.Seasoning.None, Ingredient.Chop.None);
    public static Ingredient dough = new Ingredient(dough_sprite, "Dough", Ingredient.Seasoning.None, Ingredient.Chop.None);
    public static Ingredient egg = new Ingredient(egg_sprite, "Egg", Ingredient.Seasoning.None, Ingredient.Chop.None);
    public static Ingredient fish = new Ingredient(fish_sprite, "Fish", Ingredient.Seasoning.None, Ingredient.Chop.None);
    public static Ingredient garlic = new Ingredient(garlic_sprite, "Garlic", Ingredient.Seasoning.None, Ingredient.Chop.None);
    public static Ingredient ginger = new Ingredient(ginger_sprite, "Ginger", Ingredient.Seasoning.None, Ingredient.Chop.None);
    public static Ingredient lettuce = new Ingredient(lettuce_sprite, "Lettuce", Ingredient.Seasoning.None, Ingredient.Chop.None);
    public static Ingredient mushroom = new Ingredient(mushroom_sprite, "Mushroom", Ingredient.Seasoning.None, Ingredient.Chop.None);
    public static Ingredient noodles = new Ingredient(noodles_sprite, "Noodles", Ingredient.Seasoning.None, Ingredient.Chop.None);
    public static Ingredient onion = new Ingredient(onion_sprite, "Onion", Ingredient.Seasoning.None, Ingredient.Chop.None);
    public static Ingredient parsley = new Ingredient(parsley_sprite, "Parsley", Ingredient.Seasoning.None, Ingredient.Chop.None);
    public static Ingredient pasta = new Ingredient(pasta_sprite, "Pasta", Ingredient.Seasoning.None, Ingredient.Chop.None);
    public static Ingredient peas = new Ingredient(peas_sprite, "Peas", Ingredient.Seasoning.None, Ingredient.Chop.None);
    public static Ingredient pork = new Ingredient(pork_sprite, "Pork", Ingredient.Seasoning.None, Ingredient.Chop.None);
    public static Ingredient potato = new Ingredient(potato_sprite, "Potato", Ingredient.Seasoning.None, Ingredient.Chop.None);
    public static Ingredient rice = new Ingredient(rice_sprite, "Rice", Ingredient.Seasoning.None, Ingredient.Chop.None);
    public static Ingredient spring_onion = new Ingredient(spring_onion_sprite, "Spring Onion", Ingredient.Seasoning.None, Ingredient.Chop.None);
    public static Ingredient taco = new Ingredient(taco_sprite, "Taco", Ingredient.Seasoning.None, Ingredient.Chop.None);
    public static Ingredient tofu = new Ingredient(tofu_sprite, "Tofu", Ingredient.Seasoning.None, Ingredient.Chop.None);
    public static Ingredient tomato = new Ingredient(tomato_sprite, "Tomato", Ingredient.Seasoning.None, Ingredient.Chop.None);


    //Dishes
    public static Dish chicken_soup = new Dish("Chicken Soup", chicken_soup_sprite, 3, 10);
    public static Dish fried_chicken = new Dish("Fried Chicken", fried_chicken_sprite, 5, 10);
    public static Dish steak_and_chips = new Dish("Steak and Chips", steak_and_chips_sprite, 8, 20);
    public static Dish bbq_spare_ribs = new Dish("BBQ Spare Ribs", bbq_spare_ribs_sprite, 8, 28);
    public static Dish sloppy_joe = new Dish("Sloppy Joe", sloppy_joe_sprite, 5, 10);
    public static Dish chicken_burger = new Dish("Chicken Burger", chicken_burger_sprite, 3, 25);
    public static Dish beef_burger = new Dish("Beef Burger", beef_burger_sprite, 3, 25);
    public static Dish garden_salad = new Dish("Garden Salad", garden_salad_sprite, 0.1f, 5);
    public static Dish fish_and_chips = new Dish("Fish and Chips", fish_and_chips_sprite, 10, 30);
    public static Dish chicken_and_veg = new Dish("Chicken and Veg", chicken_and_veg_sprite, 10, 20);
    public static Dish beef_and_veg = new Dish("Beef and Veg", beef_and_veg_sprite, 10, 20);
    public static Dish pork_and_veg = new Dish("Pork and Veg", pork_and_veg_sprite, 10, 20);
    public static Dish english_breakfast = new Dish("English Breakfast", english_breakfast_sprite, 5, 10);
    public static Dish bangers_and_mash = new Dish("Bangers and Mash", bangers_and_mash_sprite, 5, 15);
    public static Dish steamed_fish = new Dish("Steamed Fish", steamed_fish_sprite, 10, 20);
    public static Dish chicken_fried_rice = new Dish("Chicken Fried Rice", chicken_fried_rice_sprite, 3, 10);
    public static Dish beef_fried_rice = new Dish("Beef Fried Rice", beef_fried_rice_sprite, 3, 10);
    public static Dish chicken_chow_mein = new Dish("Chicken Chow Mein", chicken_chow_mein_sprite, 3, 10);
    public static Dish beef_chow_mein = new Dish("Beef Chow Mein", beef_chow_mein_sprite, 3, 10);
    public static Dish ma_po_tofu = new Dish("Ma Po Tofu", ma_po_tofu_sprite, 5, 15);
    public static Dish won_ton = new Dish("Won Ton", won_ton_sprite, 5, 30);
    public static Dish spring_rolls = new Dish("Spring Rolls", spring_rolls_sprite, 3, 25);
    public static Dish kung_pao_chicken = new Dish("Kung Pao Chicken", kung_pao_chicken_sprite, 5, 25);
    public static Dish margherita = new Dish("Margherita", margherita_sprite, 10, 35);
    public static Dish spaghetti_bolognese = new Dish("Spaghetti Bolognese", spaghetti_bolognese_sprite, 5, 30);
    public static Dish lasagne = new Dish("Lasagne", lasagne_sprite, 15, 50);
    public static Dish carbonara = new Dish("Carbonara", carbonara_sprite, 5, 25);
    public static Dish mushroom_risotto = new Dish("Mushroom Risotto", mushroom_risotto_sprite, 10, 30);
    public static Dish bruschetta = new Dish("Bruschetta", bruschetta_sprite, 2, 10);
    public static Dish beef_taco = new Dish("Beef Taco", beef_taco_sprite, 2, 25);
    public static Dish pork_taco = new Dish("Pork Taco", pork_taco_sprite, 2, 25);
    public static Dish salsa = new Dish("Salsa", salsa_sprite, 0.1f, 10);
    public static Dish chicken_burrito = new Dish("Chicken Burrito", chicken_burrito_sprite, 2, 25);
    public static Dish pork_burrito = new Dish("Pork Burrito", pork_burrito_sprite, 2, 25);
    public static Dish guacamole = new Dish("Guacamole", guacamole_sprite, 0.1f, 25);
    public static Dish nachos = new Dish("Nachos", nachos_sprite, 2, 25);

    public static Sprite ChoppedSprite(Sprite sprite)
    {
        Sprite choppedSprite = sprite;
        if (sprite == tomato_sprite)
        {
            choppedSprite = tomato_chopped_sprite;
        }
        else if (sprite == chicken_sprite)
        {
            choppedSprite = chicken_chopped_sprite;
        }
        else if (sprite == chicken_chopped_sprite)
        {
            choppedSprite = chicken_diced_sprite;
        }
        else if (sprite == chicken_diced_sprite)
        {
            choppedSprite = chicken_minced_sprite;
        }
        return choppedSprite;
    }
}


