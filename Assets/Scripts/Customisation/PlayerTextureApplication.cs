using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class PlayerTextureApplication : MonoBehaviour
{
    private Sprite[] _sprites;
    [SerializeField] private SpriteRenderer spriteToUse;
    [SerializeField] private SpriteRenderer originalSprite;
    [SerializeField] private SpriteRenderer createdSprite;

    private Dictionary<string, List<Sprite>> _animsSheets;

    public void ApplyTextureOnPlayer(string OriginalSpritesPath)
    {
        GetSprites(OriginalSpritesPath);
        CreateFolder();
        DuplicateTexture();
        ApplyTextureOnEachSprite();
        SaveAnims();
    }

    private void SaveAnims()
    {
        _animsSheets = new Dictionary<string, List<Sprite>>();
        _animsSheets["attack"] = new List<Sprite> {_sprites[0], _sprites[1], _sprites[2], _sprites[3], _sprites[4], _sprites[5], _sprites[6], _sprites[7], _sprites[8]}; //attack
        _animsSheets["dead"] = new List<Sprite> {_sprites[9], _sprites[10], _sprites[11], _sprites[12]}; //dead
        _animsSheets["idle"] = new List<Sprite> {_sprites[13], _sprites[14], _sprites[15], _sprites[16], _sprites[17]}; //idle
        _animsSheets["run"] = new List<Sprite> {_sprites[18], _sprites[19], _sprites[20], _sprites[21], _sprites[22], _sprites[23], _sprites[24], _sprites[25]}; //run
        _animsSheets["takedamage"] = new List<Sprite> {_sprites[26]}; //takedamage
        
        //TODO FIX BECAUSE INCORRECT
    }

    private void GetSprites(string path)
    {
        _sprites = Resources.LoadAll<Sprite>(path);
    }

    private void DuplicateTexture()
    {
        Texture2D tex = originalSprite.sprite.texture;
        createdSprite.sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 16f);
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
                    if (pixel.a > 0.3f)
                    {
                        if (pixel.r > 0.12f && pixel.g > 0.12f && pixel.b > 0.2f)
                            tex.SetPixel(i, j, spriteToUse.sprite.texture.GetPixel(i, j));
                        else
                            tex.SetPixel(i, j, sprite.texture.GetPixel(i, j));
                    }
                    else
                        tex.SetPixel(i, j, new Color(1.0f, 1.0f, 1.0f, 0f));
                }
            }

            tex.Apply();
            if (sprite.name == "attack1")
                createdSprite.sprite = newSprite;
            SaveTextureToFile(newSprite);
        }
    }

    private void SaveTextureToFile(Sprite sprite)
    {
        var bytes = sprite.texture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/Resources/Textures/TextureApplicationOnPlayer/PostApplication/" + sprite.name + ".png", bytes);
    }

    private void CreateFolder()
    {
        string path = "Assets/Resources/Textures/TextureApplicationOnPlayer/PostApplication/";
        if (Directory.Exists(path))
            Directory.Delete(path, true);
        Directory.CreateDirectory(path);
    }
}