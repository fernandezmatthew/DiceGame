using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScoresheetDisplay : MonoBehaviour
{
    public GameObject sheetDisplayPanelLeft;
    public GameObject sheetDisplayPanelRight;
    public GameObject sheetDisplayPanelMiddle;
    public GameObject sheetDisplayPanelTotal;

    public ScoreEntryDisplay scoreEntryLeftDisplay;
    public ScoreEntryDisplay scoreEntryRightDisplay;
    public DetailEntryDisplay detailEntryLargeDisplay;
    public DetailEntryDisplay detailEntrySmallDisplay;
    public DetailEntryDisplay detailEntryTotalDisplay;

    private List<GameObject> panelDisplays = new List<GameObject>();
    private List<ScoreEntryDisplay> scoreEntryDisplays = new List<ScoreEntryDisplay>();
    private List<DetailEntryDisplay> detailEntryDisplays = new List<DetailEntryDisplay>();

    private Scoresheet scoresheet;

    public List<ScoreEntryDisplay> ScoreEntryDisplays { get { return scoreEntryDisplays; } }

    bool initiated = false;

    private void OnEnable() {
        if (scoreEntryDisplays != null) {
            foreach (ScoreEntryDisplay scoreEntryDisplay in scoreEntryDisplays) {
                if (scoreEntryDisplay != null) {
                    scoreEntryDisplay.entryClicked.AddListener(EntryClicked);
                }
            }
        }
    }

    private void OnDisable() {
        if (scoreEntryDisplays != null) {
            foreach (ScoreEntryDisplay scoreEntryDisplay in scoreEntryDisplays) {
                if (scoreEntryDisplay != null) {
                    scoreEntryDisplay.entryClicked.RemoveListener(EntryClicked);
                }
            }
        }
    }

    private void Start() {
        
    }

    public void Init(Scoresheet scoresheet) {
        if (!initiated) {
            initiated = true;
            this.scoresheet = scoresheet;
            panelDisplays.Add(Instantiate(sheetDisplayPanelLeft, this.transform));
            panelDisplays.Add(Instantiate(sheetDisplayPanelRight, this.transform));
            panelDisplays.Add(Instantiate(sheetDisplayPanelMiddle, this.transform));
            panelDisplays.Add(Instantiate(sheetDisplayPanelTotal, this.transform));

            //Instantiate the 13 score entry displays
            for (int i = 0; i < 6; i++) {
                scoreEntryDisplays.Add(Instantiate(scoreEntryLeftDisplay, panelDisplays[0].transform));
            }

            for (int i = 0; i < 7; i++) {
                scoreEntryDisplays.Add(Instantiate(scoreEntryRightDisplay, panelDisplays[1].transform));
            }

            //Link entries to their displays
            for (int i = 0; i < scoreEntryDisplays.Count; i++) {
                scoreEntryDisplays[i].SetEntry(this.scoresheet.GetScoreEntry(i));
            }

            //Add Scoresheet.FillEntry as listener to each entrydisplay.
            foreach (ScoreEntryDisplay scoreEntryDisplay in scoreEntryDisplays) {
                scoreEntryDisplay.entryClicked.AddListener(EntryClicked);
            }

            //Instantiate the 5 detail displays
            detailEntryDisplays.Add(Instantiate(detailEntryLargeDisplay, panelDisplays[2].transform));
            detailEntryDisplays.Add(Instantiate(detailEntrySmallDisplay, panelDisplays[2].transform));
            detailEntryDisplays.Add(Instantiate(detailEntryLargeDisplay, panelDisplays[2].transform));
            detailEntryDisplays.Add(Instantiate(detailEntrySmallDisplay, panelDisplays[2].transform));
            detailEntryDisplays.Add(Instantiate(detailEntryTotalDisplay, panelDisplays[3].transform));

            //Link entries to their displays
            for (int i = 0; i < detailEntryDisplays.Count;i++) {
                detailEntryDisplays[i].SetEntry(scoresheet.GetDetailEntry(i));
            }
        }
    }

    public void UpdateScoresheetDisplay() {
        //Update the 13 score tabs
        foreach (ScoreEntryDisplay x in scoreEntryDisplays) {
            x.UpdateDisplay();
        }
        //Update the details tabs
        foreach (DetailEntryDisplay x in detailEntryDisplays) {
            x.UpdateDisplay();
        }
    }

    //Called when entry is clicked
    public void EntryClicked(Scoresheet.ScoreEntry scoreEntry) {
        scoresheet.EntryClicked(scoreEntry);
    }
}
