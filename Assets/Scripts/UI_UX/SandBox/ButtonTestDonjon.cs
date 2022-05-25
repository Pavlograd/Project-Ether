using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ButtonTestDonjon : MonoBehaviour
{
    [SerializeField] Image image;

    // Start is called before the first frame update
    void Start()
    {
        // Read the entire file and save its contents.
        string fileContents = File.ReadAllText(Application.persistentDataPath + "/save.json");

        // Deserialize the JSON data 
        // into a pattern matching the PlayerData class.
        PlayerClass player = JsonUtility.FromJson<PlayerClass>(fileContents);

        image.color = (player.donjon.tested) ? Color.green : Color.red;
    }

    public void TestDonjon()
    {
        CrossSceneInfos.testDonjon = true;
        SceneManager.LoadScene("Donjon");
    }
}
