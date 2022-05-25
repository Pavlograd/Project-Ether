using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;


public class skillsSave : MonoBehaviour
{
    private const string one  = "Assets/Scripts/Skills/PowerBeam.asset";
    private const string two  = "Assets/Scripts/Skills/SparkPower.asset";
    private const string three  = "Assets/Scripts/Skills/SpeedBuff.asset";
    private const string four = "Assets/Scripts/Skills/StrenghtBuff.asset";
    //public AttackPowers ActualPlayer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void saveFirstGear(string id)
    {
        if (File.Exists(Application.persistentDataPath + "/save.json"))
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(Application.persistentDataPath + "/save.json");

            // Deserialize the JSON data 
            // into a pattern matching the PlayerData class.
            PlayerClass player = JsonUtility.FromJson<PlayerClass>(fileContents);

            if (player.gear.skills.Count < 4)
            {
                while (player.gear.skills.Count < 4) {
                    player.gear.skills.Add(new SkillClass());
                }
            }

            player.gear.skills[0].id = id;
            //Save json
            //NetworkManager network = new NetworkManager();
            string json = JsonUtility.ToJson(player);

            Debug.Log(Application.persistentDataPath);
            File.WriteAllText(Application.persistentDataPath + "/save.json", json);
        }
        
    }

    public void saveSecondGear(string id)
    {
        if (File.Exists(Application.persistentDataPath + "/save.json"))
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(Application.persistentDataPath + "/save.json");

            // Deserialize the JSON data 
            // into a pattern matching the PlayerData class.
            PlayerClass player = JsonUtility.FromJson<PlayerClass>(fileContents);

            if (player.gear.skills.Count < 4)
            {
                while (player.gear.skills.Count < 4)
                {
                    player.gear.skills.Add(new SkillClass());
                }
            }

            player.gear.skills[1].id = id;
            //Save json
            //NetworkManager network = new NetworkManager();
            string json = JsonUtility.ToJson(player);

            Debug.Log(Application.persistentDataPath);
            File.WriteAllText(Application.persistentDataPath + "/save.json", json);
        }

    }

    public void saveBuff(string id)
    {
        if (File.Exists(Application.persistentDataPath + "/save.json"))
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(Application.persistentDataPath + "/save.json");

            // Deserialize the JSON data 
            // into a pattern matching the PlayerData class.
            PlayerClass player = JsonUtility.FromJson<PlayerClass>(fileContents);

            if (player.gear.skills.Count < 4)
            {
                while (player.gear.skills.Count < 4)
                {
                    player.gear.skills.Add(new SkillClass());
                }
            }

            player.gear.skills[3].id = id;
            //Save json
            //NetworkManager network = new NetworkManager();
            string json = JsonUtility.ToJson(player);

            Debug.Log(Application.persistentDataPath);
            File.WriteAllText(Application.persistentDataPath + "/save.json", json);
        }

    }

    public void loadGear()
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

           /* if (player.gear.skills[0].id != "a")
            {
                if (player.gear.skills[0].id == "one")
                {
                    ActualPlayer.replaceMyFirstSpell(AssetDatabase.LoadAssetAtPath<SkillsData>(one));
                }
                if (player.gear.skills[0].id == "two")
                {
                    ActualPlayer.replaceMyFirstSpell(AssetDatabase.LoadAssetAtPath<SkillsData>(two));
                }
            }

            if (player.gear.skills[1].id != "a")
            {
                if (player.gear.skills[1].id == "one")
                {
                    ActualPlayer.replaceMySecondSpell(AssetDatabase.LoadAssetAtPath<SkillsData>(one));
                }
                if (player.gear.skills[1].id == "two")
                {
                    ActualPlayer.replaceMySecondSpell(AssetDatabase.LoadAssetAtPath<SkillsData>(two));
                }
            }

            if (player.gear.skills[3].id != "a")
            {
                if (player.gear.skills[3].id == "three")
                {
                    ActualPlayer.replaceMyBuffSpell(AssetDatabase.LoadAssetAtPath<BuffersData>(three));
                }
                if (player.gear.skills[3].id == "four")
                {
                    ActualPlayer.replaceMyBuffSpell(AssetDatabase.LoadAssetAtPath<BuffersData>(four));
                }
            }*/
        }
    }
}
