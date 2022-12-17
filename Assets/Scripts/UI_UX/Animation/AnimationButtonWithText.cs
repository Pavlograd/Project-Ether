using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class AnimationButtonWithText : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    private bool buttonPressed;
    private bool inAnimation = false;
    private float defaultAnimationTime = 0.25f;
    private float animationTime = 0f;
    private float buttonMinimumSize = 0.9f; // value from 0 to 1
    private Vector2 buttonSize;
    private float textSize;
    private RectTransform rectTransform;
    private TextMeshProUGUI textMeshPro;

    private void Start()
    {
        rectTransform = transform.GetComponent<RectTransform>();
        buttonSize = rectTransform.sizeDelta;
        textMeshPro = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        if (textMeshPro)
            textSize = textMeshPro.fontSize;
    }

    private void Update()
    {
        if (!inAnimation)
            return;
        if (buttonPressed) { // Start animation
            animationTime += Time.deltaTime;
            if (animationTime >= defaultAnimationTime) {
                animationTime = defaultAnimationTime;
                inAnimation = false;
            }
        } else { // End animation
            animationTime -= Time.deltaTime;
            if (animationTime <= 0) {
                animationTime = 0;
                inAnimation = false;
            }
        }
        // Set size with start or end animation
        float actualSize = Mathf.Lerp(1f, buttonMinimumSize, animationTime * (1 / defaultAnimationTime));
        rectTransform.sizeDelta = buttonSize * actualSize;
        if (textMeshPro)
            textMeshPro.fontSize = textSize * actualSize;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        buttonPressed = true;
        inAnimation = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        buttonPressed = false;
        inAnimation = true;
    }
}
