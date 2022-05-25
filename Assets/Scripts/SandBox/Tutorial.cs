using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class Tutorial : MonoBehaviour
{
    [SerializeField] Camera tutorialCamera;
    [SerializeField] Canvas canvas;
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] GameObject room;

    // Start is called before the first frame update
    void Start()
    {
        // Read the entire file and save its contents.
        string fileContents = File.ReadAllText(Application.persistentDataPath + "/save.json");

        // Deserialize the JSON data 
        // into a pattern matching the PlayerData class.
        PlayerClass player = JsonUtility.FromJson<PlayerClass>(fileContents);

        if (!player.hasDoneTutorial)
        {
            canvas.gameObject.SetActive(false);
            room.SetActive(false);

            videoPlayer.Play();

            tutorialCamera.gameObject.SetActive(true);

            player.hasDoneTutorial = true;

            string json = JsonUtility.ToJson(player);
            File.WriteAllText(Application.persistentDataPath + "/save.json", json);
            Invoke("ReloadScene", (float)videoPlayer.clip.length);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ReloadScene()
    {
        SceneManager.LoadScene("SandBox");
    }
}
