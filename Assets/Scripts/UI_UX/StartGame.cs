using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class StartGame : MonoBehaviour
{
    [SerializeField] LevelLoader levelLoader;

    // Start is called before the first frame update
    void Start()
    {
        // Check if user is connected
        // If connect user go to main menu
        // Else they go to login screen

        string token = Token.GetToken();

        if (token != null) // Save token for session
        {

            Token.SetStaticValues(token);
            CrossSceneInfos.token = token;
            CrossSceneInfos.username = API.GetUser().username;
            initShop();

        }
        levelLoader.LoadLevel(token == null ? "LoginScreen" : "Main Menu");
    }

    void initShop()
    {
       /* if (File.Exists(Application.persistentDataPath + "/ShopItem.json")) {
            Debug.Log("file exist");
        } else {
            ShopListClass save = new ShopListClass();
            save.items.Add(addItemToShop(0, "Iron", 0, 150));
            save.items.Add(addItemToShop(1, "Cobalt", 1, 150));
            save.items.Add(addItemToShop(2, "Bronze", 0, 150));
            save.textures.Add(addTexturesToShop(0, "Basic Floor", 1, 1000));

            string json = JsonUtility.ToJson(save);
            File.WriteAllText(Application.persistentDataPath + "/ShopItem.json", json);
        }*/
    }

    ShopItemClass addItemToShop(uint id, string name, int moneyType, int price)
    {
        ShopItemClass Item = new ShopItemClass();

        Item.id = id;
        Item.name = name;
        Item.price = price;
        Item.quantity = 10;

        return (Item);
    }

    ShopTextureClass addTexturesToShop(uint id, string name, int moneyType, int price)
    {
        ShopTextureClass Item = new ShopTextureClass();

        Item.id = id;
        Item.name = name;
        Item.price = price;
        Item.quantity = 1;

        return (Item);
    }
}
