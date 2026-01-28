using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YahtzeePauseMenuOption : MonoBehaviour
{
    public enum EYahtzeePauseMenuOption { 
        Resume,
        Restart,
        Options,
        MainMenu
    }
    public EYahtzeePauseMenuOption optionID;
}
