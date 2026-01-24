using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cheeseburger", menuName = "ScriptableObjects/Chef/ScriptableDishes/Cheeseburger")]
public class CheeseburgerDish : Dish {
    public CheeseburgerDish() {
        dishType = EDishType.Cheeseburger;
        name = "Cheeseburger";
    }
    public override int Score(in ChefDie[] dice) {
        //Scores base amount + grade of highest beef, +4 if extra cheese
        int score = baseValue;

        //Check dice for conditional values
        int maxBeefGrade = 1;
        int cheeseCount = 0;
        foreach (var die in dice) {
            if (die.CurrentFace.ingredient == Ingredient.EIngredientType.Beef) {
                if (die.CurrentFace.grade.value > maxBeefGrade) {
                    maxBeefGrade = die.CurrentFace.grade.value;
                }
            }
            else if (die.CurrentFace.ingredient == Ingredient.EIngredientType.Cheese) {
                cheeseCount++;
            }
        }

        //Adjust score based on conditions
        score += maxBeefGrade;
        if (cheeseCount > 1) {
            score += 4;
        }
        return score;
    }
}
