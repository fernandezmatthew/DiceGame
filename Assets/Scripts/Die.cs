using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Die : MonoBehaviour
{
    [System.Serializable]
    public struct Face
    {
        public Sprite sprite;
        public int value;
    }

    public UnityEvent finishedRolling;

    [SerializeField] private ScriptableDie scriptableDie;
    private SpriteRenderer spriteRenderer;
    private int currentFaceIndex = 0;
    private int numFaces;
    private int rollingCount = 0;
    private bool isLocked = false;
    private GameObject highlight;

    public Face CurrentFace { get { return scriptableDie.faces[currentFaceIndex]; } }
    public int NumFaces { get { return scriptableDie.faces.Length;}}
    public bool IsRolling { get { return rollingCount > 0; } }
    public bool IsLocked { get { return isLocked; } }
    public Face[] Faces { get { return scriptableDie.faces;} }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = scriptableDie.faces[currentFaceIndex].sprite;
        numFaces = scriptableDie.faces.Length;
        InstantiateHightlight();
    }

    void Update()
    {
        
    }

    public void Roll(float length) {
        if (!isLocked) {
            rollingCount = 2;
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
            
            spriteRenderer.sprite = scriptableDie.faces[currentFaceIndex].sprite;
            elapsedTime += Time.deltaTime + switchTime;
            yield return new WaitForSeconds(switchTime);
        }
        rollingCount -= 1;
        if (rollingCount == 0) {
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
        rollingCount -= 1;
        if (rollingCount == 0) { 
            finishedRolling.Invoke();
        }
    }

    public void ToggleLock() {
        if (isLocked)
        {
            highlight.SetActive(false);
        }
        else {
            highlight.SetActive(true);

        }
        isLocked = !isLocked;
    }

    private void InstantiateHightlight() {
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
