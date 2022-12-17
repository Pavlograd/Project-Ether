using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TextureSlot : MonoBehaviour
{
    public TMP_Text prefab = null;
    public TMP_Text max = null;
    // Start is called before the first frame update
    void Start()
    {
        prefab.SetText(API.GetTextureSlot().ToString());

        if (max != null)
        {
            max.SetText(API.GetMaxTextureSlot().ToString());
        }

    }

    // Update is called once per frame
    public void changeText()
    {
        prefab.SetText(API.GetTextureSlot().ToString());
    }

    public void buyMaxSlot()
    {
        API.PostMaxTextureSlot();
        max.SetText(API.GetMaxTextureSlot().ToString());
    }
}
