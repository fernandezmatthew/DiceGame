using UnityEngine;
using UnityEngine.InputSystem;

public abstract class YahtzeeGeneralManager : MonoBehaviour
{
    public GameObject quitConfirmUi;

    public virtual void ConfirmQuit() {
        quitConfirmUi.SetActive(true);
    }

    public virtual void QuitCancelled() {
        quitConfirmUi.SetActive(false);
    }

    public void Quit(InputAction.CallbackContext ctx) {
        Quit();
    }

    public void Quit() {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
