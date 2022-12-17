using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

// UIManager
// Used to get elements from the ui to show/hide or modify them without settings variables or getting them with Transform or GameObject
public class UIManager : MonoBehaviour
{
    public Dictionary<string, TMP_Text> texts = new Dictionary<string, TMP_Text>();
    public Dictionary<string, Button> buttons = new Dictionary<string, Button>();
    public Dictionary<string, RectTransform> elements = new Dictionary<string, RectTransform>();

    // Start is called before the first frame update
    void Start()
    {
        // Get All Texts
        TMP_Text[] allTexts = GetComponentsInChildren<TMP_Text>(true);

        // Get All Buttons
        Button[] allButtons = GetComponentsInChildren<Button>(true);

        // Get All RectTransform
        RectTransform[] rectTransforms = GetComponentsInChildren<RectTransform>(true);

        foreach (TMP_Text text in allTexts)
        {
            try
            {
                texts.Add(text.name, text);
            }
            catch (Exception error)
            {
                DumpError(error);
            }
        }

        foreach (Button button in allButtons)
        {
            try
            {
                buttons.Add(button.name, button);
            }
            catch (Exception error)
            {
                DumpError(error);
            }
        }

        foreach (RectTransform rectTransform in rectTransforms)
        {
            try
            {
                elements.Add(rectTransform.name, rectTransform);
            }
            catch (Exception error)
            {
                DumpError(error);
            }
        }
    }

    public void ChangeText(string textName, string text)
    {
        texts[textName].text = text;
    }

    public void HideText(string textName)
    {
        texts[textName].gameObject.SetActive(false);
    }
    public void ShowText(string textName)
    {
        texts[textName].gameObject.SetActive(true);
    }

    public void HideButton(string buttonName)
    {
        buttons[buttonName].gameObject.SetActive(false);
    }
    public void ShowButton(string buttonName)
    {
        buttons[buttonName].gameObject.SetActive(true);
    }

    public Button GetButton(string buttonName)
    {
        return buttons[buttonName];
    }

    public void HideElement(string elementName)
    {
        elements[elementName].gameObject.SetActive(false);
    }
    public void ShowElement(string elementName)
    {
        elements[elementName].gameObject.SetActive(true);
    }

    public RectTransform GetElement(string elementName)
    {
        return elements[elementName];
    }

    void DumpError(Exception error)
    {
        return;
    }
}
