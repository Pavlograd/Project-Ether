using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
 
public class AnimationButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    private bool buttonPressed;
    private bool inAnimation = false;
    private float defaultAnimationTime = 0.25f;
    private float animationTime = 0f;
    private float buttonMinimumSize = 0.9f; // value from 0 to 1
    private Vector2 buttonSize;
    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = transform.GetComponent<RectTransform>();
        buttonSize = rectTransform.sizeDelta;
    }

    private void Update()
    {
        if (!inAnimation)
            return;
        if (buttonPressed) {
            animationTime += Time.deltaTime;
            if (animationTime >= defaultAnimationTime) {
                animationTime = defaultAnimationTime;
                inAnimation = false;
            }
        } else {
            animationTime -= Time.deltaTime;
            if (animationTime <= 0) {
                animationTime = 0;
                inAnimation = false;
            }
        }
        rectTransform.sizeDelta = buttonSize * Mathf.Lerp(1f, buttonMinimumSize, animationTime * (1 / defaultAnimationTime));
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
