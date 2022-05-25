using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ColorCircle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image _imageColor;
    [SerializeField] InputField _hexColor;
    [SerializeField] Slider _transparancy;
    Texture2D _texture;
    bool _inside = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TakeScreenShot());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && _inside)
        {
            Color colorPicked;

            colorPicked = _texture.GetPixel((int)Input.mousePosition.x, (int)Input.mousePosition.y);

            _imageColor.color = colorPicked;
            _transparancy.value = colorPicked.a;
            ChangeHexColor();
        }
    }

    public void ChangeColorFromString(string hexColor)
    {
        Color colorPicked;

        if (ColorUtility.TryParseHtmlString(hexColor, out colorPicked))
        {
            _imageColor.color = colorPicked;
            _transparancy.value = colorPicked.a;
        }
    }

    void ChangeHexColor()
    {
        _hexColor.text = "#" + ColorUtility.ToHtmlStringRGB(_imageColor.color);
    }

    public void ChangeTransparancy(float transparancy)
    {
        Color colorPicked = _imageColor.color;

        colorPicked.a = transparancy;
        _imageColor.color = colorPicked;
        ChangeHexColor();
    }

    IEnumerator TakeScreenShot()
    {
        yield return new WaitForEndOfFrame();

        _texture = ScreenCapture.CaptureScreenshotAsTexture();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _inside = true;
    }

    // When brush is not over anymore
    public void OnPointerExit(PointerEventData eventData)
    {
        _inside = false;
    }
}
