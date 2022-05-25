using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestsAPI : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    void Start()
    {
        CrossSceneInfos.token = "9aae593e15488b98e57b6da07823bb122afbe53b";
        //StartCoroutine(GetUserData());
        StartCoroutine(GetUserTextures());
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator GetUserData()
    {
        Debug.Log(API.GetUserData());
        yield return null;
    }

    IEnumerator GetUserTextures()
    {
        API_User_Textures userTextures = API.GetUserTextures(API.GetUser().username);

        Debug.Log(userTextures);

        Debug.Log(userTextures.textures.Count);

        foreach (API_User_Texture texture in userTextures.textures)
        {
            byte[] bytes = System.Convert.FromBase64String(texture.texture);

            Texture2D texture2D = new Texture2D(16, 16);

            // Texture blurry if not set
            texture2D.filterMode = FilterMode.Point;

            texture2D.LoadImage(bytes);

            //if (wallFilter) texture = AddFilter(texture);

            // Texture 16*16, pivot is center and pixel per unity is 16
            Sprite sprite = Sprite.Create(texture2D, new Rect(0, 0, 16, 16), new Vector2(0.5f, 0.5f), 16);

            // Only get file name (with extension yet)
            sprite.name = texture.id;

            spriteRenderer.sprite = sprite;
        }

        yield return null;
    }
}
