using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class loadTextureSlot : MonoBehaviour
{
    public Text slot;
    public Text maxSlot;
    private int textureSlotNbr;
    private int maxTextureSlotNbr;

    // Start is called before the first frame update
    void Start()
    {
        if (File.Exists(Application.persistentDataPath + "/save.json"))
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(Application.persistentDataPath + "/save.json");

            // Deserialize the JSON data 
            // into a pattern matching the PlayerData class.
            PlayerClass player = JsonUtility.FromJson<PlayerClass>(fileContents);

            // Load islands from save
            textureSlotNbr = player.textureSlot;
            slot.text = textureSlotNbr.ToString();
            maxTextureSlotNbr = player.maxTextureSlot;
            maxSlot.text = maxTextureSlotNbr.ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (File.Exists(Application.persistentDataPath + "/save.json"))
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(Application.persistentDataPath + "/save.json");

            // Deserialize the JSON data 
            // into a pattern matching the PlayerData class.
            PlayerClass player = JsonUtility.FromJson<PlayerClass>(fileContents);

            // Load islands from save
            textureSlotNbr = player.textureSlot;
            slot.text = textureSlotNbr.ToString();
            maxTextureSlotNbr = player.maxTextureSlot;
            maxSlot.text = maxTextureSlotNbr.ToString();
        }
    }
}
