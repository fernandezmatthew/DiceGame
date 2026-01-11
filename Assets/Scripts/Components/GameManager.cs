using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    Die[] dice;
    Camera cam;

    int diceRolling = 0;

    public UnityEvent diceRollFinished;
    public UnityEvent toggleLockDie;

    private ScoreSheet scoreSheet;
    private YahtzeeScoreEntryDisplay[] entryDisplays;

    void OnEnable() {
        //Subscribe to each die's "finishedRolling" event
        //This won't happen on start up cuz dice will == null. So we will do it in start(). But if we re-enable, we want the events back.
        if (dice != null) {
            foreach (Die die in dice) {
                die.finishedRolling.AddListener(DecrementDiceRolling);
            }
        }
    }

    private void OnDisable()
    {
        if (dice != null) {
            foreach (Die die in dice) {
                die.finishedRolling.RemoveListener(DecrementDiceRolling);
            }
        }
    }

    private void Start()
    {
        //Find scene objects
        dice = FindObjectsOfType<Die>();
        cam = FindObjectOfType<Camera>();

        //Subscribe to each die's "finishedRolling" event
        if (dice != null) {
            foreach (Die die in dice) {
                die.finishedRolling.AddListener(DecrementDiceRolling);
            }
        }

        //Make yahtzee scoresheet
        scoreSheet = new YahtzeeScoreSheet();
        ScoreSheet.ScoreEntry[] yahtzeeEntries = scoreSheet.Entries;
        entryDisplays = FindObjectsOfType<YahtzeeScoreEntryDisplay>();
        entryDisplays[0].SetEntry((YahtzeeScoreSheet.YahtzeeScoreEntry)yahtzeeEntries[0]);
    }

    private void Update() {

    }

    private void UpdateScoreSheet(in Die[] dice) {
        scoreSheet.UpdateScoreSheet(dice);
        for (int i = 0; i < entryDisplays.Length; i++) {
            entryDisplays[i].UpdateDisplay();
        }
    }


    //On Event Trigger Functions
    //*************************************

    //Triggers when button is pressed
    public void RollDice() {
        if (dice != null) {
            if (diceRolling == 0) {
                foreach (Die die in dice) {
                    if (!die.IsLocked) {
                        diceRolling++;
                        float rollLength = UnityEngine.Random.Range(.8f, 1.2f);
                        die.Roll(rollLength);
                    }
                }
                if (diceRolling > 0) {
                    // this is a new roll. Increment number of rolls.
                }
            }
        }
    }

    //Triggers when a die finishes rolling
    public void DecrementDiceRolling() {
        diceRolling -= 1;
        if (diceRolling == 0) {
            diceRollFinished.Invoke();
            UpdateSumOfDice();
        }
    }

    //Triggers from DecrementDiceRolling if all dice are finished
    public void UpdateSumOfDice() { 
        if (dice != null) {
            int sum = 0;
            foreach (Die die in dice) {
                sum += die.CurrentFace.value;
            }
        }
        UpdateScoreSheet(dice);
    }

    //Triggers when left mouse button clicked
    public void CheckWhatsClicked(InputAction.CallbackContext ctx) {
        if (ctx.phase == InputActionPhase.Started) {
            // Check if clicked on a die.
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

            if (hit.collider != null) {
                foreach (Die die in dice) {
                    if (die.gameObject == hit.collider.gameObject) {
                        if (diceRolling == 0) {
                            die.ToggleLock();
                            break;
                        }
                    }
                }
            }
        }
    }
}
