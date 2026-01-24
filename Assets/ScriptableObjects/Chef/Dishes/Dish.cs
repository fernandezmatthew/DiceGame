using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Dish : ScriptableObject {
    public Sprite sprite;
    public List<Ingredient> recipe;
    public int baseValue;

    protected string name;
    public enum EDishType {
        Default,
        Cheeseburger,
        FrenchFries,
        ChickenNuggets
    }
    protected EDishType dishType;

    public string DishName { get { return name; } }
    public EDishType DishType { get { return dishType; } }

    public abstract int Score(in ChefDie[] dice);
}
