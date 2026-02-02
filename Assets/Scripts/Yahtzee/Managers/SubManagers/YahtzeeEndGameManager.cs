using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class YahtzeeEndGameManager : MonoBehaviour {
    private List<YahtzeeEndGameOption> menuOptions;
    private Animator introAnimator;
    private YahtzeeManager yahtzeeManager;

    private void OnEnable() {
        if (menuOptions != null) {
            //subscribe proper function to each option's onClick event
            foreach (var option in menuOptions) {
                switch (option.optionID) {
                    case YahtzeeEndGameOption.EYahtzeeEndGameOption.PlayAgain:
                        option.GetComponent<MattUiButton>().onClick.AddListener(PlayAgainButtonClicked);
                        break;
                    case YahtzeeEndGameOption.EYahtzeeEndGameOption.MainMenu:
                        option.GetComponent<MattUiButton>().onClick.AddListener(MainMenuButtonClicked);
                        break;
                    default:
                        break;
                }
            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        yahtzeeManager = FindFirstObjectByType<YahtzeeManager>();
        //Open up scene with animation.
        //first, find the animator
        //introAnimator = FindFirstObjectByType<Animator>();
        //then, trigger open
        //introAnimator.SetTrigger("Open");
        //disable player input while this is happening.
        //StartCoroutine(DisableInputWhileAnimating());

        //get all main menu options
        menuOptions = FindObjectsByType<YahtzeeEndGameOption>(FindObjectsSortMode.None).ToList();
        //subscribe proper function to each option's onClick event
        //subscribe proper function to each option's onClick event
        foreach (var option in menuOptions) {
            switch (option.optionID) {
                case YahtzeeEndGameOption.EYahtzeeEndGameOption.PlayAgain:
                    option.GetComponent<MattUiButton>().onClick.AddListener(PlayAgainButtonClicked);
                    break;
                case YahtzeeEndGameOption.EYahtzeeEndGameOption.MainMenu:
                    option.GetComponent<MattUiButton>().onClick.AddListener(MainMenuButtonClicked);
                    break;
                default:
                    break;
            }
        }
    }

    private void OnDisable() {
        if (menuOptions != null) {
            //subscribe proper function to each option's onClick event
            //subscribe proper function to each option's onClick event
            foreach (var option in menuOptions) {
                switch (option.optionID) {
                    case YahtzeeEndGameOption.EYahtzeeEndGameOption.PlayAgain:
                        option.GetComponent<MattUiButton>().onClick.RemoveListener(PlayAgainButtonClicked);
                        break;
                    case YahtzeeEndGameOption.EYahtzeeEndGameOption.MainMenu:
                        option.GetComponent<MattUiButton>().onClick.RemoveListener(MainMenuButtonClicked);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void PlayAgainButtonClicked() {
        yahtzeeManager.Restart();
    }

    private void MainMenuButtonClicked() {
        yahtzeeManager.ReturnMainMenu();
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
