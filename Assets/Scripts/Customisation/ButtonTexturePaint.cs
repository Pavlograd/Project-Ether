using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTexturePaint : MonoBehaviour
{
    [SerializeField] Image image;
    public string id = "";

    private TexturePaint texturePaint;

    void Start()
    {
        texturePaint = GameObject.Find("TexturePaint").GetComponent<TexturePaint>();
    }

    public void ChangeTexture()
    {
        texturePaint.ChangeCurrentTexture(image.sprite, id);
    }

    public void SetIdTexture(string _id)
    {
        id = _id;
    }
}
