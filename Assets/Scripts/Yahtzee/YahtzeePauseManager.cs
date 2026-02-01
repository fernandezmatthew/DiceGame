using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class YahtzeePauseMenuManager : MonoBehaviour {
    private List<YahtzeePauseMenuOption> menuOptions;
    private Animator introAnimator;
    private YahtzeeManager yahtzeeManager;

    private void OnEnable() {
        if (menuOptions != null) {
            //subscribe proper function to each option's onClick event
            foreach (var option in menuOptions) {
                switch (option.optionID) {
                    case YahtzeePauseMenuOption.EYahtzeePauseMenuOption.Resume:
                        option.GetComponent<MattUiButton>().onClick.AddListener(ResumeButtonClicked);
                        break;
                    case YahtzeePauseMenuOption.EYahtzeePauseMenuOption.Restart:
                        option.GetComponent<MattUiButton>().onClick.AddListener(RestartButtonClicked);
                        break;
                    case YahtzeePauseMenuOption.EYahtzeePauseMenuOption.Options:
                        option.GetComponent<MattUiButton>().onClick.AddListener(OptionsButtonClicked);
                        break;
                    case YahtzeePauseMenuOption.EYahtzeePauseMenuOption.MainMenu:
                        option.GetComponent<MattUiButton>().onClick.AddListener(MainMenuButtonClicked);
                        break;
                    case YahtzeePauseMenuOption.EYahtzeePauseMenuOption.Quit:
                        option.GetComponent<MattUiButton>().onClick.AddListener(QuitButtonClicked);
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
        menuOptions = FindObjectsByType<YahtzeePauseMenuOption>(FindObjectsSortMode.None).ToList();
        //subscribe proper function to each option's onClick event
        foreach (var option in menuOptions) {
            switch (option.optionID) {
                case YahtzeePauseMenuOption.EYahtzeePauseMenuOption.Resume:
                    option.GetComponent<MattUiButton>().onClick.AddListener(ResumeButtonClicked);
                    break;
                case YahtzeePauseMenuOption.EYahtzeePauseMenuOption.Restart:
                    option.GetComponent<MattUiButton>().onClick.AddListener(RestartButtonClicked);
                    break;
                case YahtzeePauseMenuOption.EYahtzeePauseMenuOption.Options:
                    option.GetComponent<MattUiButton>().onClick.AddListener(OptionsButtonClicked);
                    break;
                case YahtzeePauseMenuOption.EYahtzeePauseMenuOption.MainMenu:
                    option.GetComponent<MattUiButton>().onClick.AddListener(MainMenuButtonClicked);
                    break;
                case YahtzeePauseMenuOption.EYahtzeePauseMenuOption.Quit:
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
                switch (option.optionID) {
                    case YahtzeePauseMenuOption.EYahtzeePauseMenuOption.Resume:
                        option.GetComponent<MattUiButton>().onClick.RemoveListener(ResumeButtonClicked);
                        break;
                    case YahtzeePauseMenuOption.EYahtzeePauseMenuOption.Restart:
                        option.GetComponent<MattUiButton>().onClick.RemoveListener(RestartButtonClicked);
                        break;
                    case YahtzeePauseMenuOption.EYahtzeePauseMenuOption.Options:
                        option.GetComponent<MattUiButton>().onClick.RemoveListener(OptionsButtonClicked);
                        break;
                    case YahtzeePauseMenuOption.EYahtzeePauseMenuOption.MainMenu:
                        option.GetComponent<MattUiButton>().onClick.RemoveListener(MainMenuButtonClicked);
                        break;
                    case YahtzeePauseMenuOption.EYahtzeePauseMenuOption.Quit:
                        option.GetComponent<MattUiButton>().onClick.RemoveListener(QuitButtonClicked);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    private void ResumeButtonClicked() {
        yahtzeeManager.Pause();
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

    private void RestartButtonClicked() {
        yahtzeeManager.RestartFromPause();
    }

    private void OptionsButtonClicked() {
        Debug.Log("Options not implemented yet");
    }
    private void MainMenuButtonClicked() {
        yahtzeeManager.ReturnMainMenuFromPause();
    }

    private void QuitButtonClicked() {
        yahtzeeManager.ConfirmQuit();
    }
}
