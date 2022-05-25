using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class API : MonoBehaviour
{
    static string baseURL = "http://projectether.francecentral.cloudapp.azure.com/";

    static string GetToken() // In case the token is not in CrossSceneInfos anymore
    {
        //string testToken = "1e46a710f772726839049886fd8f7b9261e5c105"; // Write your token here if you want to test withtout connection

        return CrossSceneInfos.token; // PRODUCTION CHANGE TO CrossSceneInfos.token
    }

    static string GetUserId()
    {
        return CrossSceneInfos.userid;
    }

    static string CheckId(string id)
    {
        return id == "" ? GetUserId() : id;
    }

    static string GetUserName()
    {
        return CrossSceneInfos.username;
    }

    static string Get(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(baseURL + uri)) // Combine baseURL and uri
        {
            webRequest.SetRequestHeader("content-type", "application/json");
            webRequest.SetRequestHeader("Authorization", "Token " + GetToken());
            webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            while (!webRequest.isDone) { } // Wait for api call to be done

            string response = webRequest.downloadHandler.text;

            switch (webRequest.result) // If ERROR then responses is set to null so every call can have the same error management
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.Log(pages[page] + ": Error: " + webRequest.error);
                    response = null;
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.Log(pages[page] + ": HTTP Error: " + webRequest.error);
                    response = null;
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }

            return response;
        }
    }

    static string Post(string uri, string jsonData)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Post(baseURL + uri, jsonData)) // Combine baseURL and uri
        {
            webRequest.SetRequestHeader("content-type", "application/json");
            webRequest.SetRequestHeader("Authorization", "Token " + GetToken());
            webRequest.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.UTF8.GetBytes(jsonData));
            webRequest.uploadHandler.contentType = "application/json";
            webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            while (!webRequest.isDone) { } // Wait for api call to be done

            string response = webRequest.downloadHandler.text;

            switch (webRequest.result) // If ERROR then responses is set to null so every call can have the same error management
            {
                case UnityWebRequest.Result.ConnectionError:
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.Log(pages[page] + ": Error: " + webRequest.error);
                    response = null;
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.Log(pages[page] + ": HTTP Error: " + webRequest.error);
                    response = null;
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }

            return response;
        }
    }


    // ALL API CALLS ARE BELOW
    // The foramt of the uri must be everything after the the base url : http://projectether.francecentral.cloudapp.azure.com/api/{uri}
    // No coroutines on this side it needs to be in your scripts directly
    // And yes a coroutine is necessary or the game can crash or freeze
    // To see exemple of functions using the API check TestsAPI.cs


    public static string GetUserData()
    {
        return Get("api/users_data");
    }

    public static API_User_Textures GetUserTextures(string username)
    {
        string response = Get("api/game/users_texture/?username=" + username);

        Debug.Log(response.Length);
        Debug.Log(response);

        if (response == null) return null;

        return JsonUtility.FromJson<API_User_Textures>('{' + "\"" + "textures" + "\"" + ":" + response + '}');
    }

    public static bool PostTexture(string texture)
    {
        API_User_Texture objectTexture = new API_User_Texture();

        objectTexture.texture = texture;

        string jsonData = JsonUtility.ToJson(objectTexture);

        Debug.Log(jsonData);

        string response = Post("api/game/users_texture/", jsonData);

        return response != null;
    }

    public static API_User GetUser() // Doesn't need id
    {
        string response = Get("auth/users/me/");

        Debug.Log(response);

        if (response == null) return null;

        return JsonUtility.FromJson<API_User>(response);
    }

    public static API_User GetUserById(string id = "")
    {
        id = CheckId(id);

        string response = Get("auth/users/" + id);

        Debug.Log(response);

        if (response == null) return null;

        return JsonUtility.FromJson<API_User>(response);
    }

    public static API_User GetUserSkills(string id = "")
    {
        id = CheckId(id);

        string response = Get("api/users_skills/" + id);

        return null;
    }

    public static bool PostSkill(Ability skill)
    {
        API_skill objectSkill = new API_skill();

        objectSkill._id = skill.id.ToString();
        objectSkill._parentId = skill.parentId.ToString();
        objectSkill.name = skill.name;
        objectSkill.level = skill.lvl.ToString();
        objectSkill.equipped = skill.geared.ToString();

        string jsonData = JsonUtility.ToJson(objectSkill);

        Debug.Log(jsonData);

        string response = Post("api/users_skills/", jsonData);

        return response != null;
    }

    public static API_skills GetSkills()
    {
        string response = Get("api/users_skills/");
        //Debug.Log(response);

        if (response == null) return null;

        return (JsonUtility.FromJson<API_skills>('{' + "\"" + "skills" + "\"" + ":" + response + '}'));
    }

    public static bool PostInventory(int id, string name, int quantity)
    {
        API_inventory objectInventory = new API_inventory();

        objectInventory._id = id.ToString();
        objectInventory.name = name;
        objectInventory.quantity = quantity.ToString();
        objectInventory.comment = name;

        string jsonData = JsonUtility.ToJson(objectInventory);

        Debug.Log(jsonData);

        string response = Post("api/users_inventory/", jsonData);

        return response != null;
    }

    public static API_inventories GetInventory()
    {
        string response = Get("api/users_inventory/");
        //Debug.Log(response);

        if (response == null) return null;

        return (JsonUtility.FromJson<API_inventories>('{' + "\"" + "inventories" + "\"" + ":" + response + '}'));
    }
}
