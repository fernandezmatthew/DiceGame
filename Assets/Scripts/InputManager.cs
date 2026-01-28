using UnityEngine;
using UnityEngine.InputSystem.UI;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    private PlayerInputActions playerInputActions;
    private InputSystemUIInputModule uiInputModule;

    public static PlayerInputActions PlayerInputActions { get { return instance.playerInputActions; } }
    public static InputSystemUIInputModule UiInputModule { get { return instance.uiInputModule; }  }


    private void Awake() {
        if (instance == null) {
            instance = this;

            playerInputActions = new PlayerInputActions();
            playerInputActions.Global.Enable();
            uiInputModule = GetComponent<InputSystemUIInputModule>();
            DontDestroyOnLoad(gameObject);
        }
    }

    public static bool IsInitiated() { 
        return instance != null;
    }

    public static void EnableInput() {
        instance.playerInputActions.Yahtzee.Enable();
        UiInputModule.enabled = true;
    }

    public static void DisableInput() {
        instance.playerInputActions.Yahtzee.Disable();
        UiInputModule.enabled = false;
    }

    public static void EnablePlayerInput() {
        instance.playerInputActions.Yahtzee.Enable();
    }

    public static void DisablePlayerInput() {
        instance.playerInputActions.Yahtzee.Disable();
    }

    public static void EnableUiInput() {
        UiInputModule.enabled = true;
    }

    public static void DisableUiInput() {
        UiInputModule.enabled = false;
    }
}
