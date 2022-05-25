using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;


public class SaveTexture : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Save(string Name)
    {
        if (File.Exists(Application.persistentDataPath + "/save.json"))
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(Application.persistentDataPath + "/save.json");

            // Deserialize the JSON data 
            // into a pattern matching the PlayerData class.
            PlayerClass player = JsonUtility.FromJson<PlayerClass>(fileContents);

            TextureClass texture = new TextureClass();

            texture.id = Name;

            player.inventory.textureInventory.Add(texture);

            //Save json
            //NetworkManager network = new NetworkManager();
            string json = JsonUtility.ToJson(player);

            Debug.Log(Application.persistentDataPath);
            File.WriteAllText(Application.persistentDataPath + "/save.json", json);
        }
    }
}
