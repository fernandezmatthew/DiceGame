using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ScriptableDie", menuName = "ScriptableObjects/ScriptableDie")]
public class ScriptableDie : ScriptableObject {
    [SerializeField] public Die.Face[] faces;
    public int NumFaces { get { return faces.Length; } }

    private SpriteRenderer spriteRenderer;
}
