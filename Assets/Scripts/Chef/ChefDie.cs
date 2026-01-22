using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ChefDie : MonoBehaviour
{
    [System.Serializable]
    public struct Face {
        public Ingredient ingredient;
        public ChefDieGrade grade;
    }

    [HideInInspector] public UnityEvent finishedRolling;

    [SerializeField] private ScriptableChefDie scriptableChefDie;
    private SpriteRenderer[] spriteRenderers;
    private int currentFaceIndex = 0;
    private int numFaces;
    private int rollingCoroutineCount = 0;
    private bool isLocked = false;
    private GameObject highlight;

    //public Face CurrentFace { get { return scriptableDie.faces[currentFaceIndex]; } }
    public int NumFaces { get { return scriptableChefDie.faces.Length;}}
    public bool IsRolling { get { return rollingCoroutineCount > 0; } }
    public bool IsLocked { get { return isLocked; } }
    //public Face[] Faces { get { return scriptableDie.faces;} }

    void Start() {
        spriteRenderers = GetComponentsInChildren<SpriteRenderer>();
        spriteRenderers[0].sprite = scriptableChefDie.background;
        spriteRenderers[0].color = scriptableChefDie.backgroundColor;
        UpdateFaceSprites(currentFaceIndex);
        
        
        numFaces = scriptableChefDie.faces.Length;
        InstantiateHighlight();
    }

    private void UpdateFaceSprites(int currentFaceIndex) {
        spriteRenderers[0].sprite = scriptableChefDie.background; //this doesnt have to happen each time
        spriteRenderers[1].sprite = scriptableChefDie.faces[currentFaceIndex].ingredient.IngredientSprite;
        spriteRenderers[2].sprite = scriptableChefDie.faces[currentFaceIndex].grade.gradeSprite;
    }

    public void Roll(float length) {
        if (!isLocked) {
            rollingCoroutineCount = 2;
            StartCoroutine(RollNumber(length));
            StartCoroutine(SpinDie(length));
        }
    }

    IEnumerator RollNumber(float length) {
        float elapsedTime = 0f;

        float switchTime = .05f;
        while (elapsedTime < length)
        {
            currentFaceIndex = Random.Range(0, numFaces);

            UpdateFaceSprites(currentFaceIndex);
            elapsedTime += Time.deltaTime + switchTime;
            yield return new WaitForSeconds(switchTime);
        }
        rollingCoroutineCount -= 1;
        if (rollingCoroutineCount == 0) {
            finishedRolling.Invoke();
        }
    }

    IEnumerator SpinDie(float length) {
        float elapsedTime = 0f;
        while (elapsedTime < length) {
            transform.Rotate(Vector3.forward * 1000f * Time.deltaTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.rotation = Quaternion.identity;
        rollingCoroutineCount -= 1;
        if (rollingCoroutineCount == 0) { 
            finishedRolling.Invoke();
        }
    }

    public void ToggleLock() {
        if (isLocked) {
            highlight.SetActive(false);
        }
        else {
            highlight.SetActive(true);
        }
        isLocked = !isLocked;
    }

    public void Reset() {
        StopAllCoroutines();
        currentFaceIndex = 0;
        UpdateFaceSprites(0);
        transform.rotation = Quaternion.identity;
    }

    private void InstantiateHighlight() {
        highlight = new GameObject("Highlight");
        highlight.SetActive(false);
        highlight.AddComponent<SpriteRenderer>();
        highlight.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("highlight");
        highlight.GetComponent<SpriteRenderer>().color = Color.red;
        highlight.GetComponent<SpriteRenderer>().sortingLayerName = "Hightlight";
        highlight.transform.parent = transform;
        highlight.transform.localPosition = Vector3.zero;
        highlight.transform.localScale = new Vector3(1.1f, 1.1f, 1f);
    }
}
