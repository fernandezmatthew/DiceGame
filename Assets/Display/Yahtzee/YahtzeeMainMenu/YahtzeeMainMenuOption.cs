using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YahtzeeMainMenuOption : MonoBehaviour
{
    public enum EYahtzeeMainMenuOption { 
        Start,
        PastScores,
        DiceSelection,
        Quit
    }
    public EYahtzeeMainMenuOption optionID;
}
