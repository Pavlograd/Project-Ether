using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Brush : MonoBehaviour
{
    public Color color;
    [SerializeField] Image _image;

    void Awake()
    {
        // Set default color white
        color = _image.color;
    }

    public void BrushImage(Image image)
    {
        Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        //button.colors.normalColor = color;
        image.color = color;
    }

    public void ChangeBrushColor(Image image)
    {
        _image = image;
        color = image.color;
    }

    public void ChangeImageColor(Image image)
    {
        _image.color = image.color;
        color = image.color;
    }
}
