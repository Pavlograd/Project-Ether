using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class inventorySave : MonoBehaviour
{
    void Start()
    {
        loadSkills();
    }

    void Update()
    {
        
    }

    public void addItem(int id)
    {
        RessourceClass item = new RessourceClass();
        item.id = Convert.ToUInt16(id);

        if (File.Exists(Application.persistentDataPath + "/save.json"))
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(Application.persistentDataPath + "/save.json");

            // Deserialize the JSON data 
            // into a pattern matching the PlayerData class.
            PlayerClass player = JsonUtility.FromJson<PlayerClass>(fileContents);
            player.inventory.ressources.Add(item);
            //Save json
            //NetworkManager network = new NetworkManager();
            string json = JsonUtility.ToJson(player);

            File.WriteAllText(Application.persistentDataPath + "/save.json", json);
        }
    }

    public void addSKills(int id)
    {
        SkillClass item = new SkillClass();
        item.invid = id;

        if (File.Exists(Application.persistentDataPath + "/save.json"))
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(Application.persistentDataPath + "/save.json");

            // Deserialize the JSON data 
            // into a pattern matching the PlayerData class.
            PlayerClass player = JsonUtility.FromJson<PlayerClass>(fileContents);
            player.inventory.skills.Add(item);
            //Save json
            //NetworkManager network = new NetworkManager();
            string json = JsonUtility.ToJson(player);

            File.WriteAllText(Application.persistentDataPath + "/save.json", json);
        }
    }

    public void loadItem()
    {
        //GameObject[] itemInventory = GameObject.FindGameObjectsWithTag("ItemInventory");
        /*foreach (GameObject item in itemInventory)
        {
            item.SetActive(false);
        }

        if (File.Exists(Application.persistentDataPath + "/save.json"))
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(Application.persistentDataPath + "/save.json");

            // Deserialize the JSON data 
            // into a pattern matching the PlayerData class.
            PlayerClass player = JsonUtility.FromJson<PlayerClass>(fileContents);

            // Load islands from save
            foreach (RessourceClass item in player.inventory.ressources ) {
                if (itemInventory[item.id -1 ].activeSelf == false)
                {
                    Debug.Log(itemInventory.Length);
                    itemInventory[item.id - 1].SetActive(true);
                }
            }
        }*/
    }

    public void loadSkills()
    {
       /* GameObject[] SkillsInventory = GameObject.FindGameObjectsWithTag("SkillsInventory");
        foreach (GameObject skill in SkillsInventory)
        {
            skill.SetActive(false);
        }
        if (File.Exists(Application.persistentDataPath + "/save.json"))
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(Application.persistentDataPath + "/save.json");

            // Deserialize the JSON data 
            // into a pattern matching the PlayerData class.
            PlayerClass player = JsonUtility.FromJson<PlayerClass>(fileContents);

            // Load islands from save
            foreach (SkillClass item in player.inventory.skills)
            {
                if (SkillsInventory[item.invid - 1].activeSelf == false)
                {
                    Debug.Log(SkillsInventory.Length);
                    SkillsInventory[item.invid - 1].SetActive(true);
                }
            }
        }*/
    }
}
