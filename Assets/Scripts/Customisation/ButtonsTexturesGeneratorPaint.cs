using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsTexturesGeneratorPaint : MonoBehaviour
{
    private List<Sprite> textures = new List<Sprite>();
    [SerializeField] private GameObject _content;
    [SerializeField] private GameObject _button;

    // Use start not awake please
    void Start()
    {
        API_User_Textures userTextures = API.GetUserTextures(CrossSceneInfos.username); //  For ids

        GetTexturesFromUsername(CrossSceneInfos.username);

        // Position of the first button
        Vector3 position = new Vector3(80.0f, -150.0f, 0.0f);

        // Futur parent of buttons
        RectTransform contentTransform = _content.GetComponent<RectTransform>();

        int index = 0;

        foreach (Sprite texture in textures)
        {
            GameObject newButton = Instantiate(_button, position, Quaternion.identity, contentTransform);

            newButton.transform.localPosition = position;

            // Change Sprite Image
            newButton.GetComponent<Image>().sprite = texture;
            newButton.GetComponent<ButtonTexturePaint>().SetIdTexture(userTextures.textures[index].id);

            position.x += 120.0f;

            // Next life if too on the right
            if (position.x >= 1200.0f)
            {
                position.x = 80.0f;
                position.y -= 120.0f;
            }

            index++;
        }

        // Script is now useless
        Destroy(this);
    }

    void GetTexturesFromUsername(string username)
    {
        API_User_Textures userTextures = API.GetUserTextures(username);

        foreach (API_User_Texture texture in userTextures.textures)
        {
            byte[] imageBytes = System.Convert.FromBase64String(texture.texture);

            textures.Add(GetSpriteFromFile(texture.id, imageBytes, false));
        }
    }

    Sprite GetSpriteFromFile(string name, byte[] imageBytes, bool wallFilter)
    {
        Texture2D texture = new Texture2D(16, 16);

        // Texture blurry if not set
        texture.filterMode = FilterMode.Point;

        texture.LoadImage(imageBytes);

        // Texture 16*16, pivot is center and pixel per unity is 16
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, 16, 16), new Vector2(0.5f, 0.5f), 16);

        // Only get file name (with extension yet)
        sprite.name = name;

        return sprite;
    }
}
