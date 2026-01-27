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

    private DishDisplay[] dishDisplays;

    //UI STUFF
    public Canvas canvas;
    public GameInfoDisplay rollsLeftDisplay;

    private void Start() {
        //Find scene objects
        dice = FindObjectsByType<ChefDie>(FindObjectsSortMode.None);
        cam = FindFirstObjectByType<Camera>();
        rollButton = FindFirstObjectByType<Button>();
        dishDisplays = FindObjectsByType<DishDisplay>(FindObjectsSortMode.None);
        foreach (var dish in dishDisplays) {
            dish.dishClicked.AddListener(AttemptScoreDish);
        }

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

    //called from dishDisplay.dishClicked
    private void AttemptScoreDish(Dish dish) {
        //Make sure dice not rolling
        if (diceRolling == 0) {
            //Make sure player has made at least one roll
            if (rollsRemaining < 3) {
                // check to see if proper ingredients are satisfied
                // start by making a dictionary using the given recipe list
                Dictionary<Ingredient, int> recipeDict = new Dictionary<Ingredient, int>();
                foreach (var ingredient in dish.recipe) {
                    if (!recipeDict.ContainsKey(ingredient)) {
                        recipeDict.Add(ingredient, 1);
                    }
                    else {
                        recipeDict[ingredient]++;
                    }
                }

                foreach (var die in dice) {
                    if (recipeDict.ContainsKey(die.CurrentFace.ingredient)) {
                        if (recipeDict[die.CurrentFace.ingredient] > 1) {
                            recipeDict[die.CurrentFace.ingredient]--;
                        }
                        else {
                            recipeDict.Remove(die.CurrentFace.ingredient);
                        }
                    }
                }

                //if dictionary is empty, then the condition was satisfied
                if (recipeDict.Count == 0) {
                    //Condition satisfied, find the score
                    int dishScore = dish.Score(dice);
                    Debug.Log("You scored " + dishScore + " points by cooking a " + dish.DishName + "!");
                    UpdateRound();
                }
            }
        }
        //otherwise, we do nothing.
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
