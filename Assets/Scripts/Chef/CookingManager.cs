using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CookingManager : MonoBehaviour {
    [HideInInspector] public UnityEvent diceRollFinished;
    [HideInInspector] public UnityEvent toggleLockDie;

    private ChefDie[] dice;
    private Camera cam;
    private Button rollButton;
    private PlayerInputActions playerActions;
    private int diceRolling = 0;
    private int rollsRemaining;

    //UI STUFF
    public Canvas canvas;
    public GameInfoDisplay rollsLeftDisplay;

    private void Start() {
        //Find scene objects
        dice = FindObjectsOfType<ChefDie>();
        cam = FindObjectOfType<Camera>();
        rollButton = FindObjectOfType<Button>();

        playerActions = new PlayerInputActions();


        //subscribe to all input functions
        playerActions.Enable();
        playerActions.Yahtzee.Select.started += CheckIfDieClicked;
        playerActions.Yahtzee.Restart.started += Restart;
        rollButton.onClick.AddListener(TryRollDice);

        //Subscribe to each die's "finishedRolling" event
        if (dice != null) {
            foreach (ChefDie die in dice) {
                die.finishedRolling.AddListener(DecrementDiceRolling);
            }
        }

        rollsRemaining = 3;

        //THIS IS ALL UI STUFF
        UpdateGameInfoDisplays();
    }

    //On Event Trigger Functions
    //*************************************

    //Triggers when button is pressed
    public void TryRollDice() {
        if (dice != null) {
            if (diceRolling == 0 && rollsRemaining > 0) {
                foreach (ChefDie die in dice) {
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
    }

    private void UpdateGameInfoDisplays() {
        rollsLeftDisplay.UpdateDisplay(rollsRemaining);
    }

    //Called from scoresheet.entryFilled
    public void UpdateRound() {
        UnlockAllDice();
        rollsRemaining = 3;
        UpdateGameInfoDisplays();
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
                foreach (ChefDie die in dice) {
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
        rollsRemaining = 3;
        diceRolling = 0;
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
