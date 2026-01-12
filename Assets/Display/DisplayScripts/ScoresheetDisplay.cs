using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoresheetDisplay : MonoBehaviour
{
    public GameObject sheetDisplayPanelLeft;
    public GameObject sheetDisplayPanelRight;
    public GameObject sheetDisplayPanelMiddle;
    public GameObject sheetDisplayPanelTotal;

    private List<GameObject> displayPanels = new List<GameObject>();

    bool initiated = false;

    private void Start() {
        Init();
    }

    public void Init() {
        if (!initiated) {
            initiated = true;
            displayPanels.Add(sheetDisplayPanelLeft);
            displayPanels.Add(sheetDisplayPanelRight);
            displayPanels.Add(sheetDisplayPanelMiddle);
            displayPanels.Add(sheetDisplayPanelTotal);

            foreach (GameObject panel in displayPanels) {
                Instantiate(panel, this.transform);
            }
        }
    }
}
