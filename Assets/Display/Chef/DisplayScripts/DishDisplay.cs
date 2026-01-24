using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class DishDisplay : MonoBehaviour, IPointerClickHandler {
    [HideInInspector] public UnityEvent<Dish> dishClicked;

    private TMP_Text dishName;
    private Image dishImage;

    public Dish dish;

    private void OnValidate() {
        if (dishName == null) {
            dishName = GetComponent<TMP_Text>();
        }
        if (dishImage == null) {
            dishImage = GetComponent<Image>();
        }
        if (dishName != null && dishImage != null && dish != null) {
            UpdateDisplay();
        }
    }
    public void Start() {
        if (dishName == null) {
            dishName = GetComponent<TMP_Text>();
        }
        if (dishImage == null) {
            dishImage = GetComponent<Image>();
        }
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