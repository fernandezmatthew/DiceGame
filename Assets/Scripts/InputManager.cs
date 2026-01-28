using UnityEngine;
using UnityEngine.InputSystem.UI;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    private static InputManager instance;
    private PlayerInputActions playerInputActions;
    private InputSystemUIInputModule uiInputModule;
    private EventSystem eventSystem;

    public static PlayerInputActions PlayerInputActions { get { return instance.playerInputActions; } }
    public static InputSystemUIInputModule UiInputModule { get { return instance.uiInputModule; }  }


    private void Awake() {
        if (instance == null) {
            instance = this;

            playerInputActions = new PlayerInputActions();
            playerInputActions.Global.Enable();
            uiInputModule = GetComponent<InputSystemUIInputModule>();
            uiInputModule.enabled = true;
            eventSystem = GetComponent<EventSystem>();
            eventSystem.enabled = true;
            DontDestroyOnLoad(gameObject);
        }
        else {
            //We don't want there to be two input manager objects, which includes event system
            //and ui input module, so destroy this object
            Destroy(this.gameObject);
        }
    }

    public static bool IsInitiated() { 
        return instance != null;
    }

    public static void EnableInput() {
        instance.playerInputActions.Yahtzee.Enable();
        instance.uiInputModule.enabled = true;
    }

    public static void DisableInput() {
        instance.playerInputActions.Yahtzee.Disable();
        instance.uiInputModule.enabled = false;
    }

    public static void EnableGlobalInput() { 
        instance.playerInputActions.Global.Enable();
    }

    public static void DisableGlobalInput() {
        instance.playerInputActions.Global.Disable();
    }

    public static void EnablePlayerInput() {
        instance.playerInputActions.Yahtzee.Enable();
    }

    public static void DisablePlayerInput() {
        instance.playerInputActions.Yahtzee.Disable();
    }

    public static void EnableUiInput() {
        instance.uiInputModule.enabled = true;
    }

    public static void DisableUiInput() {
        instance.uiInputModule.enabled = false;
    }
}
