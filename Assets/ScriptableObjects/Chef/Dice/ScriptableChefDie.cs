using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScriptableChefDie", menuName = "ScriptableObjects/Chef/ScriptableChefDie")]
public class ScriptableChefDie : ScriptableObject {
    public ChefDie.Face[] faces;
    public Sprite background;
    public Color backgroundColor;
}
