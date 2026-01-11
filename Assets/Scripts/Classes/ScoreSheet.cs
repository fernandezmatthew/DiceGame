using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScoreSheet
{
    public abstract class ScoreEntry {
        public int score = 0;
        public string displayName = "[no name]";
    }
    protected ScoreEntry[] entries;
    protected int totalScore;

    public ScoreEntry[] Entries { get { return entries; } }

    public abstract void FillEntry(int scoreType); //May want to take in an entry object instead of int
    public abstract void UpdateScoreSheet(in Die[] dice);

    public ScoreEntry GetScoreEntry(int scoreType) {
        return entries[scoreType];
    }
}
