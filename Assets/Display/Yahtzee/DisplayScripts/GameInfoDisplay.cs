using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameInfoDisplay : MonoBehaviour
{
    public TMP_Text scoreText;
    // Start is called before the first frame update
    public void UpdateDisplay(int val) {
        scoreText.text = val.ToString();
    }
}
