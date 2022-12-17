using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class settexturePrice : MonoBehaviour
{
    public Sprite spriteToChange = null;

    public TMP_InputField input = null;

    public void setTexture(Button sprite)
    {
        if (sprite != null)
        {
            spriteToChange = sprite.GetComponent<Image>().sprite; ;
        }
    }

    public void sellItem()
    {   
        if (spriteToChange != null)
        {
            API_ShopTextureToSell objectTexture = new API_ShopTextureToSell();

            Texture2D texture = spriteToChange.texture;
            //objectTexture.seller = "http://projectether.francecentral.cloudapp.azure.com/api/users/" + API.GetUser().id + "/";
            objectTexture.texture = System.Convert.ToBase64String(DeCompress(texture).EncodeToPNG());
            objectTexture.price = input.text;
            API.PostShopTextures(objectTexture);
            spriteToChange = null;
            input.text = null;
            
        } else
        {
            Debug.Log("Input or sprite empty");
        }
    }

    public static Texture2D DeCompress(Texture2D source)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
    }
}
