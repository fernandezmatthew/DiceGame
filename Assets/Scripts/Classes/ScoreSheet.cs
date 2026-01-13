using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Scoresheet;
using UnityEngine.Events;

public class Scoresheet {

    public UnityEvent<ScoreEntry> entryClicked;
    public UnityEvent entryFilled;
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
        public string displayName = "[no name]";
        public int potentialScore = 0;
        public bool isFilled = false;
    }
    public class DetailEntry {
        public int score = 0;
        public string displayName = "[no name]";
    }
    protected ScoreEntry[] scoreEntries;
    protected DetailEntry[] detailEntries;
    protected int totalScore;
    private bool hasBonus;
    private int upperScore;
    private int lowerScore;
    private int extraYahtzeeCount;

    public ScoreEntry GetScoreEntry(int scoreType) {
        return scoreEntries[scoreType];
    }

    public DetailEntry GetDetailEntry(int detailType) { 
        return detailEntries[detailType];
    }

    public Scoresheet() {
        entryFilled = new UnityEvent();
        entryClicked = new UnityEvent<ScoreEntry>();

        scoreEntries = new ScoreEntry[13];
        string[] yahtzeeScoreDisplayNames = {
            "Ones",
            "Twos",
            "Threes",
            "Fours",
            "Fives",
            "Sixes",
            "3 of a Kind",
            "4 of a Kind",
            "Full House",
            "Sm. Straight",
            "Lg. Straight",
            "Yahtzee",
            "Chance"
        };

        for (int i = 0; i < 13; i++) {
            scoreEntries[i] = new ScoreEntry();
            scoreEntries[i].displayName = yahtzeeScoreDisplayNames[i];
        }

        detailEntries = new DetailEntry[5];
        string[] DetailsDisplayNames = {
            "Upper Total",
            "Upper Bonus",
            "Lower Total",
            "Yahtzee Bonus",
            "Total Score"
        };

        for (int i = 0; i < detailEntries.Length; i++) {
            detailEntries[i] = new DetailEntry();
            detailEntries[i].displayName = DetailsDisplayNames[i];
        }

        hasBonus = false;
        upperScore = 0;
        lowerScore = 0;
        totalScore = 0;
        extraYahtzeeCount = 0;
    }

    public void UpdateDetails() {
        detailEntries[0].score = upperScore;
        detailEntries[1].score = hasBonus ? 35 : 0;
        detailEntries[2].score = lowerScore;
        detailEntries[3].score = extraYahtzeeCount * 100;
        detailEntries[4].score = totalScore;
    }

    public void UpdatePotentials(in Die[] dice) {
        //Gather data necessary to update potential scores
        int[] faceSums = { 0, 0, 0, 0, 0, 0 };
        int[] faceQuantities = { 0, 0, 0, 0, 0, 0 };
        int diceSum = 0;

        for (int i = 0; i < 5; i++) {
            int faceValue = dice[i].CurrentFace.value;
            faceSums[faceValue - 1] += faceValue;
            faceQuantities[faceValue - 1]++;
            diceSum += faceValue;
        }

        //Update each potential score
        //Start with the upper scores
        for (int i = 0; i < 6; i++) {
            scoreEntries[i].potentialScore = faceSums[i];
        }

        //chance is easy
        scoreEntries[(int)ScoreType.Chance].potentialScore = diceSum;

        //Update information for 3, 4, 5 of a kind, straights, and full house
        bool has3ofKind = false;
        bool has4ofKind = false;
        bool hasYahtzee = false;
        bool hasFullHouse3 = false;
        bool hasFullHouse2 = false;
        int straightRunningTally = 0;
        int largestStraight = 0;

        foreach (int n in faceQuantities) {
            // Assume straightRunningTally will increment.
            bool breakStraight = false;
            if (n == 5) {
                hasYahtzee = true;
                has4ofKind = true;
                has3ofKind = true;
            }
            else if (n == 4) {
                has4ofKind = true;
                has3ofKind = true;
            }
            else if (n == 3) {
                has3ofKind = true;
                hasFullHouse3 = true;
            }
            else if (n == 2) {
                hasFullHouse2 = true;
            }
            else if (n == 0) {
                breakStraight = true;
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
        }
        //Use information to fill out potential scores
        scoreEntries[(int)ScoreType.Yahtzee].potentialScore = hasYahtzee ? 50 : 0;
        scoreEntries[(int)ScoreType.FourOfKind].potentialScore = has4ofKind ? diceSum : 0;
        scoreEntries[(int)ScoreType.ThreeOfKind].potentialScore = has3ofKind ? diceSum : 0;
        scoreEntries[(int)ScoreType.FullHouse].potentialScore = hasFullHouse2 && hasFullHouse3 ? 25 : 0;
        scoreEntries[(int)ScoreType.LStraight].potentialScore = largestStraight >= 5 ? 40 : 0;
        scoreEntries[(int)ScoreType.SStraight].potentialScore = largestStraight >= 4 ? 30 : 0;
    }

    private void ResetPotentials() { 
        foreach (var entry in scoreEntries) {
            entry.potentialScore = 0;
        }
    }
    //Called from scoresheetDisplay.entryClicked
    public void EntryClicked(ScoreEntry scoreEntry) {
        entryClicked.Invoke(scoreEntry);
    }

    //called from GameManager if entry is clicked when its allowed to be filled
    public void FillEntry(ScoreEntry scoreEntry) {
        ScoreEntry targetEntry = null;
        int targetEntryType = -1;

        for (int i = 0; i < scoreEntries.Length; i++) {
            if (scoreEntry == scoreEntries[i]) {
                targetEntry = scoreEntries[i];
                targetEntryType = i;
                break;
            }
            else {
                targetEntry = null;
            }
        }

        if (targetEntry != null) {
            if (!targetEntry.isFilled) { //cannot fill an entry twice

                targetEntry.score = targetEntry.potentialScore;
                targetEntry.isFilled = true;
                if ((int)targetEntryType < 6) {
                    upperScore += targetEntry.score;
                    if (!hasBonus) {
                        if (upperScore >= 63) {
                            hasBonus = true;
                            totalScore += 35;
                        }
                    }
                }
                else {
                    lowerScore += targetEntry.score;
                }

                totalScore += targetEntry.score;

                //Check if also scored a bonus yahtzee
                if (targetEntryType != (int)ScoreType.Yahtzee &&
                    scoreEntries[(int)ScoreType.Yahtzee].score == 50 &&
                    scoreEntries[(int)ScoreType.Yahtzee].potentialScore == 50) {
                    extraYahtzeeCount++;
                    totalScore += 100;
                }
                UpdateDetails();
                ResetPotentials();
                entryFilled.Invoke();
            }
        }
    }

    //Debugging function
    public void PrintPotentialScores() {
        string s = "POTENTIAL SCORES\n****************\n\n" +
                   "Ones: " + scoreEntries[0].potentialScore + "\n" +
                   "Twos: " + scoreEntries[1].potentialScore + "\n" +
                   "Threes: " + scoreEntries[2].potentialScore + "\n" +
                   "Fours: " + scoreEntries[3].potentialScore + "\n" +
                   "Fives: " + scoreEntries[4].potentialScore + "\n" +
                   "Sixes: " + scoreEntries[5].potentialScore + "\n" +
                   "3 of a Kind: " + scoreEntries[6].potentialScore + "\n" +
                   "4 of a Kind: " + scoreEntries[7].potentialScore + "\n" +
                   "Full House: " + scoreEntries[8].potentialScore + "\n" +
                   "Sm. Straight: " + scoreEntries[9].potentialScore + "\n" +
                   "Lg. Straight: " + scoreEntries[10].potentialScore + "\n" +
                   "Yahtzee: " + scoreEntries[11].potentialScore + "\n" +
                   "Chance: " + scoreEntries[12].potentialScore;

        Debug.Log(s);
    }
}
