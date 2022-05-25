using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomDeletor : MonoBehaviour
{
    private LoadRoom _roomLoader;

    // Start is called before the first frame update
    void Start()
    {
        _roomLoader = GameObject.Find("Load").GetComponent<LoadRoom>();
    }

    public void DeleteRoom()
    {
        string fileContents = File.ReadAllText(Application.persistentDataPath + "/save.json");
        PlayerClass player = JsonUtility.FromJson<PlayerClass>(fileContents);

        player.donjon.rooms.RemoveAt(_roomLoader.activeRoom);
        player.donjon.tested = false;

        string json = JsonUtility.ToJson(player);
        File.WriteAllText(Application.persistentDataPath + "/save.json", json);

        // Reload Scene to be sure
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
