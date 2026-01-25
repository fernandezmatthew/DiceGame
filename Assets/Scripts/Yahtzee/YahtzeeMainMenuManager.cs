using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class YahtzeeMainMenuManager : MonoBehaviour
{
    private List<YahtzeeMainMenuOption> menuOptions;
    private Animator crossFadeAnimator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        crossFadeAnimator = FindFirstObjectByType<Animator>();
        menuOptions = FindObjectsByType<YahtzeeMainMenuOption>(FindObjectsSortMode.None).ToList();
        foreach (var option in menuOptions) {
            switch (option.optionID) {
                case YahtzeeMainMenuOption.EYahtzeeMainMenuOption.Start:
                    option.GetComponent<Button>().onClick.AddListener(StartButtonClicked);
                    break;
                case YahtzeeMainMenuOption.EYahtzeeMainMenuOption.PastScores:
                    option.GetComponent<Button>().onClick.AddListener(ScoresButtonClicked);
                    break;
                case YahtzeeMainMenuOption.EYahtzeeMainMenuOption.DiceSelection:
                    option.GetComponent<Button>().onClick.AddListener(DiceButtonClicked);
                    break;
                case YahtzeeMainMenuOption.EYahtzeeMainMenuOption.Quit:
                    option.GetComponent<Button>().onClick.AddListener(QuitButtonClicked);
                    break;
                default:
                    break;
            }
        }
    }

    private void StartButtonClicked() {
        //Debug.Log("Start");
        StartCoroutine(LoadSceneAfterAnimation("Scenes/YahtzeeClone"));
    }

    IEnumerator LoadSceneAfterAnimation(string scene) {
        crossFadeAnimator.SetTrigger("Start");
        //wait for transition
        while (!crossFadeAnimator.GetCurrentAnimatorStateInfo(0).IsName("crossfade_end")) {
            yield return null;
        }
        //wait for 
        while (crossFadeAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) {
            yield return null;
        }
        SceneManager.LoadScene("Scenes/YahtzeeClone");
    }

    private void ScoresButtonClicked() {
        Debug.Log("Past Scores not implemented yet");
    }

    private void DiceButtonClicked() {
        Debug.Log("Dice Selection not implemented yet");
    }
    private void QuitButtonClicked() {
        //Debug.Log("Quit");
    #if UNITY_STANDALONE
        Application.Quit();
    #endif
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }
}
