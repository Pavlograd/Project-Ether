using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class textureSlot : MonoBehaviour
{
    public int textureSlotNbr;
    public int maxTextureSlotNbr;

    // Start is called before the first frame update
    void Start()
    {
        getSlot();
    }

    public void addSlot(int nbr)
    {
        if (textureSlotNbr + nbr <= maxTextureSlotNbr)
        {
            textureSlotNbr = textureSlotNbr + nbr;
            if (File.Exists(Application.persistentDataPath + "/save.json"))
            {
                // Read the entire file and save its contents.
                string fileContents = File.ReadAllText(Application.persistentDataPath + "/save.json");

                // Deserialize the JSON data 
                // into a pattern matching the PlayerData class.
                PlayerClass player = JsonUtility.FromJson<PlayerClass>(fileContents);
                player.textureSlot = textureSlotNbr; 
                //Save json
                //NetworkManager network = new NetworkManager();
                string json = JsonUtility.ToJson(player);

                File.WriteAllText(Application.persistentDataPath + "/save.json", json);
            }
        }
    }

    private void getSlot()
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
            maxTextureSlotNbr = player.maxTextureSlot;
        }
    }

}
