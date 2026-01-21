using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class YahtzeeManager : MonoBehaviour {
    [HideInInspector] public UnityEvent diceRollFinished;
    [HideInInspector] public UnityEvent toggleLockDie;

    private Scoresheet scoresheet;
    private Die[] dice;
    private Camera cam;
    private Button rollButton;
    private PlayerInputActions playerActions;
    private int diceRolling = 0;
    private int rollsRemaining;
    private int currentRound;

    //UI STUFF
    public Canvas canvas;
    public ScoresheetDisplay scoresheetDisplayPrefab;
    public GameInfoDisplay rollsLeftDisplay;
    public GameInfoDisplay currentRoundDisplay;
    private ScoresheetDisplay scoresheetDisplay;

    //Save stuff
    private YahtzeeSaveHandler saveHandler;

    private void Start() {
        //Find scene objects
        dice = FindObjectsOfType<Die>();
        cam = FindObjectOfType<Camera>();
        rollButton = FindObjectOfType<Button>();

        playerActions = new PlayerInputActions();


        //subscribe to all input functions
        playerActions.Enable();
        playerActions.Yahtzee.Select.started += CheckIfDieClicked;
        playerActions.Yahtzee.Restart.started += Restart;
        playerActions.Yahtzee.Quit.started += Quit;
        playerActions.Yahtzee.Save.started += SaveGame;
        rollButton.onClick.AddListener(TryRollDice);

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
        scoresheetDisplay = Instantiate(scoresheetDisplayPrefab, canvas.transform);
        scoresheetDisplay.Init(scoresheet);
        UpdateGameInfoDisplays();

        //This is all save stuff
        saveHandler = new YahtzeeSaveHandler();
    }

    //On Event Trigger Functions
    //*************************************

    //Triggers when button is pressed
    public void TryRollDice() {
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
                    UpdateGameInfoDisplays();
                }
            }
        }
    }

    //Triggers when a die finishes rolling
    public void DecrementDiceRolling() {
        diceRolling -= 1;
        if (diceRolling == 0) {
            ResolveRollFinished();
        }
    }

    //Triggers from decrement when dicerolling = 0
    private void ResolveRollFinished() {
        diceRollFinished.Invoke();
        UpdateScoresheetPotentials(dice);
        UpdateScoresheetDisplay();
    }

    private void UpdateScoresheetDisplay() {
        scoresheetDisplay.UpdateScoresheetDisplay();
    }

    private void UpdateGameInfoDisplays() {
        rollsLeftDisplay.UpdateDisplay(rollsRemaining);
        currentRoundDisplay.UpdateDisplay(currentRound);
    }

    //Triggers from resolveRollFinished() if all dice are finished
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
        UnlockAllDice();
        if (currentRound < 13) {
            currentRound++;
            rollsRemaining = 3;
            UpdateGameInfoDisplays();
        }
        else {
            rollsRemaining = 0;
            //Do game end stuff
            SaveGame();
        }
    }

    private void SaveGame() {
        int[] scoreEntryValues = scoresheet.GetScoreEntryValues();
        int[] detailEntryVaues = scoresheet.GetDetailEntryValues();
        
        saveHandler.Save(scoreEntryValues, detailEntryVaues);
    }

    private void SaveGame(InputAction.CallbackContext ctx) {
        int[] scoreEntryValues = scoresheet.GetScoreEntryValues();
        int[] detailEntryVaues = scoresheet.GetDetailEntryValues();

        saveHandler.Save(scoreEntryValues, detailEntryVaues);
    }

    private void UnlockAllDice() {
        foreach (var die in dice) {
            if (die.IsLocked) {
                die.transform.Translate(new Vector3(0, -.2f, 0));
                die.ToggleLock();
            }
        }
    }

    private void ResetAllDice() {
        foreach (var die in dice) {
            die.Reset();
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
    public void Restart(InputAction.CallbackContext ctx) {
        //instead of reloading the current scene, lets reset the game in-place
        UnlockAllDice();
        ResetAllDice();
        currentRound = 1;
        rollsRemaining = 3;
        diceRolling = 0;
        scoresheet.ResetScoresheet();
        scoresheetDisplay.UpdateScoresheetDisplay();
        UpdateGameInfoDisplays();
    }

    public void Quit(InputAction.CallbackContext ctx) {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
