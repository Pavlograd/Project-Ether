using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

public class LoadTextureInventory : MonoBehaviour
{
    [SerializeField] private GameObject texture;
    [SerializeField] private string id;
    // Start is called before the first frame update
    void Start()
    {
        load();
    }

    // Update is called once per frame
    void Update()
    {
        load();
    }

    private void load() {
        if (File.Exists(Application.persistentDataPath + "/save.json"))
        {
            int i = 0;
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(Application.persistentDataPath + "/save.json");

            // Deserialize the JSON data 
            // into a pattern matching the PlayerData class.
            PlayerClass player = JsonUtility.FromJson<PlayerClass>(fileContents);

            foreach (TextureClass item in player.inventory.textureInventory) {
                if (item.id == id) {
                    texture.SetActive(true);
                    i = i + 1;
                }                                      
            }
            if (i == 0)
                texture.SetActive(false);
        }
    }
}
