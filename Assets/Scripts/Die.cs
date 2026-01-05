using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Die : MonoBehaviour
{

    [SerializeField] ScriptableDie scriptableDie;
    SpriteRenderer spriteRenderer;
    int currentFaceIndex = 0;
    int numFaces;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = scriptableDie.faces[currentFaceIndex].sprite;
        numFaces = scriptableDie.faces.Length;
        Roll(1.0f);
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Roll(float length) {
        StartCoroutine(RollNumber(length));
        StartCoroutine(SpinDie(length));
    }

    IEnumerator RollNumber(float length) {
        float elapsedTime = 0f;

        float switchTime = .1f;
        while (elapsedTime < length)
        {
            int newIndex = Random.Range(0, numFaces);
            while (newIndex == currentFaceIndex) {
                newIndex = Random.Range(0, numFaces);
            }
            currentFaceIndex = newIndex;
            spriteRenderer.sprite = scriptableDie.faces[currentFaceIndex].sprite;
            elapsedTime += Time.deltaTime + switchTime;
            yield return new WaitForSeconds(switchTime);
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
    }
}
