using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;

public class LoadTextures : MonoBehaviour
{
    // Texture for the game (that's why it's not called sprites)
    public List<Sprite> textures;
    public List<Sprite> texturesWall;
    public Object[] tilesGround;
    public Object[] tilesWall;
    [SerializeField] Texture2D _wallFilter;
    [SerializeField] DonjonGeneratorData generationData;

    // Start is called before the first frame update
    void Awake()
    {
        GetTexturesPacks();
        GetTexturesFromUsername(CrossSceneInfos.username);

        tilesGround = CreateTiles(textures);
        tilesWall = CreateTiles(texturesWall);
    }

    void GetTexturesPacks()
    {
        foreach (TexturePack texturesPack in generationData._texturesPacks)
        {
            foreach (Sprite item in texturesPack.walls)
            {
                textures.Add(item);
                texturesWall.Add(item);
            }

            foreach (Sprite item in texturesPack.grounds)
            {
                textures.Add(item);
                texturesWall.Add(item);
            }
        }
    }

    void GetTexturesFromUsername(string username)
    {
        API_User_Textures userTextures = API.GetUserTextures(username);

        foreach (API_User_Texture texture in userTextures.textures)
        {
            byte[] imageBytes = System.Convert.FromBase64String(texture.texture);

            textures.Add(GetSpriteFromFile(texture.id, imageBytes, false));
            texturesWall.Add(GetSpriteFromFile(texture.id, imageBytes, true));
        }
    }

    Sprite GetSpriteFromFile(string name, byte[] imageBytes, bool wallFilter)
    {
        Texture2D texture = new Texture2D(16, 16);

        // Texture blurry if not set
        texture.filterMode = FilterMode.Point;

        texture.LoadImage(imageBytes);

        if (wallFilter) texture = AddFilter(texture);

        // Texture 16*16, pivot is center and pixel per unity is 16
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 16, 16), new Vector2(0.5f, 0.5f), 16);

        // Only get file name (with extension yet)
        sprite.name = name;

        return sprite;
    }

    Object[] CreateTiles(List<Sprite> sprites)
    {
        Object[] tiles = new Object[sprites.Count];

        for (int i = 0; i < sprites.Count; i++)
        {
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = sprites[i];

            tile.name = sprites[i].name;

            tiles[i] = tile;
        }

        return tiles;
    }

    Texture2D AddFilter(Texture2D texture)
    {
        for (int x = 0; x < _wallFilter.width; x++)
        {
            for (int y = 0; y < _wallFilter.height; y++)
            {
                Color color = _wallFilter.GetPixel(x, y);

                if (color.a == 1.0f) //Is not transparent
                {
                    //Debug.Log(color.a);

                    //Copy pixel colot in TexturaA
                    texture.SetPixel(x, y, color);
                }
            }
        }

        texture.Apply();
        return texture;
    }

    void LoadTexturesFromResources()
    {
        /*_textures = Resources.LoadAll("Textures", typeof(Sprite));

        // Deserialize the JSON data 
        // into a pattern matching the PlayerData class.
        Vector3 position = new Vector3(80.0f, 1010.0f, 0.0f);

        RectTransform contentTransform = _content.GetComponent<RectTransform>();

        for (var i = 0; i < _textures.Length; i++)
        {
            GameObject newButton = Instantiate(_button, position, Quaternion.identity, contentTransform);

            // Change Sprite Image
            newButton.GetComponent<Image>().sprite = (Sprite)_textures[i];

            position.x += 120.0f;

            if (position.x >= 1200.0f)
            {
                position.x = 80.0f;
                position.y -= 120.0f;
            }
        }*/
    }
}
