using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Token : MonoBehaviour
{
    static string path = Application.persistentDataPath + "/token.json";

    public static void SaveToken(string token)
    {
        API_Token ObjectToken = new API_Token();

        ObjectToken.token = token;

        SetStaticValues(token);

        string json = JsonUtility.ToJson(ObjectToken);

        File.WriteAllText(path, json);

        Debug.Log("Token file created");
    }

    public static void SetStaticValues(string token)
    {
        CrossSceneInfos.token = token; // Set Token for every scenes
        CrossSceneInfos.username = API.GetUser().username; // Set Username for every scenes
        CrossSceneInfos.userid = API.GetUser().id; // Set UserId for every scenes
    }

    public static void CreateSave()
    {
        // Create save for player
        string pathToSave = Application.persistentDataPath + "/save.json";
        API_User ObjectUser = API.GetUser();
        PlayerClass player = new PlayerClass();

        CrossSceneInfos.username = ObjectUser.username;

        player.name = ObjectUser.username;
        player.id = uint.Parse(ObjectUser.id);

        string json = JsonUtility.ToJson(player);
        File.WriteAllText(pathToSave, json);
    }

    public static string GetToken()
    {
        if (File.Exists(path))
        {
            string fileContents = File.ReadAllText(path);

            API_Token ObjectToken = JsonUtility.FromJson<API_Token>(fileContents);

            return ObjectToken.token;
        }
        return null;
    }
}
