using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableChefDieRank", menuName = "ScriptableObjects/Chef/ScriptableChefDieRank")]
public class ChefDieGrade : ScriptableObject {
    public int value;
    public Sprite gradeSprite;
}
