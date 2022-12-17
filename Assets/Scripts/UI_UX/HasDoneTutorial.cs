using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class HasDoneTutorial : MonoBehaviour
{
    // Start is called before the first frame update
    
    public GameObject loadingScreen;
    public Slider slider;
    
    void Start()
    {
        // Read the entire file and save its contents.
        string fileContents = File.ReadAllText(Application.persistentDataPath + "/save.json");

        // Deserialize the JSON data 
        // into a pattern matching the PlayerData class.
        PlayerClass player = JsonUtility.FromJson<PlayerClass>(fileContents);
        
        if (!player.hasDoneMainTutorial)
        {
            player.hasDoneMainTutorial = true;
            
            string json = JsonUtility.ToJson(player);
            File.WriteAllText(Application.persistentDataPath + "/save.json", json);
            StartCoroutine(LoadAsynchronously("TutoMenu"));
            
        }
    }

    IEnumerator LoadAsynchronously(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            yield return null;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
