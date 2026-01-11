using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YahtzeeScoreSheet : ScoreSheet {
    public enum YahtzeeScoreType {
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

    public class YahtzeeScoreEntry : ScoreEntry {
        public int potentialScore = 0;
        public bool isFilled = false;
    }

    private bool hasBonus;
    private int upperScore;
    private int extraYahtzeeCount;

    public YahtzeeScoreSheet() {
        entries = new YahtzeeScoreEntry[13];
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
            entries[i] = new YahtzeeScoreEntry();
            entries[i].displayName = yahtzeeScoreDisplayNames[i];
        }

        hasBonus = false;
        upperScore = 0;
        totalScore = 0;
        extraYahtzeeCount = 0;
    }

    public override void FillEntry(int scoreType) {
        YahtzeeScoreEntry targetEntry = (YahtzeeScoreEntry)entries[scoreType];

        if (!targetEntry.isFilled) { //cannot fill an entry twice

            targetEntry.score = targetEntry.potentialScore;
            targetEntry.isFilled = true;
            if ((int)scoreType < 6) {
                upperScore += targetEntry.score;
            }
            if (!hasBonus) {
                if (upperScore >= 63) {
                    hasBonus = true;
                    totalScore += 35;
                }
            }
            totalScore += targetEntry.score;

            //Check if also scored a bonus yahtzee
            if (((YahtzeeScoreEntry)entries[(int)YahtzeeScoreType.Yahtzee]).isFilled && ((YahtzeeScoreEntry)entries[(int)YahtzeeScoreType.Yahtzee]).potentialScore == 50) {
                extraYahtzeeCount++;
                totalScore += 100;
            }
        }
    }

    public override void UpdateScoreSheet(in Die[] dice) {
        UpdatePotentialScores(dice);
        PrintPotentialScores();
    }

    public void UpdatePotentialScores(in Die[] dice) {
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
            ((YahtzeeScoreEntry)entries[i]).potentialScore = faceSums[i];
        }

        //chance is easy
        ((YahtzeeScoreEntry)entries[(int)YahtzeeScoreType.Chance]).potentialScore = diceSum;

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
        ((YahtzeeScoreEntry)entries[(int)YahtzeeScoreType.Yahtzee]).potentialScore = hasYahtzee ? 50 : 0;
        ((YahtzeeScoreEntry)entries[(int)YahtzeeScoreType.FourOfKind]).potentialScore = has4ofKind ? diceSum : 0;
        ((YahtzeeScoreEntry)entries[(int)YahtzeeScoreType.ThreeOfKind]).potentialScore = has3ofKind ? diceSum : 0;
        ((YahtzeeScoreEntry)entries[(int)YahtzeeScoreType.FullHouse]).potentialScore = hasFullHouse2 && hasFullHouse3 ? 25 : 0;
        ((YahtzeeScoreEntry)entries[(int)YahtzeeScoreType.LStraight]).potentialScore = largestStraight >= 5 ? 40 : 0;
        ((YahtzeeScoreEntry)entries[(int)YahtzeeScoreType.SStraight]).potentialScore = largestStraight >= 4 ? 30 : 0;
    }

    public void PrintPotentialScores() {
        string s = "POTENTIAL SCORES\n****************\n\n" +
                   "Ones: " + ((YahtzeeScoreEntry)entries[0]).potentialScore + "\n" +
                   "Twos: " + ((YahtzeeScoreEntry)entries[1]).potentialScore + "\n" +
                   "Threes: " + ((YahtzeeScoreEntry)entries[2]).potentialScore + "\n" +
                   "Fours: " + ((YahtzeeScoreEntry)entries[3]).potentialScore + "\n" +
                   "Fives: " + ((YahtzeeScoreEntry)entries[4]).potentialScore + "\n" +
                   "Sixes: " + ((YahtzeeScoreEntry)entries[5]).potentialScore + "\n" +
                   "3 of a Kind: " + ((YahtzeeScoreEntry)entries[6]).potentialScore + "\n" +
                   "4 of a Kind: " + ((YahtzeeScoreEntry)entries[7]).potentialScore + "\n" +
                   "Full House: " + ((YahtzeeScoreEntry)entries[8]).potentialScore + "\n" +
                   "Sm. Straight: " + ((YahtzeeScoreEntry)entries[9]).potentialScore + "\n" +
                   "Lg. Straight: " + ((YahtzeeScoreEntry)entries[10]).potentialScore + "\n" +
                   "Yahtzee: " + ((YahtzeeScoreEntry)entries[11]).potentialScore + "\n" +
                   "Chance: " + ((YahtzeeScoreEntry)entries[12]).potentialScore;

        Debug.Log(s);
    }
}
