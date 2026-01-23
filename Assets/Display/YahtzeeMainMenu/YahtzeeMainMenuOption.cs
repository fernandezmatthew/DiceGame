using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YahtzeeMainMenuOption : MonoBehaviour
{
    public enum EYahtzeeMainMenuOption { 
        Start,
        PastScores,
        Quit
    }
    public EYahtzeeMainMenuOption optionID;
}
