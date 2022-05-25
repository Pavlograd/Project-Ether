using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class sellItem : MonoBehaviour
{
    public InputField price = null;
    public uint id = 0;

    private ComShopListClass ShopList;
    // Start is called before the first frame update
    void Start()
    {
        if (File.Exists(Application.persistentDataPath + "/ComShopItem.json"))
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(Application.persistentDataPath + "/ComShopItem.json");

            // Deserialize the JSON data 
            // into a pattern matching the PlayerData class.
            ShopList = JsonUtility.FromJson<ComShopListClass>(fileContents);
        }
    }

    private uint getId()
    {
        if (File.Exists(Application.persistentDataPath + "/save.json"))
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(Application.persistentDataPath + "/save.json");

            // Deserialize the JSON data
            // into a pattern matching the PlayerData class.
            PlayerClass player = JsonUtility.FromJson<PlayerClass>(fileContents);
            return player.id;
        }
        return 0;
    }

    public void buy()
    {
        ComShopItemClass Item = new ComShopItemClass();

        if (File.Exists(Application.persistentDataPath + "/ComShopItem.json"))
        {
            string jsonShop = JsonUtility.ToJson(ShopList);
            ShopList = JsonUtility.FromJson<ComShopListClass>(jsonShop);


            Item.id = id;
            Item.name = "toto";

            Item.price = Int16.Parse(price.text);
            Item.quantity = 1;
            Item.ownerID = getId();
            //Debug.Log(Application.persistentDataPath);
            ShopList.items.Add(Item);
            string json = JsonUtility.ToJson(ShopList);
            File.WriteAllText(Application.persistentDataPath + "/ComShopItem.json", json);
        }
    }
}
