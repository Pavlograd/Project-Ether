using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// SCRIPT USELESS NOW
// DON'T USE IT
// SCRIPT USELESS NOW
// DON'T USE IT
// SCRIPT USELESS NOW
// DON'T USE IT
// SCRIPT USELESS NOW
// DON'T USE IT
// SCRIPT USELESS NOW
// DON'T USE IT
// USED TO LOAD INFOS FOR MENUS

public class LoadTowerRoom : MonoBehaviour
{
    [SerializeField] private LoadTower roomLoader;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ExampleCoroutine());
    }

    IEnumerator ExampleCoroutine()
    {
        while (!roomLoader.isStarted)
        {
            yield return new WaitForSeconds(0.05f);
        }

        if (File.Exists(Application.persistentDataPath + "/save.json"))
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(Application.persistentDataPath + "/save.json");

            // Deserialize the JSON data 
            // into a pattern matching the PlayerData class.
            PlayerClass player = JsonUtility.FromJson<PlayerClass>(fileContents);

            for (int i = 0; i < player.donjon.rooms.Count; i++)
            {
                roomLoader.LoadTowerFromSave();
                roomLoader.offset += 100;
            }
        }
    }
}
