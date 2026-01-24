using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class FaceGridPrototype : MonoBehaviour
{
    [System.Serializable]
    public struct IngredientAmount {
        public Sprite sprite;
        public int amount;
    }
    public IngredientAmount[] ingredientAmounts;

    // Start is called before the first frame update
    void Start()
    {
        List<List<GameObject>> ingredients = new List<List<GameObject>>();
        for (int i = 0; i < ingredientAmounts.Length; i++) {
            List<GameObject> ingredient = new List<GameObject>();
            for (int j = 0; j < ingredientAmounts[i].amount; j++) {
                ingredient.Add(new GameObject("IngredientSprite"));
                ingredient[j].AddComponent<SpriteRenderer>();
                ingredient[j].GetComponent<SpriteRenderer>().sprite = ingredientAmounts[i].sprite;
                ingredient[j].GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder + 1;
                if (j == 0) {
                    ingredient[j].transform.parent = transform;
                }
                else {
                    ingredient[j].transform.parent = ingredient[0].transform;
                }
                ingredient[j].transform.localPosition = Vector3.zero;
            }
            ingredients.Add(ingredient);
        }

        Bounds dieBounds = GetComponent<SpriteRenderer>().bounds;


        if (ingredients.Count == 1) {
            for (int i = 1; i < ingredients[0].Count; i++) {
                ingredients[0][i].transform.Translate(dieBounds.extents.x * .5f, 0, 0);
            }
        }
        else if (ingredients.Count == 2) {
            //First, arrange the parent ingredients
            ingredients[0][0].transform.localPosition = new Vector3(-dieBounds.extents.x / 2, -dieBounds.extents.y / 2, ingredients[0][0].transform.position.z);
            ingredients[0][0].transform.localScale = ingredients[0][0].transform.localScale * .6f;

            ingredients[1][0].transform.localPosition = new Vector3(dieBounds.extents.x / 2, dieBounds.extents.y / 2, ingredients[1][0].transform.position.z);
            ingredients[1][0].transform.localScale = ingredients[1][0].transform.localScale * .6f;

            //resolve child ingredients.
            //for left ingredient, move slightly to the right
            //for right ingredient, move slightly to the left
        }

    }

    // Update is called once per frame
    void Update() {
    }
}
