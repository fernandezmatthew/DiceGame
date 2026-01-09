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

    private ScoreSheet scoreSheet;

    // This scoresheet only works for Yahtzee, which means 5d6. Cannot change number of dice, or sides on each die.
    private class ScoreSheet {
        public enum ScoreType { 
            Ones,
            Twos,
            Threes,
            Fours,
            Fives,
            Sixes,
            ThreeOfKind,
            FourOfKind,
            FullHouse,
            SStraight,
            LStraight,
            Yahtzee,
            Chance
        }

        public class ScoreEntry {
            public int score = 0;
            public int potentialScore = 0;
            public bool isFilled = false;
        }

        private ScoreEntry[] scores;
        private bool hasBonus;
        private int upperScore;
        private int totalScore;
        private int extraYahtzeeCount;

        public ScoreSheet() {
            scores = new ScoreEntry[13];
            for (int i = 0; i < 13; i++)
            {
                scores[i] = new ScoreEntry();
            }
        }

        public void FillEntry(ScoreType scoreType)
        {
            ScoreEntry targetEntry = scores[(int)scoreType];

            if (!targetEntry.isFilled) { //cannot fill an entry twice
                //Check if scoring a bonus yahtzee
                if (scores[(int)ScoreType.Yahtzee].isFilled && scores[(int)ScoreType.Yahtzee].potentialScore == 50)
                {
                    extraYahtzeeCount++;
                }

                targetEntry.score = targetEntry.potentialScore;
                targetEntry.isFilled = true;
                totalScore += targetEntry.score;
                if ((int)scoreType < 6)
                {
                    upperScore += targetEntry.score;
                }

                if (!hasBonus) {
                    if (upperScore >= 63) {
                        hasBonus = true;
                        totalScore += 35;
                    }
                }
            }
        }

        public void UpdatePotentialScores(in Die[] dice) {
            //Gather data necessary to update potential scores
            int[] faceSums = { 0, 0, 0, 0, 0, 0 };
            int[] faceQuantities = { 0, 0, 0, 0, 0, 0};
            int diceSum = 0;

            for (int i = 0; i < 5; i++)
            {
                int faceValue = dice[i].CurrentFace.value;
                faceSums[faceValue - 1] += faceValue;
                faceQuantities[faceValue - 1]++;
                diceSum += faceValue;
            }

            //Update each potential score
            //Start with the upper scores
            for (int i = 0; i < 6; i++)
            {
                scores[i].potentialScore = faceSums[i];
            }

            //chance is easy
            scores[(int)ScoreType.Chance].potentialScore = diceSum;

            //Check 3, 4, 5 of a kind. Update information for straights and full house
            bool hasFullHouse3 = false;
            bool hasFullHouse2 = false;
            int straightRunningTally = 0;
            int largestStraight = 0;

            foreach (int n in faceQuantities)
            {
                // Assume straightRunningTally will increment.
                bool breakStraight = false;
                if (n == 5)
                {
                    scores[(int)ScoreType.Yahtzee].potentialScore = 50;
                    scores[(int)ScoreType.FourOfKind].potentialScore = diceSum;
                    scores[(int)ScoreType.ThreeOfKind].potentialScore = diceSum;
                }
                else
                {
                    scores[(int)ScoreType.Yahtzee].potentialScore = 0;
                    if (n == 4)
                    {
                        scores[(int)ScoreType.FourOfKind].potentialScore = diceSum;
                        scores[(int)ScoreType.ThreeOfKind].potentialScore = diceSum;
                    }
                    else
                    {
                        scores[(int)ScoreType.FourOfKind].potentialScore = 0;
                        if (n == 3)
                        {
                            scores[(int)ScoreType.ThreeOfKind].potentialScore = diceSum;
                            hasFullHouse3 = true;
                        }
                        else
                        {
                            scores[(int)ScoreType.ThreeOfKind].potentialScore = 0;
                            if (n == 2)
                            {
                                hasFullHouse2 = true;
                            }
                            else
                            {
                                if (n == 0)
                                {
                                    breakStraight = true;
                                }
                            }
                        }
                    }
                }
                if (!breakStraight) {
                    straightRunningTally++;
                    if (straightRunningTally > largestStraight) {
                        largestStraight = straightRunningTally;
                    }
                }
                else {
                    straightRunningTally = 0;
                }

                //Use information to fill out straights and full house
                if (largestStraight == 5)
                {
                    scores[(int)ScoreType.LStraight].potentialScore = 40;
                    scores[(int)ScoreType.SStraight].potentialScore = 30;
                }
                else
                {
                    scores[(int)ScoreType.LStraight].potentialScore = 0;
                    if (largestStraight == 4)
                    {
                        scores[(int)ScoreType.SStraight].potentialScore = 30;
                    }
                    else
                    {
                        scores[(int)ScoreType.SStraight].potentialScore = 0;
                    }
                }

                if (hasFullHouse2 && hasFullHouse3)
                {
                    scores[(int)ScoreType.FullHouse].potentialScore = 25;
                }
                else
                {
                    scores[(int)ScoreType.FullHouse].potentialScore = 0;
                }
            }
        }

        public void PrintPotentialScores() {
            string s = "POTENTIAL SCORES\n****************\n\n" + 
                       "Ones: " + scores[0].potentialScore + "\n" +
                       "Twos: " + scores[1].potentialScore + "\n" +
                       "Threes: " + scores[2].potentialScore + "\n" +
                       "Fours: " + scores[3].potentialScore + "\n" +
                       "Fives: " + scores[4].potentialScore + "\n" +
                       "Sixes: " + scores[5].potentialScore + "\n" +
                       "3 of a Kind: " + scores[6].potentialScore + "\n" +
                       "4 of a Kind: " + scores[7].potentialScore + "\n" +
                       "Full House: " + scores[8].potentialScore + "\n" +
                       "Sm. Straight: " + scores[9].potentialScore + "\n" +
                       "Lg. Straight: " + scores[10].potentialScore + "\n" +
                       "Yahtzee: " + scores[11].potentialScore + "\n" +
                       "Chance: " + scores[12].potentialScore;

            Debug.Log(s);
        }

        public ScoreEntry[] Scores { get { return scores; } }
        public int TotalScore { get { return totalScore; } }
    }

    void OnEnable() {
        //Subscribe to each die's "finishedRolling" event
        //This won't happen on start up cuz dice will == null. So we will do it in start(). But if we re-enable, we want the events back.
        if (dice != null)
        {
            foreach (Die die in dice)
            {
                die.finishedRolling.AddListener(DecrementDiceRolling);
            }
        }
    }

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

    private void Start()
    {
        //Find scene objects
        dice = FindObjectsOfType<Die>();
        cam = FindObjectOfType<Camera>();

        //Subscribe to each die's "finishedRolling" event
        if (dice != null)
        {
            foreach (Die die in dice)
            {
                die.finishedRolling.AddListener(DecrementDiceRolling);
            }
        }

        //Make scoresheet
        scoreSheet = new ScoreSheet();
    }

    private void Update()
    {

    }

    private void UpdateScoreSheet() {
        scoreSheet.UpdatePotentialScores(dice);
        scoreSheet.PrintPotentialScores();
    }


    //On Event Trigger Functions

    public void RollDice() {
        if (dice != null) {
            if (diceRolling == 0)
            {
                foreach (Die die in dice)
                {
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
                sum += die.CurrentFace.value;
            }
            newSum.Invoke(sum.ToString());
        }
        UpdateScoreSheet();
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
