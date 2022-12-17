using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Pixel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Color color;
    public Image _image;
    Brush _brush;

    void Awake()
    {
        // Set default color white
        color = new Color(1f, 1f, 1f, 1f);
        _image = GetComponent<Image>();
        _image.color = color;
    }

    void Start()
    {
        _brush = GameObject.Find("Brush").GetComponent<Brush>();
        _brush.pixels.Add(this);
    }

    // When brush click on the pixel
    public void OnPointerClick(PointerEventData eventData)
    {
        //color = _brush.color;
        //_image.color = color;

        _brush.Paint(this, true);

    }

    // When brush is over
    public void OnPointerEnter(PointerEventData eventData)
    {
        _brush.Paint(this, Input.GetMouseButton(0));

        /*if (Input.GetMouseButton(0))
        {
            color = _brush.color;
        }

        _image.color = _brush.color;*/
    }

    // When brush is not over anymore
    public void OnPointerExit(PointerEventData eventData)
    {
        _image.color = color;
    }
}
