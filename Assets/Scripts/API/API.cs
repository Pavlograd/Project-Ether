using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class API : MonoBehaviour
{
    static string baseURL = "http://projectether.francecentral.cloudapp.azure.com/";

    static string GetToken() // In case the token is not in CrossSceneInfos anymore
    {
        // string testToken = "1e46a710f772726839049886fd8f7b9261e5c105"; // Write your token here if you want to test withtout connection

#if UNITY_EDITOR
        // return testToken;
        return MyTestToken.token; // DEV MODE CrossSceneInfos.token
#else
        return CrossSceneInfos.token; // PRODUCTION CHANGE TO CrossSceneInfos.token
#endif
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

            while (!webRequest.isDone)
            {
            } // Wait for api call to be done

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
                    //Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
                    break;
            }

            webRequest.Dispose();
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

            while (!webRequest.isDone)
            {
            } // Wait for api call to be done

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

            webRequest.Dispose();
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

        //Debug.Log(response.Length);
        //Debug.Log(response);

        if (response == null) return null;

        return JsonUtility.FromJson<API_User_Textures>('{' + "\"" + "textures" + "\"" + ":" + response + '}');
    }

    public static bool PostTexture(string id, string texture) // id = "" if new texture
    {
        API_User_Texture objectTexture = new API_User_Texture();
        string response = null;
        string jsonData = null;

        objectTexture.id = id;
        objectTexture.texture = texture;

        if (id == "")
        {
            Debug.Log("New texture");
            API_User_NewTexture newTexture = new API_User_NewTexture();

            newTexture.texture = texture;
            jsonData = JsonUtility.ToJson(newTexture);
        }
        else
        {
            jsonData = JsonUtility.ToJson(objectTexture);
        }

        response = Post("api/game/users_texture/", jsonData);

        return response != null;
    }

    public static API_User GetUser() // Doesn't need id
    {
        string response = Get("auth/users/me/");

        Debug.Log(response);

        if (response == null) return null;

        return JsonUtility.FromJson<API_User>(response);
    }

    public static API_Users GetUsers() // Return ALL Users (But we will never have >100 people so we don't care)
    {
        string response = Get("api/users/");

        if (response == null) return null;

        return JsonUtility.FromJson<API_Users>('{' + "\"" + "users" + "\"" + ":" + response + '}');
    }

    public static API_User_Datas GetUserDatas() // Doesn't need id
    {
        string response = Get("api/users_data/");
        response = response.Replace("[", "").Replace("]", "");

        if (response == null) return null;

        return (JsonUtility.FromJson<API_User_Datas>(response));
    }

    public static API_User GetUserById(string id = "")
    {
        id = CheckId(id);

        string response = Get("auth/users/" + id);

        Debug.Log(response);

        if (response == null) return null;

        return JsonUtility.FromJson<API_User>(response);
    }


    // Donjon


    public static API_Donjon GetUserDonjon(string username = "")
    {
        string response = Get("api/users_dungeons?username=" + username);
        response = response.Substring(1, response.Length - 2);
        ;

        if (response == null) return null;

        return JsonUtility.FromJson<API_Donjon>(response);
    }

    public static bool PostUserDonjon(string data = "")
    {
        API_Donjon objectDonjon = new API_Donjon();

        objectDonjon.data = data;

        string jsonData = JsonUtility.ToJson(objectDonjon);

        Debug.Log(jsonData);

        string response = Post("api/users_dungeons/", jsonData);

        return response != null;
    }


    // Skills


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
        if (quantity < 0)
        {
            quantity = 0;
        }

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

    public static API_Incomes GetLastIncomes()
    {
        string response = Get("api/users_data/");
        response = response.Replace("[", "").Replace("]", "");

        if (response == null) return null;

        return (JsonUtility.FromJson<API_Incomes>(response));
    }

    public static bool RemoveCash(int amount)
    {
        API_User_Datas objectUserDatas = GetUserDatas();

        objectUserDatas.cash -= amount;
        string jsonData = JsonUtility.ToJson(objectUserDatas);
        string response = Post("api/users_data/", jsonData);
        return response != null;
    }


    public static bool AddCash(int amount)
    {
        API_User_Datas objectUserDatas = GetUserDatas();

        objectUserDatas.cash += amount;

        string jsonData = JsonUtility.ToJson(objectUserDatas);

        string response = Post("api/users_data/", jsonData);

        return response != null;
    }

    public static bool PostIncomes(string date, int amount)
    {
        API_User_Datas objectIncomes = GetUserDatas();

        objectIncomes.name = GetUserName();
        objectIncomes.passif = date;
        objectIncomes.cash += amount;

        string jsonData = JsonUtility.ToJson(objectIncomes);

        string response = Post("api/users_data/", jsonData);

        return response != null;
    }

    public static API_ShopTextures GetShopTextures()
    {
        string response = Get("api/shop/textures/");

        Debug.Log(response.Length);
        Debug.Log(response);

        if (response == null) return null;

        return JsonUtility.FromJson<API_ShopTextures>('{' + "\"" + "shopTextures" + "\"" + ":" + response + '}');
    }

    public static bool PostShopTextures(API_ShopTextureToSell item)
    {
        string jsonData = JsonUtility.ToJson(item);

        Debug.Log(jsonData);

        string response = Post("api/shop/textures/", jsonData);

        return response != null;
    }

    public static PlayerClass GetUserDataForSave()
    {
        string response = GetUserData();
        API_User_Datas savedData = JsonUtility.FromJson<API_User_Datas>('{' + "\"" + "datas" + "\"" + ":" + response + '}');


        PlayerClass player = new PlayerClass();
        player.id = System.Convert.ToUInt32(GetUserId());
        player.name = savedData.name;
        player.level = System.Convert.ToUInt16(savedData.level);
        // player.skinId = savedData.skinId;
        player.crystal = savedData.crystal;
        player.cash = savedData.cash;
        player.mentoring = savedData.mentoring;
        player.textureSlot = savedData.textureSlot;
        player.maxTextureSlot = savedData.maxTextureSlot;
        player.hasDoneTutorial = savedData.hasDoneTutorial;
        return player;
    }

    public static bool PostTutorial()
    {
        string response = GetUserData();
        API_User_Datas savedData = JsonUtility.FromJson<API_User_Datas>(response.Replace("[", "").Replace("]", ""));

        API_User_Datas player = new API_User_Datas();

        player.name = savedData.name;
        player.level = savedData.level;
        player.crystal = savedData.crystal;
        player.cash = savedData.cash;
        player.mentoring = savedData.mentoring;
        player.textureSlot = savedData.textureSlot;
        player.maxTextureSlot = savedData.maxTextureSlot;
        player.hasDoneTutorial = true;

        string jsonData = JsonUtility.ToJson(player);

        Debug.Log(jsonData);

        string response2 = Post("api/users_data/", jsonData);

        PlayerClass player2 = new PlayerClass();
        player2.id = System.Convert.ToUInt32(GetUserId());
        player2.name = savedData.name;
        player2.level = System.Convert.ToUInt16(savedData.level);
        player2.crystal = savedData.crystal;
        player2.cash = savedData.cash;
        player2.mentoring = savedData.mentoring;
        player2.textureSlot = savedData.textureSlot;
        player2.maxTextureSlot = savedData.maxTextureSlot;
        player2.hasDoneTutorial = true;
        player2.hasDoneMainTutorial = true;
        string json = JsonUtility.ToJson(player2);
        File.WriteAllText(Application.persistentDataPath + "/save.json", json);

        return response != null;
    }

    public static bool SaveCrystal(int amount)
    {
        API_User_Datas objectUserDatas = GetUserDatas();

        objectUserDatas.crystal = amount;
        string jsonData = JsonUtility.ToJson(objectUserDatas);
        string response = Post("api/users_data/", jsonData);

        return response != null;
    }

    public static bool SaveCash(int amount)
    {
        API_User_Datas objectUserDatas = GetUserDatas();

        objectUserDatas.cash = amount;
        string jsonData = JsonUtility.ToJson(objectUserDatas);
        string response = Post("api/users_data/", jsonData);

        return response != null;
    }


    public static bool SaveMentoring(int amount)
    {
        API_User_Datas objectUserDatas = GetUserDatas();

        objectUserDatas.mentoring = amount;

        string jsonData = JsonUtility.ToJson(objectUserDatas);

        string response = Post("api/users_data/", jsonData);

        return response != null;
    }


    // ENERGY FUNCTIONS

    // Check current energy and calculate with current date
    public static void CheckEnergy()
    {
        EnergyClass energy = new EnergyClass();

        if (File.Exists(Application.persistentDataPath + "/energy.json"))
        {
            // Get values from file
            string fileContents = File.ReadAllText(Application.persistentDataPath + "/energy.json");
            energy = JsonUtility.FromJson<EnergyClass>(fileContents);
        }
        else
        {
            // Set basic value
            energy.date = System.DateTime.UtcNow.ToString();
        }

        System.DateTime dateTime = System.DateTime.Parse(energy.date);
        System.TimeSpan ts = System.DateTime.UtcNow - dateTime;

        int secondsSinceLastCheck = (int)(ts.TotalSeconds);

        energy.energy += (uint)secondsSinceLastCheck / 60; //One energy per minute 
        energy.energy = energy.energy >= 100 ? 100 : energy.energy;

        // Reset date now
        energy.date = System.DateTime.UtcNow.ToString();

        // Save energy file
        string jsonData = JsonUtility.ToJson(energy);
        File.WriteAllText(Application.persistentDataPath + "/energy.json", jsonData);
    }

    public static uint GetEnergy()
    {
        CheckEnergy();

        string fileContents = File.ReadAllText(Application.persistentDataPath + "/energy.json");
        EnergyClass energy = JsonUtility.FromJson<EnergyClass>(fileContents);

        return energy.energy;
    }

    public static bool RemoveEnergy(int amount = 10)
    {
        CheckEnergy();

        string fileContents = File.ReadAllText(Application.persistentDataPath + "/energy.json");
        EnergyClass energy = JsonUtility.FromJson<EnergyClass>(fileContents);

        if (energy.energy - (uint)amount < 0) return false;

        energy.energy -= (uint)amount;

        string jsonData = JsonUtility.ToJson(energy);
        File.WriteAllText(Application.persistentDataPath + "/energy.json", jsonData);

        return true;
    }

    public static bool PostBuyedShopTextures(API_ShopTexture item)
    {
        string jsonData = JsonUtility.ToJson(item);

        Debug.Log(jsonData);

        string response = Post("api/shop/textures/", jsonData);

        return response != null;
    }

    public static bool PostMaxTextureSlot()
    {
        API_User_Datas objectUserDatas = GetUserDatas();

        objectUserDatas.maxTextureSlot = objectUserDatas.maxTextureSlot + 1;

        string jsonData = JsonUtility.ToJson(objectUserDatas);

        string response = Post("api/users_data/", jsonData);

        return response != null;
    }

    public static bool PostTextureSlot()
    {
        API_User_Datas objectUserDatas = GetUserDatas();

        objectUserDatas.textureSlot = objectUserDatas.textureSlot + 1;

        string jsonData = JsonUtility.ToJson(objectUserDatas);

        string response = Post("api/users_data/", jsonData);

        return response != null;
    }

    public static int GetMaxTextureSlot()
    {
        API_User_Datas objectUserDatas = GetUserDatas();

        return objectUserDatas.maxTextureSlot + 1;
    }

    public static int GetTextureSlot()
    {
        API_User_Datas objectUserDatas = GetUserDatas();

        return objectUserDatas.textureSlot;
    }

    public static API_FriendsList AddRemoveFriend(string id)
    {
        API_FriendToAdd friendToAdd = new API_FriendToAdd();
        if (id != "")
            friendToAdd.friends.Add("http://projectether.francecentral.cloudapp.azure.com/api/users/" + id + "/");
        string jsonData = JsonUtility.ToJson(friendToAdd);
        string response = Post("api/users_friends/", jsonData);

        //Debug.Log(response.Length);
        // Debug.Log(response);

        if (response == null) return null;
        return JsonUtility.FromJson<API_FriendsList>(response);
    }

    public static bool SetPlayerSkinId(string id)
    {
        API_User_Datas objectUserDatas = GetUserDatas();

        string jsonData = "{\"name\": \"" + objectUserDatas.name + "\",\"skinId\": " + id + "}";
        string response = Post("api/users_data/", jsonData);

        return response != null;
    }

    public static ShopListClass getShop()
    {
        API_User_Datas objectUserDatas = GetUserDatas();

        string response = Get("api/shops/?username=" + objectUserDatas.name);
        response = response.Substring(1, response.Length - 2);
        Debug.Log(response);
        string[] words = response.Split("data\":");

        string data = words[1].Remove(words[1].Length - 1);


        ShopListClass ShopList;
        ShopList = JsonUtility.FromJson<ShopListClass>(data);


        return ShopList;
    }

    public static void postShop(ShopListClass shopItems)
    {
        string jsonData = JsonUtility.ToJson(shopItems);
        jsonData = '{' + "\"" + "data\":" + jsonData + '}';
        Debug.Log(jsonData);
        string response = Post("api/shops/", jsonData);
    }
}