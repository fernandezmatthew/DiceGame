using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class YahtzeeGameData {
    [SerializeField] int[] rollScores;
    [SerializeField] int[] gameScores;
    [SerializeField] int[] date;

    public YahtzeeGameData(int[] rollScores, int[] gameScores, int[] date) {
        this.rollScores = rollScores;
        this.gameScores = gameScores;
        this.date = date;
    }
}
