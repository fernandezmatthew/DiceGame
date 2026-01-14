using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DetailEntryDisplay : MonoBehaviour
{
    public TMP_Text entryNameText;
    public TMP_Text scoreText;

    private Scoresheet.DetailEntry detailEntry;
    public void SetEntry(Scoresheet.DetailEntry detailEntry) {
        this.detailEntry = detailEntry;
        entryNameText.text = detailEntry.displayName;
        scoreText.text = detailEntry.score.ToString();
    }

    public void UpdateDisplay() {
        entryNameText.text = detailEntry.displayName;
        scoreText.text = detailEntry.score.ToString();
    }
}
