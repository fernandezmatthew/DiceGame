using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSheetDisplay : MonoBehaviour
{
    public List<GameObject> displayPanels = new List<GameObject>();

    private void Start() {
        foreach (GameObject panel in displayPanels) {
            Instantiate(panel);
        }
    }
}
