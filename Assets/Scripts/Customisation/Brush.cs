using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

[System.Serializable]
public struct Tool
{
    public string name;
    public Image background;
}

public class Brush : MonoBehaviour
{
    public Color color;
    [SerializeField] Image _image;
    public List<Pixel> pixels = new List<Pixel>();
    public Pixel[][] pixelsDoubleArray = null;
    [SerializeField] List<Color[]> historyColors = new List<Color[]>(); // History if you want to undo
    [SerializeField] List<Tool> tools = new List<Tool>();
    [SerializeField] Color[] futureColors = null; // History if you want to undo what you just undo
    [SerializeField] Button buttonBack;
    [SerializeField] Button buttonFuture;
    [SerializeField] Sprite defaultSprite;
    bool canOfPaint = false;
    bool eraser = false;
    bool eye = false;
    bool pen = true;

    int sizeList = 16;
    float timerHistoric = 0.0f;
    Texture2D startTexture = null;

    void Awake()
    {
        // Set default color white
        color = _image.color;
    }

    void Update()
    {
        buttonFuture.interactable = futureColors != null;
        buttonBack.interactable = historyColors.Count > 0;
        timerHistoric += Time.deltaTime;
    }

    public Color[] GetPixelsAsColors() // Return all the colors from the pixels
    {
        Color[] pixelsColors = new Color[pixels.Count];

        for (int i = 0; i < pixels.Count; ++i)
        {
            pixelsColors[i] = pixels[i].color;
        }

        return pixelsColors;
    }

    public void GoBack()
    {
        if (historyColors.Count == 0) return; // Can't replace with nothing

        futureColors = GetPixelsAsColors(); // Present pattern is now the future

        ReplaceColors(historyColors[historyColors.Count - 1]);

        historyColors.RemoveAt(historyColors.Count - 1); // Not in the history now as it is the present pattern
        timerHistoric = 0.0f;
    }

    void AddHistoric(Color[] oldColors)
    {
        if (historyColors.Count == 0 || !oldColors.SequenceEqual(historyColors[historyColors.Count - 1]))
        {
            PrintDebug.Maurin("Add historic");
            historyColors.Add(oldColors);
        }
        timerHistoric = 0.0f;
    }

    public void GoFuture()
    {
        if (futureColors == null) return; // Can't replace with nothing

        AddHistoric(GetPixelsAsColors()); // Present is now past
        futureColors = GetPixelsAsColors(); // Present pattern is now the future

        ReplaceColors(futureColors);
        futureColors = null; // Not in the history now as it is the present pattern
    }

    void ReplaceColors(Color[] newColors)
    {
        for (int i = 0; i < pixels.Count; ++i)
        {
            pixels[i].color = newColors[i]; // For hover
            pixels[i]._image.color = newColors[i];
        }
    }

    public void ToggleCan() // Wether you will paint an area or not
    {
        UnactivateTools();
        canOfPaint = true;
        ToggleTool("can");

        if (pixelsDoubleArray == null) // Create the double array out of the list
        {
            int count = 0;
            pixelsDoubleArray = new Pixel[sizeList][];

            for (int i = 0; i < sizeList; i++)
            {
                pixelsDoubleArray[i] = new Pixel[sizeList];

                for (int y = 0; y < sizeList; y++, count++)
                {
                    pixelsDoubleArray[i][y] = pixels[count];
                }
            }
        }
    }

    void ToggleTool(string name)
    {
        foreach (Tool tool in tools)
        {
            tool.background.enabled = (tool.name == name);
        }
    }

    void UnactivateTools()
    {
        canOfPaint = false;
        eye = false;
        eraser = false;
        pen = false;
    }

    public void ToggleEyeDropper()
    {
        UnactivateTools();
        ToggleTool("eye");
        eye = true;
    }

    public void ToggleEraser()
    {
        UnactivateTools();
        ToggleTool("eraser");
        eraser = true;
    }

    public void TogglePen()
    {
        UnactivateTools();
        ToggleTool("pen");
        pen = true;
    }


    void Start()
    {
        PrintDebug.Maurin("number pixels :" + pixels.Count);

        Invoke("ChangePixelsFromTexture", 0.1f);
    }

    public void ChangeStartTexture(Texture2D texture) // Called to modify starting texture
    {
        startTexture = texture;
    }

    void ChangePixelsFromTexture()
    {
        for (int i = 0; i < pixels.Count; ++i) // Change sprites of pixels in case an error occured IMPORTANT
        {
            pixels[i]._image.sprite = defaultSprite;
        }

        if (pixels.Count != 0 && startTexture != null) // Change pixels if player choose a default texture
        {
            int sourceMipLevel = 0; // Don't try to delete that line
            Color[] colors = startTexture.GetPixels(sourceMipLevel);

            for (int i = 0; i < pixels.Count; ++i)
            {
                pixels[i].color = colors[i];
                pixels[i]._image.color = colors[i];
            }
        }
    }

    public void BrushImage(Image image)
    {
        Color color = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        //button.colors.normalColor = color;
        image.color = color;
    }

    public void ChangeBrushColor(Image image = null)
    {
        if (image != null) _image = image; // Eraser will be null
        color = image.color;
    }

    public void ChangeOnlyBrushColor(Image image)
    {
        color = image.color;
    }

    public void ChangeImageColor(Image image)
    {
        _image.color = image.color;
        color = image.color;
    }

    public void Paint(Pixel pixel, bool click) // Click is only usefull for computer don't take in account if necessary
    {
        PrintDebug.Maurin("Paint a pixel");

        // The difference between color and image from pixel is that color will be the real color and image will be the visual color
        // it is for hover situation

        if (click)
        {
            if (timerHistoric > 0.5f)
            {
                AddHistoric(GetPixelsAsColors());
            }
        }

        if (canOfPaint && click)
        {
            Color colorToReplace = pixel.color;

            if (colorToReplace == _image.color) return;

            for (int i = 0; i < sizeList; i++)
            {
                for (int y = 0; y < sizeList; y++)
                {
                    if (pixelsDoubleArray[i][y] == pixel)
                    {
                        CanPaintPixel(i, y, colorToReplace); // Will paint the pixel and then try to paint the pixels next to it
                        break;
                    }
                }
            }
        }
        else if (eraser && click)
        {
            pixel.color = Color.white;
        }
        else if (pen && click)
        {
            pixel.color = color;
        }
        else if (eye && click)
        {
            _image.color = pixel.color;
            color = _image.color;
            pixel.color = color;
        }

        if (eraser)
        {
            pixel._image.color = Color.white;
        }
        else if (eye)
        {
            pixel._image.color = pixel._image.color;
        }
        else
        {
            pixel._image.color = color;
        }
    }

    void CanPaintPixel(int x, int y, Color colorToReplace)
    {
        Pixel pixel = pixelsDoubleArray[x][y];

        if (pixel.color == colorToReplace)
        {
            pixel.color = color;
            pixel._image.color = color;

            // Try now to paint the neigbourhs pixels
            if (x > 0)
            {
                CanPaintPixel(x - 1, y, colorToReplace);
            }
            if (x < sizeList - 1)
            {
                CanPaintPixel(x + 1, y, colorToReplace);
            }
            if (y > 0)
            {
                CanPaintPixel(x, y - 1, colorToReplace);
            }
            if (y < sizeList - 1)
            {
                CanPaintPixel(x, y + 1, colorToReplace);
            }
        }
    }
}
