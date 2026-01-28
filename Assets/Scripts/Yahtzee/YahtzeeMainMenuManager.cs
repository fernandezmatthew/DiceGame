using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class YahtzeeMainMenuManager : MonoBehaviour
{
    private List<YahtzeeMainMenuOption> menuOptions;
    private Animator transitionAnimator;

    private void OnEnable() {
        if (menuOptions != null) {
            //subscribe proper function to each option's onClick event
            foreach (var option in menuOptions) {
                if (option != null) {
                    switch (option.optionID) {
                        case YahtzeeMainMenuOption.EYahtzeeMainMenuOption.Start:
                            option.GetComponent<MattUiButton>().onClick.AddListener(StartButtonClicked);
                            break;
                        case YahtzeeMainMenuOption.EYahtzeeMainMenuOption.PastScores:
                            option.GetComponent<MattUiButton>().onClick.AddListener(ScoresButtonClicked);
                            break;
                        case YahtzeeMainMenuOption.EYahtzeeMainMenuOption.DiceSelection:
                            option.GetComponent<MattUiButton>().onClick.AddListener(DiceButtonClicked);
                            break;
                        case YahtzeeMainMenuOption.EYahtzeeMainMenuOption.Quit:
                            option.GetComponent<MattUiButton>().onClick.AddListener(QuitButtonClicked);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        //Open up scene with animation.
        //first, find the animator
        transitionAnimator = FindFirstObjectByType<Animator>();
        //then, trigger open
        transitionAnimator.SetTrigger("Open");
        //disable player input while this is happening.
        StartCoroutine(DisableInputWhileAnimating());

        //get all main menu options
        menuOptions = FindObjectsByType<YahtzeeMainMenuOption>(FindObjectsSortMode.None).ToList();
        //subscribe proper function to each option's onClick event
        foreach (var option in menuOptions) {
            switch (option.optionID) {
                case YahtzeeMainMenuOption.EYahtzeeMainMenuOption.Start:
                    option.GetComponent<MattUiButton>().onClick.AddListener(StartButtonClicked);
                    break;
                case YahtzeeMainMenuOption.EYahtzeeMainMenuOption.PastScores:
                    option.GetComponent<MattUiButton>().onClick.AddListener(ScoresButtonClicked);
                    break;
                case YahtzeeMainMenuOption.EYahtzeeMainMenuOption.DiceSelection:
                    option.GetComponent<MattUiButton>().onClick.AddListener(DiceButtonClicked);
                    break;
                case YahtzeeMainMenuOption.EYahtzeeMainMenuOption.Quit:
                    option.GetComponent<MattUiButton>().onClick.AddListener(QuitButtonClicked);
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
                if (option != null) {
                    switch (option.optionID) {
                        case YahtzeeMainMenuOption.EYahtzeeMainMenuOption.Start:
                            option.GetComponent<MattUiButton>().onClick.RemoveListener(StartButtonClicked);
                            break;
                        case YahtzeeMainMenuOption.EYahtzeeMainMenuOption.PastScores:
                            option.GetComponent<MattUiButton>().onClick.RemoveListener(ScoresButtonClicked);
                            break;
                        case YahtzeeMainMenuOption.EYahtzeeMainMenuOption.DiceSelection:
                            option.GetComponent<MattUiButton>().onClick.RemoveListener(DiceButtonClicked);
                            break;
                        case YahtzeeMainMenuOption.EYahtzeeMainMenuOption.Quit:
                            option.GetComponent<MattUiButton>().onClick.RemoveListener(QuitButtonClicked);
                            break;
                        default:
                            break;
                    }
                }
            }
        }
    }

    private void StartButtonClicked() {
        transitionAnimator.SetTrigger("Close");
        StartCoroutine(LoadSceneAfterAnimation("Scenes/YahtzeeClone"));
    }

    private IEnumerator DisableInputWhileAnimating() {
        InputManager.DisableInput();
        yield return null; // wait one frame for animation info to update
        //wait for transition
        while (transitionAnimator.IsInTransition(0)) {
            yield return null;
        }
        //wait for animation to finish
        while (transitionAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) {
            yield return null;
        }

        InputManager.EnableInput();
    }

    IEnumerator LoadSceneAfterAnimation(string scene) {
        yield return null;
        //wait for transition
        while (transitionAnimator.IsInTransition(0)) {
            yield return null;
        }
        //wait for animation to finish
        while (transitionAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) {
            yield return null;
        }
        SceneManager.LoadScene(scene);
    }

    private void ScoresButtonClicked() {
        Debug.Log("Past Scores not implemented yet");
    }

    private void DiceButtonClicked() {
        Debug.Log("Dice Selection not implemented yet");
    }
    private void QuitButtonClicked() {
    #if UNITY_STANDALONE
        Application.Quit();
    #endif
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }
}
