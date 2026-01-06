using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Die : MonoBehaviour
{

    public UnityEvent finishedRolling;

    public ScriptableDie scriptableDie;
    SpriteRenderer spriteRenderer;
    public int currentFaceIndex = 0;
    int numFaces;
    public int isRolling = 0;
    public bool isLocked = false;
    GameObject highlight;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = scriptableDie.faces[currentFaceIndex].sprite;
        numFaces = scriptableDie.faces.Length;
        InstantiateHightlight();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Roll(float length) {
        if (!isLocked) {
            isRolling = 2;
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
        isRolling -= 1;
        if (isRolling == 0) {
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
        isRolling -= 1;
        if (isRolling == 0) { 
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
