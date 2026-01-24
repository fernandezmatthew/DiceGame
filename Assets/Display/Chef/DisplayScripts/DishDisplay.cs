using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class DishDisplay : MonoBehaviour, IPointerClickHandler {
    public UnityEvent<Dish> dishClicked;

    public TMP_Text dishName;
    public Image dishImage;

    public Dish dish;

    private void OnValidate() {
        if (dishName != null && dishImage != null && dish != null) {
            UpdateDisplay();
        }
    }
    public void Start() {
        UpdateDisplay();
    }
    public void SetDish(Dish dish) {
        this.dish = dish;
        dishName.text = dish.DishName;
        dishImage.sprite = dish.sprite;
    }

    public void UpdateDisplay() {
        dishName.text = dish.DishName;
        dishImage.sprite = dish.sprite;
    }

    public void OnPointerClick(PointerEventData eventData) {
        dishClicked.Invoke(dish);
    }
}