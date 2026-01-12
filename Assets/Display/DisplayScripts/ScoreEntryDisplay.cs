using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreEntryDisplay : MonoBehaviour
{
    public TMP_Text entryNameText;
    public TMP_Text scoreText;
    public TMP_Text potentialScoreText;
    private bool isFilled;

    private Scoresheet.ScoreEntry scoreEntry;

    void Update() {
        //This can be moved to an event so its not checking every frame
        if (scoreEntry != null) {
            if (isFilled) {
                scoreText.enabled = true;
                potentialScoreText.enabled = false;
            }
            else {
                scoreText.enabled = false;
                potentialScoreText.enabled = true;
            }
        }
    }


    public void SetEntry(Scoresheet.ScoreEntry scoreEntry) { 
        this.scoreEntry = scoreEntry;
        entryNameText.text = scoreEntry.displayName;
        scoreText.text = scoreEntry.score.ToString();
        potentialScoreText.text = scoreEntry.potentialScore.ToString();
        isFilled = scoreEntry.isFilled;
    }

    public void UpdateDisplay() {
        entryNameText.text = scoreEntry.displayName;
        scoreText.text = scoreEntry.score.ToString();
        potentialScoreText.text = scoreEntry.potentialScore.ToString();
        isFilled = scoreEntry.isFilled;
    }
}
