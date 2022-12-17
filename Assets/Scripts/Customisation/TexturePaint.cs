using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TexturePaint : MonoBehaviour
{
    public Image image;
    public string id;
    [SerializeField] Brush brush;
    [SerializeField] List<TextureGenerator> generators = new List<TextureGenerator>();

    public void ChangeCurrentTexture(Sprite sprite, string _id)
    {
        image.sprite = sprite;
        id = _id;
    }

    public void ModifyTexture()
    {
        foreach (TextureGenerator item in generators)
        {
            item.SetId(id);
        }

        brush.ChangeStartTexture(image.sprite.texture);
    }
}
