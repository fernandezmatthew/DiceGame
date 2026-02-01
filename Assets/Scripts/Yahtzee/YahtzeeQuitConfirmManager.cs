using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class YahtzeeQuitConfirmManager : MonoBehaviour {
    private List<YahtzeeQuitConfirmOption> menuOptions;
    private Animator introAnimator;
    private YahtzeeGeneralManager yahtzeeGeneralManager;

    private void OnEnable() {
        if (menuOptions != null) {
            //subscribe proper function to each option's onClick event
            foreach (var option in menuOptions) {
                switch (option.optionID) {
                    case YahtzeeQuitConfirmOption.EYahtzeeQuitConfirmOption.Yes:
                        option.GetComponent<MattUiButton>().onClick.AddListener(YesButtonClicked);
                        break;
                    case YahtzeeQuitConfirmOption.EYahtzeeQuitConfirmOption.No:
                        option.GetComponent<MattUiButton>().onClick.AddListener(NoButtonClicked);
                        break;
                    default:
                        break;
                }
            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        yahtzeeGeneralManager = FindFirstObjectByType<YahtzeeGeneralManager>();
        //Open up scene with animation.
        //first, find the animator
        //introAnimator = FindFirstObjectByType<Animator>();
        //then, trigger open
        //introAnimator.SetTrigger("Open");
        //disable player input while this is happening.
        //StartCoroutine(DisableInputWhileAnimating());

        //get all main menu options
        menuOptions = FindObjectsByType<YahtzeeQuitConfirmOption>(FindObjectsSortMode.None).ToList();
        //subscribe proper function to each option's onClick event
        foreach (var option in menuOptions) {
            switch (option.optionID) {
                case YahtzeeQuitConfirmOption.EYahtzeeQuitConfirmOption.Yes:
                    option.GetComponent<MattUiButton>().onClick.AddListener(YesButtonClicked);
                    break;
                case YahtzeeQuitConfirmOption.EYahtzeeQuitConfirmOption.No:
                    option.GetComponent<MattUiButton>().onClick.AddListener(NoButtonClicked);
                    break;
                default:
                    break;
            }
        }
    }

    private void OnDisable() {
        if (menuOptions != null) {
            //subscribe proper function to each option's onClick event
            foreach (var option in menuOptions) {
                switch (option.optionID) {
                    case YahtzeeQuitConfirmOption.EYahtzeeQuitConfirmOption.Yes:
                        option.GetComponent<MattUiButton>().onClick.RemoveListener(YesButtonClicked);
                        break;
                    case YahtzeeQuitConfirmOption.EYahtzeeQuitConfirmOption.No:
                        option.GetComponent<MattUiButton>().onClick.RemoveListener(NoButtonClicked);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void YesButtonClicked() {
        yahtzeeGeneralManager.Quit();
    }

    private void NoButtonClicked() {
        yahtzeeGeneralManager.QuitCancelled();
    }

    //private IEnumerator DisableInputWhileAnimating() {
    //    InputManager.DisableInput();
    //    yield return null; // wait one frame for animation info to update
    //    //wait for transition
    //    while (transitionAnimator.IsInTransition(0)) {
    //        yield return null;
    //    }
    //    //wait for animation to finish
    //    while (transitionAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) {
    //        yield return null;
    //    }

    //    InputManager.EnableInput();
    //}
}
