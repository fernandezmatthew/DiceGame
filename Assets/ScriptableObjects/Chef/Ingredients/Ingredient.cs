using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableIngredient", menuName = "ScriptableObjects/Chef/ScriptableIngredient")]
public class Ingredient : ScriptableObject {
    public EIngredientType IngredientType;
    public Sprite IngredientSprite;

    public enum EIngredientType {
        Default,
        Bread,
        Potato,
        Beef,
        Chicken,
        Cheese,
        Chili,
        Aromatics
    }

    public static bool operator ==(Ingredient ingredient,EIngredientType type) {
        return ingredient.IngredientType == type;
    }

    public static bool operator !=(Ingredient ingredient, EIngredientType type) {
        return ingredient.IngredientType != type;
    }
}
