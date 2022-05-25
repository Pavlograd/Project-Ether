using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class MoneyInventory : MonoBehaviour
{
    public int crystalCount;
    //public string name;
    public Image itemImage;
    public Text itemName;
    public Sprite emptyItem;
    public Text crystalCountText;
    public int type;

    public static MoneyInventory instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Error: use only one inventory instance in the scene.");
            return;
        }
        instance = this;
    }

    private void Start()
    {
        itemImage.sprite = emptyItem;
        if (type == 1)
        {
            itemName.text = "crystal";
        } else if (type == 2) {
            itemName.text = "cash";
        } else
        {
            itemName.text = "mentoring";
        }
        crystalCountText.text = crystalCount.ToString();
        setCrystal();
        crystalCountText.text = crystalCount.ToString();
    }

    public void AddCrystal(int count)
    {
        crystalCount += count;
        crystalCountText.text = crystalCount.ToString();
    }

    public void removeCrystal(int count)
    {
        if (count > crystalCount)
        {
            return;
        }
        crystalCount -= count;
        crystalCountText.text = crystalCount.ToString();
    }

    public void setCrystal()
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
            crystalCountText.text = player.crystal.ToString();
            if (type == 1)
            {
                crystalCount = player.crystal;
            }
            else if (type == 2)
            {
                crystalCount = player.cash;
            }
            else
            {
                crystalCount = player.mentoring;
            }
        }
    }

    public void saveCrystal()
    {
        // Does the file exist?
        if (gameObject.activeInHierarchy == true) {
            StartCoroutine(GetSave("http://projectether.francecentral.cloudapp.azure.com/api/users_data/", "1e46a710f772726839049886fd8f7b9261e5c105"));
        }
        if (File.Exists(Application.persistentDataPath + "/save.json"))
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(Application.persistentDataPath + "/save.json");

            // Deserialize the JSON data
            // into a pattern matching the PlayerData class.
            PlayerClass player = JsonUtility.FromJson<PlayerClass>(fileContents);

            // Load islands from save
            if (type == 1)
            {
                player.crystal = crystalCount;
            }
            else if (type == 2)
            {
                player.cash = crystalCount;
            }
            else
            {
                player.mentoring = crystalCount;
            }

            //Save json
            //NetworkManager network = new NetworkManager();
            string json = JsonUtility.ToJson(player);

            Debug.Log(Application.persistentDataPath);
            File.WriteAllText(Application.persistentDataPath + "/save.json", json);
            if (gameObject.activeInHierarchy == true) {
                StartCoroutine(PostSave("http://projectether.francecentral.cloudapp.azure.com/api/users_data/", "1e46a710f772726839049886fd8f7b9261e5c105", player.level, player.crystal, player.cash, player.mentoring, player.textureSlot, player.maxTextureSlot, player.hasDoneTutorial));
            }
        }
    }

    IEnumerator GetSave(string uri, string token)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            webRequest.SetRequestHeader("content-type", "application/json");
            webRequest.SetRequestHeader("Authorization", "Token " + token);
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.Log(pages[page] + ": Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.Log(pages[page] + ": HTTP Error: " + webRequest.error);
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }
        }
    }

    IEnumerator PostSave(string uri, string token, int level, int crystal, int cash, int mentoring, int textureSlot, int maxTextureSlot, bool hasDoneTutorial)
    {
        Save save = new Save();
        save.level = level;
        save.crystal = crystal;
        save.cash = cash;
        save.mentoring = mentoring;
        save.textureSlot = textureSlot;
        save.maxTextureSlot = maxTextureSlot;
        save.hasDoneTutorial = hasDoneTutorial;
        string jsonData = JsonUtility.ToJson(save);
        
        using (UnityWebRequest www = UnityWebRequest.Post(uri, jsonData))
        {
            www.SetRequestHeader("content-type", "application/json");
            www.SetRequestHeader("Authorization", "Token " + token);
            www.uploadHandler.contentType = "application/json";
            www.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError) {
                Debug.Log(www.error);
            } else {
                if (www.isDone) {
                    Debug.Log(www.downloadHandler.text);
                    Debug.Log("Saved");
                }
            }
        }
    }

    [System.Serializable]
    public class Save
    {
        public int level;
        public int crystal;
        public int cash;
        public int mentoring;
        public int textureSlot;
        public int maxTextureSlot;
        public bool hasDoneTutorial;
    }
}
