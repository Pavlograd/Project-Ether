using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class EnvTextureApplication : MonoBehaviour
{
    private Sprite[] _sprites;
    [SerializeField] private SpriteRenderer spriteToUse;
    [SerializeField] private SpriteRenderer originalSpriteWall;
    [SerializeField] private SpriteRenderer createdSpriteWall;
    [SerializeField] private SpriteRenderer createdSpriteGround;

    private Dictionary<string, List<Sprite>> _animsSheets;

    public void ApplyTextureOnEnv(string OriginalSpritesPath)
    {
        GetSprites(OriginalSpritesPath);
        CreateFolder();
        DuplicateTexture();
        ApplyTextureOnEachSprite();
    }

    private void GetSprites(string path)
    {
        _sprites = Resources.LoadAll<Sprite>(path);
    }

    private void DuplicateTexture()
    {
        Texture2D WallTex = originalSpriteWall.sprite.texture;
        Texture2D GroundTex = new Texture2D(16, 16);
        createdSpriteWall.sprite = Sprite.Create(WallTex, new Rect(0.0f, 0.0f, WallTex.width, WallTex.height), new Vector2(0.5f, 0.5f), 16f);
        createdSpriteGround.sprite = Sprite.Create(GroundTex, new Rect(0.0f, 0.0f, GroundTex.width, GroundTex.height), new Vector2(0.5f, 0.5f), 16f);
    }

    private void ApplyTextureOnEachSprite()
    {
        foreach (Sprite sprite in _sprites)
        {
            Sprite newSprite = Sprite.Create(new Texture2D(sprite.texture.width, sprite.texture.height), new Rect(0.0f, 0.0f, sprite.texture.width, sprite.texture.height), new Vector2(0.5f, 0.5f), 16f);
            Texture2D tex = newSprite.texture;
            newSprite.name = sprite.name;
            newSprite.texture.filterMode = FilterMode.Point;

            // LOOP OVER EVERY PIXEL OF THE SPRITE
            for (int i = 0; i < sprite.texture.width; i++)
            {
                for (int j = 0; j < sprite.texture.height; j++)
                {
                    Color pixel = sprite.texture.GetPixel(i, j);
                    if (sprite.name == "Wall")
                    {
                            Debug.Log(pixel.r + " " + pixel.g + " " + pixel.b + " " + pixel.a);
                            if (pixel.r == 0f && pixel.g == 0f && pixel.b == 0f && pixel.a == 0f)
                                tex.SetPixel(i, j, spriteToUse.sprite.texture.GetPixel(i, j));
                            else
                                tex.SetPixel(i, j, sprite.texture.GetPixel(i, j));
                    }
                    else if (sprite.name == "Ground")
                    {
                        tex.SetPixel(i, j, spriteToUse.sprite.texture.GetPixel(i, j));
                    }
                }
            }

            tex.Apply();
            if (sprite.name == "Wall")
                createdSpriteWall.sprite = newSprite;
            else if (sprite.name == "Ground")
                createdSpriteGround.sprite = newSprite;

            SaveTextureToFile(newSprite);
        }
    }

    private void SaveTextureToFile(Sprite sprite)
    {
        var bytes = sprite.texture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/Resources/Textures/TextureApplicationOnEnv/PostApplication/" + sprite.name + ".png", bytes);
    }

    private void CreateFolder()
    {
        string path = "Assets/Resources/Textures/TextureApplicationOnEnv/PostApplication/";
        if (Directory.Exists(path))
            Directory.Delete(path, true);
        Directory.CreateDirectory(path);
    }
}