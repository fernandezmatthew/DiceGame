using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    Die[] dice;
    Camera cam;

    int diceRolling = 0;

    //These two should have ui elements
    private int rollsRemaining;
    private int currentRound;

    public UnityEvent diceRollFinished;
    public UnityEvent toggleLockDie;

    private Scoresheet scoresheet;
    //UI STUFF
    public ScoresheetDisplay scoresheetDisplay;

    void OnEnable() {
        //Subscribe to each die's "finishedRolling" event
        //This won't happen on start up cuz dice will == null. So we will do it in start(). But if we re-enable, we want the events back.
        if (dice != null) {
            foreach (Die die in dice) {
                die.finishedRolling.AddListener(DecrementDiceRolling);
            }
        }
        if (scoresheet != null) {
            scoresheet.entryClicked.AddListener(AttemptFillEntry);
        }
    }

    private void OnDisable()
    {
        if (dice != null) {
            foreach (Die die in dice) {
                die.finishedRolling.RemoveListener(DecrementDiceRolling);
            }
        }
        if (scoresheet != null) {
            scoresheet.entryClicked.RemoveListener(AttemptFillEntry);
        }
    }

    private void Update() {

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
        scoresheet = new Scoresheet();
        scoresheet.entryClicked.AddListener(AttemptFillEntry);
        scoresheet.entryFilled.AddListener(UpdateRound);

        rollsRemaining = 3;
        currentRound = 1;

        //THIS IS ALL UI STUFF
        scoresheetDisplay.Init(scoresheet);
    }

    //On Event Trigger Functions
    //*************************************

    //Triggers when button is pressed
    public void RollDice() {
        if (dice != null) {
            if (diceRolling == 0 && rollsRemaining > 0 && currentRound <= 13) {
                foreach (Die die in dice) {
                    if (!die.IsLocked) {
                        diceRolling++;
                        float rollLength = UnityEngine.Random.Range(.8f, 1.2f);
                        die.Roll(rollLength);
                    }
                }
                if (diceRolling > 0) {
                    rollsRemaining--;
                    //UPDATE DISPLAY?
                }
            }
        }
    }

    //Triggers when a die finishes rolling
    public void DecrementDiceRolling() {
        diceRolling -= 1;
        if (diceRolling == 0) {
            diceRollFinished.Invoke();
            UpdateScoresheetPotentials(dice);
            UpdateScoresheetDisplay();
        }
    }

    private void UpdateScoresheetDisplay() {
        scoresheetDisplay.UpdateScoresheetDisplay();
    }

    //Triggers from DecrementDiceRolling if all dice are finished
    private void UpdateScoresheetPotentials(in Die[] dice) {
        scoresheet.UpdatePotentials(dice);
    }

    //triggers from scoresheet.entryClicked
    public void AttemptFillEntry(Scoresheet.ScoreEntry scoreEntry) {
        //Make sure dice not rolling
        if (diceRolling == 0) {
            //Make sure player has made at least one roll
            if (rollsRemaining < 3) { 
                scoresheet.FillEntry(scoreEntry);
            }
        }
    }

    //Called from scoresheet.entryFilled
    public void UpdateRound() {
        UpdateScoresheetDisplay();
        foreach (var die in dice) {
            if (die.IsLocked) {
                die.transform.Translate(new Vector3(0, -.2f, 0));
                die.ToggleLock();
            }
        }
        if (currentRound < 13) {
            currentRound++;
            rollsRemaining = 3;
        }
        else {
            rollsRemaining = 0;
            //Do game end stuff
        }
    }

    //Triggers when left mouse button clicked at all
    public void CheckIfDieClicked(InputAction.CallbackContext ctx) {
        if (ctx.phase == InputActionPhase.Started) {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

            // Check if clicked on a die.
            if (hit.collider != null) {
                foreach (Die die in dice) {
                    if (die.gameObject == hit.collider.gameObject) {
                        //Toggle die lock if dice not currently rolling
                        if (diceRolling == 0 && rollsRemaining < 3) {
                            if (!die.IsLocked) {
                                die.transform.Translate(new Vector3(0, .2f, 0));
                            }
                            else {
                                die.transform.Translate(new Vector3(0, -.2f, 0));
                            }
                            
                            die.ToggleLock();
                            break;
                        }
                    }
                }
            }
        }
    }

    //called from 'reset' inputaction
    public void Restart() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
