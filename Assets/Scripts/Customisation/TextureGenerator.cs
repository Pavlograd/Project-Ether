using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TextureGenerator : MonoBehaviour
{
    public bool save = false;
    [SerializeField] Texture2D _wallFilter;

    void Start()
    {
        InvokeRepeating("GenerateTexture", 1f, 1f);
    }

    public void SaveTexture()
    {
        save = true;
    }

    public void GenerateTexture()
    {
        Texture2D texture = new Texture2D(16, 16);
        var path = Application.dataPath + "/Resources/Textures/";
        GameObject[] pixelsGO = GameObject.FindGameObjectsWithTag("Pixel");
        Color[] pixels = new Color[pixelsGO.Length];

        texture.filterMode = FilterMode.Point;
        texture.wrapMode = TextureWrapMode.Clamp;

        for (int i = 0; i < pixelsGO.Length; ++i)
        {
            pixels[i] = pixelsGO[i].GetComponent<Pixel>().color;
        }

        texture.SetPixels(pixels);

        if (_wallFilter != null)
        {
            for (int x = 0; x < _wallFilter.width; x++)
            {
                for (int y = 0; y < _wallFilter.height; y++)
                {
                    Color color = _wallFilter.GetPixel(x, y);

                    if (color.a == 1.0f) //Is not transparent
                    {
                        //Copy pixel colot in TexturaA
                        texture.SetPixel(x, y, color);
                    }
                }
            }
        }

        texture.name = (Random.Range(0, 256) + "test_SpriteSheet");
        texture.Apply();

        Image renderer = GetComponent<Image>();

        renderer.sprite = Sprite.Create(texture, new Rect(0, 0, 16, 16), new Vector2(0.5f, 0.5f));
        renderer.sprite.name = Random.Range(0, 256) + "Plop";
        renderer.material.mainTexture = texture;

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        if (save)
        {
            save = false;

            // No Coroutine here
            bool succes = API.PostTexture(System.Convert.ToBase64String(texture.EncodeToPNG()));

            Debug.Log("Texture has been posted : " + succes);

            // Reload scene to prevent user thinking they can modify their texture
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);

            //File.WriteAllBytes(path + renderer.sprite.name + ".png", texture.EncodeToPNG());
        }
    }
}
