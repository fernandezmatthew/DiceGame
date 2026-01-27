using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Cheeseburger", menuName = "ScriptableObjects/Chef/ScriptableDishes/Cheeseburger")]
public class CheeseburgerDish : Dish {
    public CheeseburgerDish() {
        dishType = EDishType.Cheeseburger;
        dishName = "Cheeseburger";
    }
    public override int Score(in ChefDie[] dice) {
        //Scores base amount + grade of highest beef, +4 if extra cheese
        int score = baseValue;

        //Check dice for conditional values
        int totalBeefGrade = 0;
        foreach (var die in dice) {
            if (die.CurrentFace.ingredient.IsType(Ingredient.EIngredientType.Beef)) {
                totalBeefGrade += die.CurrentFace.grade.value;
            }
        }

        //Adjust score based on conditions
        score += totalBeefGrade;
        
        return score;
    }
}
