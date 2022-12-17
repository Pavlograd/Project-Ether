using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadTextureToSell : MonoBehaviour
{
    public GameObject prefab = null;

    private TextureHolder textureHolderObj = null;

    private List<GameObject> texturesList = new List<GameObject>();

    private void Start()
    {
        getAllTexture();

    }

    public void getAllTexture()
    {
        if (texturesList.Count > 0) return;
        var info = new DirectoryInfo("Assets/Resources/Textures/PlayerTexture");
        var fileInfo = info.GetFiles();
        int i = 0;

        //Debug.Log(fileInfo.Length);
        foreach (System.IO.FileInfo file in fileInfo)
        {
            if (file.Extension != ".meta")
            {
                Vector3 pos = new Vector3(25.0f, 0.0f, 0.0f); ;
                pos.x = pos.x + (100 * i);
                i = i + 1;
                GameObject newTextureButton = Instantiate(prefab, pos, transform.rotation);
                newTextureButton.transform.SetParent(GameObject.FindGameObjectWithTag("PlayerInfoTexture").transform, false);
                newTextureButton.SetActive(true);
                Image spriteRend = newTextureButton.GetComponent<Image>();
                spriteRend.sprite = Resources.Load<Sprite>("Textures/PlayerTexture/" + System.IO.Path.GetFileNameWithoutExtension(file.Name));
                //textureHolderObj = newTextureButton.GetComponent<TextureHolder>();
                //textureHolderObj.setTexture(Resources.Load<Sprite>("Textures/PlayerTexture/" + System.IO.Path.GetFileNameWithoutExtension(file.Name)));
                //texturesList.Add(newTextureButton);
            }
        }
    }
}

