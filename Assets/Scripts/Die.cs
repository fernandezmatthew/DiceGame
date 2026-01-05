using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Die : MonoBehaviour
{

    public UnityEvent finishedRolling;

    [SerializeField] public ScriptableDie scriptableDie;
    SpriteRenderer spriteRenderer;
    public int currentFaceIndex = 0;
    int numFaces;
    public int isRolling = 0;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = scriptableDie.faces[currentFaceIndex].sprite;
        numFaces = scriptableDie.faces.Length;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Roll(float length) {
        isRolling = 2;
        StartCoroutine(RollNumber(length));
        StartCoroutine(SpinDie(length));
    }

    IEnumerator RollNumber(float length) {
        float elapsedTime = 0f;

        float switchTime = .1f;
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
}
