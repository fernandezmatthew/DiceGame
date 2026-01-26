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
            uiInputModule = GetComponent<InputSystemUIInputModule>();
            DontDestroyOnLoad(gameObject);
        }
    }

    public static void EnableInput() {
        instance.playerInputActions.Enable();
        UiInputModule.enabled = true;
    }

    public static void DisableInput() {
        instance.playerInputActions.Disable();
        UiInputModule.enabled = false;
    }

    public static void EnablePlayerInput() {
        instance.playerInputActions.Enable();
    }

    public static void DisablePlayerInput() {
        instance.playerInputActions.Disable();
    }

    public static void EnableUiInput() {
        UiInputModule.enabled = true;
    }

    public static void DisableUiInput() {
        UiInputModule.enabled = false;
    }
}
