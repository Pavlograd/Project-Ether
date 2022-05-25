using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class playerInformation : MonoBehaviour
{
    public Text level;
    public Text PlayerName;

    public static playerInformation instance;

    public GameObject prefab = null;

    private TextureHolder textureHolderObj = null;

    private void Start()
    {
        // Does the file exist?
        if (File.Exists(Application.persistentDataPath + "/save.json"))
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(Application.persistentDataPath + "/save.json");

            // Deserialize the JSON data 
            // into a pattern matching the PlayerData class.
            PlayerClass player = JsonUtility.FromJson<PlayerClass>(fileContents);

            // Load islands from save
            level.text = player.level.ToString();
            PlayerName.text = player.name.ToString();
        }
        if (prefab != null)
        {
            getAllTexture();
        }
    }

    private void getAllTexture()
    {
        var info = new DirectoryInfo("Assets/Resources/Textures/PlayerTexture");
        var fileInfo = info.GetFiles();
        int i = 0;

        Debug.Log(fileInfo.Length);
        foreach (System.IO.FileInfo file in fileInfo)
        {
            if (file.Extension != ".meta")
            {
                Vector3 pos = new Vector3(50.0f, 105.0f, 0.0f); ;
                pos.x = pos.x + (100 * i);
                i = i + 1;
                GameObject newTextureButton = Instantiate(prefab, pos, transform.rotation);
                newTextureButton.transform.SetParent(GameObject.FindGameObjectWithTag("PlayerInfoTexture").transform, false);
                newTextureButton.SetActive(true);
                Image spriteRend = newTextureButton.GetComponent<Image>();
                spriteRend.sprite = Resources.Load<Sprite>("Textures/PlayerTexture/" + System.IO.Path.GetFileNameWithoutExtension(file.Name));
                textureHolderObj = newTextureButton.GetComponent<TextureHolder>();
                textureHolderObj.setTexture(Resources.Load<Sprite>("Textures/PlayerTexture/" + System.IO.Path.GetFileNameWithoutExtension(file.Name)));
            }
        }
    }

}
