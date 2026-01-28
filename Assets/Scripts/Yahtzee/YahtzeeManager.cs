using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class YahtzeeManager : MonoBehaviour {
    [HideInInspector] public UnityEvent diceRollFinished;
    [HideInInspector] public UnityEvent toggleLockDie;

    private Scoresheet scoresheet;
    private YahtzeeDie[] dice;
    private Camera cam;
    private Animator transitionAnimator;
    private int diceRolling = 0;
    private int rollsRemaining = 3;
    private int currentRound = 1;
    private bool isPaused = false;
    private bool hasStarted = false;

    //UI STUFF
    public RectTransform canvas;
    public MattUiButton rollButton;
    public ScoresheetDisplay scoresheetDisplayPrefab;
    public GameInfoDisplay rollsLeftDisplay;
    public GameInfoDisplay currentRoundDisplay;
    private ScoresheetDisplay scoresheetDisplay;
    public GameObject pauseMenu;

    //Save stuff
    private YahtzeeSaveHandler saveHandler;

    private void OnEnable() {
        if (hasStarted) {
            //subscribe to all input functions
            InputManager.PlayerInputActions.Yahtzee.Select.started += CheckIfDieClicked;
            InputManager.PlayerInputActions.Yahtzee.Restart.started += RestartClicked;
            InputManager.PlayerInputActions.Yahtzee.Quit.started += Quit;
            InputManager.PlayerInputActions.Yahtzee.Save.started += SaveGame;
            InputManager.PlayerInputActions.Global.Pause.started += PauseClicked;
            rollButton.onClick.AddListener(AttemptRollDice);

            //Subscribe to each die's "finishedRolling" event
            if (dice != null) {
                foreach (YahtzeeDie die in dice) {
                    die.finishedRolling.AddListener(DecrementDiceRolling);
                }
            }

            //subscribe to relevant events
            scoresheet.entryClicked.AddListener(AttemptFillEntry);
            scoresheet.entryFilled.AddListener(UpdateRound);
        }
    }
    private void Start() {
        hasStarted = true;

        //Handle animations, such as initial scene transition, can probably move
        transitionAnimator = FindFirstObjectByType<Animator>();
        transitionAnimator.SetTrigger("Open");
        if (InputManager.IsInitiated()) {
            InputManager.DisableInput();
            InputManager.DisableGlobalInput();
        }
        StartCoroutine(DisableInputWhileAnimating());

        //Find scene objects
        dice = FindObjectsByType<YahtzeeDie>(FindObjectsSortMode.None);
        cam = FindFirstObjectByType<Camera>();

        //pauseMenu.SetActive(false);

        //subscribe to all input events
        if (InputManager.IsInitiated()) {
            InputManager.PlayerInputActions.Yahtzee.Select.started += CheckIfDieClicked;
            InputManager.PlayerInputActions.Yahtzee.Restart.started += RestartClicked;
            InputManager.PlayerInputActions.Yahtzee.Quit.started += Quit;
            InputManager.PlayerInputActions.Yahtzee.Save.started += SaveGame;
            InputManager.PlayerInputActions.Global.Pause.started += PauseClicked;
            rollButton.onClick.AddListener(AttemptRollDice);
        }
        else {
            Debug.LogError("InputManager not initialized before yahtzeeManager.start()");
        }
        

        //Subscribe to each die's "finishedRolling" event
        if (dice != null) {
            foreach (YahtzeeDie die in dice) {
                die.finishedRolling.AddListener(DecrementDiceRolling);
            }
        }

        //Make yahtzee scoresheet
        scoresheet = new Scoresheet();
        //subscribe to relevant events
        scoresheet.entryClicked.AddListener(AttemptFillEntry);
        scoresheet.entryFilled.AddListener(UpdateRound);

        //THIS IS ALL UI STUFF, can move
        scoresheetDisplay = Instantiate(scoresheetDisplayPrefab, canvas.transform);
        scoresheetDisplay.Init(scoresheet);
        UpdateGameInfoDisplays();

        //This is all save stuff, can move
        saveHandler = new YahtzeeSaveHandler();
    }
    private void OnDisable() {
        InputManager.PlayerInputActions.Yahtzee.Select.started -= CheckIfDieClicked;
        InputManager.PlayerInputActions.Yahtzee.Restart.started -= RestartClicked;
        InputManager.PlayerInputActions.Yahtzee.Quit.started -= Quit;
        InputManager.PlayerInputActions.Yahtzee.Save.started -= SaveGame;
        InputManager.PlayerInputActions.Global.Pause.started -= PauseClicked;
        InputManager.PlayerInputActions.Yahtzee.Disable();

        if (dice != null) {
            foreach (YahtzeeDie die in dice) {
                die.finishedRolling.RemoveListener(DecrementDiceRolling);
            }
        }

        if (scoresheet != null) {
            scoresheet.entryClicked.RemoveListener(AttemptFillEntry);
            scoresheet.entryFilled.RemoveListener(UpdateRound);
        }
    }
    private void OnDestroy() {
     
    }

    //On Event Trigger Functions
    //*************************************

    //Triggers when a die finishes rolling
    public void DecrementDiceRolling() {
        diceRolling -= 1;
        if (diceRolling == 0) {
            ResolveRollFinished();
        }
    }

    //Triggers from decrement when dicerolling = 0
    private void ResolveRollFinished() {
        diceRollFinished.Invoke();
        UpdateScoresheetPotentials(dice);
        UpdateScoresheetDisplay();
    }

    private void UpdateScoresheetDisplay() {
        scoresheetDisplay.UpdateScoresheetDisplay();
    }

    private void UpdateGameInfoDisplays() {
        rollsLeftDisplay.UpdateDisplay(rollsRemaining);
        currentRoundDisplay.UpdateDisplay(currentRound);
    }

    //Triggers from resolveRollFinished() if all dice are finished
    private void UpdateScoresheetPotentials(in YahtzeeDie[] dice) {
        scoresheet.UpdatePotentials(dice);
    }

    //triggers from scoresheet.entryClicked
    public void AttemptFillEntry(Scoresheet.ScoreEntry scoreEntry) {
        //Make sure dice not rolling
        if (diceRolling == 0) {
            //Make sure player has made at least one roll
            if (rollsRemaining < 3) {
                scoresheet.FillEntry(scoreEntry);
            }
        }
    }

    //Called from scoresheet.entryFilled
    public void UpdateRound() {
        UpdateScoresheetDisplay();
        UnlockAllDice();
        if (currentRound < 13) {
            currentRound++;
            rollsRemaining = 3;
            UpdateGameInfoDisplays();
        }
        else {
            rollsRemaining = 0;
            //Do game end stuff
            SaveGame();
        }
    }

    private void SaveGame() {
        int[] scoreEntryValues = scoresheet.GetScoreEntryValues();
        int[] detailEntryVaues = scoresheet.GetDetailEntryValues();
        
        saveHandler.Save(scoreEntryValues, detailEntryVaues);
    }

    private void SaveGame(InputAction.CallbackContext ctx) {
        SaveGame();
    }

    private void UnlockAllDice() {
        foreach (var die in dice) {
            if (die.IsLocked) {
                die.transform.Translate(new Vector3(0, -.2f, 0));
                die.ToggleLock();
            }
        }
    }

    private void ResetAllDice() {
        foreach (var die in dice) {
            die.Reset();
        }
    }

    private IEnumerator DisableInputWhileAnimating() {
        InputManager.DisableInput();
        InputManager.DisableGlobalInput();
        Debug.Log("Disabling input");
        yield return null; // wait for animator to update

        //wait for transition
        while (transitionAnimator.IsInTransition(0)) {
            yield return null;
        }
        //wait for animation to finish
        while (transitionAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) {
            yield return null;
        }
        Debug.Log("Animation finished");
        InputManager.EnableInput();
        InputManager.EnableGlobalInput();
    }

    private IEnumerator RestartAnimation() {
        transitionAnimator.SetTrigger("Close");
        InputManager.DisableInput();
        InputManager.DisableGlobalInput();
        yield return null;

        //wait for transition
        while (transitionAnimator.IsInTransition(0)) {
            yield return null;
        }
        //wait for animation to finish
        while (transitionAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) {
            yield return null;
        }

        RestartData();
        transitionAnimator.SetTrigger("Open");
        yield return null;
        

        //wait for transition
        while (transitionAnimator.IsInTransition(0)) {
            yield return null;
        }
        //wait for animation to finish
        while (transitionAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1) {
            yield return null;
        }

        InputManager.EnableInput();
        InputManager.EnableGlobalInput();
    }

    //Triggers when roll button is pressed
    public void AttemptRollDice() {
        if (dice != null) {
            if (diceRolling == 0 && rollsRemaining > 0 && currentRound <= 13) {
                foreach (YahtzeeDie die in dice) {
                    if (!die.IsLocked) {
                        diceRolling++;
                        float rollLength = UnityEngine.Random.Range(.8f, 1.2f);
                        die.Roll(rollLength);
                    }
                }
                if (diceRolling > 0) {
                    rollsRemaining--;
                    UpdateGameInfoDisplays();
                }
            }
        }
    }

    //Triggers when left mouse button clicked at all
    public void CheckIfDieClicked(InputAction.CallbackContext ctx) {
        if (ctx.phase == InputActionPhase.Started) {
            Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);

            // Check if clicked on a die.
            if (hit.collider != null) {
                foreach (YahtzeeDie die in dice) {
                    if (die.gameObject == hit.collider.gameObject) {
                        //Toggle die lock if dice not currently rolling
                        if (diceRolling == 0 && rollsRemaining < 3) {
                            if (!die.IsLocked) {
                                die.transform.Translate(new Vector3(0, .2f, 0));
                            }
                            else {
                                die.transform.Translate(new Vector3(0, -.2f, 0));
                            }

                            die.ToggleLock();
                            break;
                        }
                    }
                }
            }
        }
    }

    public void RestartClicked(InputAction.CallbackContext ctx) {
        StartCoroutine(RestartAnimation());
    }

    //called from 'reset' inputaction
    public void RestartData() {
        //instead of reloading the current scene, lets reset the game in-place
        UnlockAllDice();
        ResetAllDice();
        currentRound = 1;
        rollsRemaining = 3;
        diceRolling = 0;
        scoresheet.ResetScoresheet();
        scoresheetDisplay.UpdateScoresheetDisplay();
        UpdateGameInfoDisplays();
    }

    public void RestartFromPause() {
        Pause();
        StartCoroutine(RestartAnimation());
    }

    //called from 'pause' inputaction
    public void PauseClicked(InputAction.CallbackContext ctx) {
        Pause();
    }

    public void Pause() {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
        if (isPaused) {
            Time.timeScale = 0f;
            InputManager.DisablePlayerInput();
        }
        else {
            Time.timeScale = 1f;
            InputManager.EnablePlayerInput();
        }
        HandlePauseUi();
    }

    private void HandlePauseUi() {
        if (isPaused) {
            pauseMenu.SetActive(true);
        }
        else { 
            pauseMenu.SetActive(false);
        }
    }

    public void ReturnMainMenu() {
        Pause();
        transitionAnimator.SetTrigger("Close");
        StartCoroutine(LoadSceneAfterAnimation("Scenes/YahtzeeMainMenu"));
        StartCoroutine(DisableInputWhileAnimating());
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

    public void Quit(InputAction.CallbackContext ctx) {
    #if UNITY_STANDALONE
        Application.Quit();
    #endif
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
    }
}
