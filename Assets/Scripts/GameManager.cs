using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    Die[] dice;

    int diceRolling = 0;

    public UnityEvent diceRollFinished;

    private void Start()
    {
        dice = FindObjectsOfType<Die>();
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
                    diceRolling++;
                    float rollLength = UnityEngine.Random.Range(.8f, 1.2f);
                    die.Roll(rollLength);
                }
            }
        }
    }

    public void DecrementDiceRolling() {
        diceRolling -= 1;
        if (diceRolling == 0) {
            diceRollFinished.Invoke();
            PrintSumOfDice();
        }
    }

    public void PrintSumOfDice() { 
        if (dice != null)
        {
            int sum = 0;
            foreach (Die die in dice) {
                sum += die.scriptableDie.faces[die.currentFaceIndex].value;
            }
            Debug.Log("Sum of Dice Equal to " + sum);
        }
    }
}
