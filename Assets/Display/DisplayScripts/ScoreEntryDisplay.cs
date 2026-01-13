using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class ScoreEntryDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public UnityEvent<Scoresheet.ScoreEntry> entryClicked;

    public TMP_Text entryNameText;
    public TMP_Text scoreText;
    public TMP_Text potentialScoreText;
    private bool isFilled;

    private Scoresheet.ScoreEntry scoreEntry;


    public void SetEntry(Scoresheet.ScoreEntry scoreEntry) { 
        this.scoreEntry = scoreEntry;
        entryNameText.text = scoreEntry.displayName;
        scoreText.text = scoreEntry.score.ToString();
        potentialScoreText.text = scoreEntry.potentialScore.ToString();
        isFilled = scoreEntry.isFilled;

        scoreText.enabled = false;
        potentialScoreText.enabled = false;
    }

    public void UpdateDisplay() {
        entryNameText.text = scoreEntry.displayName;
        scoreText.text = scoreEntry.score.ToString();
        potentialScoreText.text = scoreEntry.potentialScore.ToString();
        isFilled = scoreEntry.isFilled;
        if (isFilled) {
            scoreText.enabled = true;
            potentialScoreText.enabled = false;
        }
    }

    //Functions triggered from Mouse
    public void OnPointerEnter(PointerEventData eventData) {
        if (!isFilled) {
            potentialScoreText.enabled = true;
        }
    }

    public void OnPointerExit(PointerEventData eventData) { 
        potentialScoreText.enabled = false;
    }

    public void OnPointerClick(PointerEventData eventData) {
        entryClicked.Invoke(scoreEntry);
    }
}
