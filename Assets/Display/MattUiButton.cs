using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Collections;


[RequireComponent(typeof(Image))]
public class MattUiButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler {
    [HideInInspector] public UnityEvent onClick;
    
    public Color HighlightedColor = Color.white;
    public Color PressedColor = Color.white;
    public float fadeDuration = .1f;

    private Image image;
    private Color originalColor;
    private Color onReleaseColor;
    private bool isPressed = false;
    private Coroutine lerpingCoroutine = null;

    private Vector3 onPressedTranslationVector = new Vector3(1, -3, 0);
    //Handle case where user clicks button quickly on edge, thus being out of bounds before mouse up
    private float clickOutOfBoundsForgivenessThreshold = .1f;
    private float timeSinceMouseDown = 100f;
    private bool countingTimeSinceMouseDown = false;
    private bool triggeredClickInMouseUp = false;

    void Start() {
        image = GetComponent<Image>();
        originalColor = image.color;
        onReleaseColor = originalColor;
    }

    void Update() {
        if (countingTimeSinceMouseDown) {
            timeSinceMouseDown += Time.deltaTime;
            if (timeSinceMouseDown > clickOutOfBoundsForgivenessThreshold) {
                countingTimeSinceMouseDown = false;
            }
        }
    }

    //Functions triggered from Mouse
    public void OnPointerEnter(PointerEventData eventData) {
        if (!isPressed) {
            TransitionButtonColor(HighlightedColor);
        }
        onReleaseColor = HighlightedColor;
    }

    public void OnPointerExit(PointerEventData eventData) {
        if (!isPressed) {
            TransitionButtonColor(originalColor);
        }
        onReleaseColor = originalColor;
    }

    public void OnPointerDown(PointerEventData eventData) {
        TransitionButtonColor(PressedColor);
        isPressed = true;
        transform.Translate(onPressedTranslationVector);
        countingTimeSinceMouseDown = true;
        timeSinceMouseDown = 0f;
    }

    //this executes before onpointerclick
    public void OnPointerUp(PointerEventData eventData) {
        TransitionButtonColor(onReleaseColor);
        isPressed = false;
        transform.Translate(-onPressedTranslationVector);
        if (timeSinceMouseDown < clickOutOfBoundsForgivenessThreshold) {
            onClick.Invoke();
            triggeredClickInMouseUp = true;
        }
    }


    public void OnPointerClick(PointerEventData eventData) {
        if (triggeredClickInMouseUp) { 
            triggeredClickInMouseUp = false;
        }
        else {
            onClick.Invoke();
        }
    }

    private void TransitionButtonColor(Color color) {
        if (lerpingCoroutine!= null) {
            StopCoroutine(lerpingCoroutine);
        }
        lerpingCoroutine = StartCoroutine(LerpColor(image.color, color));
    }
    private IEnumerator LerpColor(Color startColor, Color endColor) {
        float startTime = 0;
        float elapsedTime = startTime;
        while (elapsedTime < fadeDuration) {
            float interpolationPercent = elapsedTime / fadeDuration;
            image.color = Color.Lerp(startColor, endColor, interpolationPercent);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        image.color = endColor;
    }
}
