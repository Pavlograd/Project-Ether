using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevel : MonoBehaviour
{
    [SerializeField] private LoadRoom roomLoader;

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

        if (File.Exists(Application.persistentDataPath + "/levels.json"))
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(Application.persistentDataPath + "/levels.json");

            File.WriteAllText(Application.persistentDataPath + "/save.json", fileContents);

            // Deserialize the JSON data 
            // into a pattern matching the PlayerData class.
            PlayerClass player = JsonUtility.FromJson<PlayerClass>(fileContents);
            Debug.Log(player.donjon.rooms[0].name);

            for (int i = 0; i < player.donjon.rooms.Count; i++)
            {
                roomLoader.LoadRoomFromSave(i);
                roomLoader.offset += 100;
            }
        }
    }
}
