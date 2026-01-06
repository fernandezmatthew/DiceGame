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
    public UnityEvent<string> newSum;
    public UnityEvent toggleLockDie;

    private void Start()
    {
        dice = FindObjectsOfType<Die>();
        cam = FindObjectOfType<Camera>();
        if (dice != null)
        {
            foreach (Die die in dice)
            {
                die.finishedRolling.AddListener(DecrementDiceRolling);
            }
        }
    }

    void OnEnable() {}

    private void OnDisable()
    {
        if (dice != null)
        {
            foreach (Die die in dice)
            {
                die.finishedRolling.RemoveListener(DecrementDiceRolling);
            }
        }
    }

    public void RollDice() {
        if (dice != null) {
            if (diceRolling == 0)
            {
                foreach (Die die in dice)
                {
                    if (!die.isLocked) {
                        diceRolling++;
                        float rollLength = UnityEngine.Random.Range(.8f, 1.2f);
                        die.Roll(rollLength);
                    }
                }
                // if diceRolling > 0, increment number of rolls
            }
        }
    }

    public void DecrementDiceRolling() {
        diceRolling -= 1;
        if (diceRolling == 0) {
            diceRollFinished.Invoke();
            UpdateSumOfDice();
        }
    }

    public void UpdateSumOfDice() { 
        if (dice != null)
        {
            int sum = 0;
            foreach (Die die in dice) {
                sum += die.scriptableDie.faces[die.currentFaceIndex].value;
            }
            newSum.Invoke(sum.ToString());
        }
    }

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
                        }
                    }
                }
            }
        }
    }
}
