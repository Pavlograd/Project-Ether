using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class receiverTexture : MonoBehaviour
{
    public SpriteRenderer oldSprite;
    private PlayerTextureApplication _target = null;

    public void changeTexture(Sprite newSprite)
    {
        oldSprite.sprite = newSprite;

    }

    public void actualiseTexture()
    {
        _target = GetComponent<PlayerTextureApplication>();
        _target.ApplyTextureOnPlayer("Textures/TextureApplicationOnPlayer/PreApplication");
        Debug.Log("ActualiseTexture");
        //UNITYEDITOR
        //AssetDatabase.Refresh();
    }
}
