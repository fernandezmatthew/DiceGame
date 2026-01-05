using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ScriptableDie", menuName = "ScriptableObjects/ScriptableDie")]
public class ScriptableDie : ScriptableObject
{
    [System.Serializable]
    public struct Face {
        public Sprite sprite;
        public int value;
    }

    [SerializeField] public Face[] faces;

    private SpriteRenderer spriteRenderer;
}
