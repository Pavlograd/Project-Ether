using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class itemSlot : MonoBehaviour
{
    public int id;
    public Text price;
    public Text printPrice;

    // Start is called before the first frame update
    void Start()
    {
        loadShopItem();
        changePrice();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addItemSlot()
    {
        PublicShopClass item = new PublicShopClass();
        item.id = Convert.ToUInt16(id);
        item.price = Int32.Parse(price.text);


        if (File.Exists(Application.persistentDataPath + "/save.json"))
        {
            Debug.Log("test 2");
            Debug.Log(item.price);
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(Application.persistentDataPath + "/save.json");

            // Deserialize the JSON data 
            // into a pattern matching the PlayerData class.
            PlayerClass player = JsonUtility.FromJson<PlayerClass>(fileContents);
            bool stop = false;
            foreach (PublicShopClass items in player.publicShop)
            {
                if (items.id == id)
                {
                    stop = true;
                }
            }
            if (stop == false)
            {
                player.publicShop.Add(item);
            }
            //Save json
            //NetworkManager network = new NetworkManager();
            string json = JsonUtility.ToJson(player);

            File.WriteAllText(Application.persistentDataPath + "/save.json", json);
        }
    }

    public void loadShopItem()
    {
        GameObject[] itemInventory = GameObject.FindGameObjectsWithTag("CommunityShopItem");
        foreach (GameObject item in itemInventory)
        {
            item.SetActive(false);
            Debug.Log(item.activeSelf);
        }

        if (File.Exists(Application.persistentDataPath + "/save.json"))
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(Application.persistentDataPath + "/save.json");

            // Deserialize the JSON data 
            // into a pattern matching the PlayerData class.
            PlayerClass player = JsonUtility.FromJson<PlayerClass>(fileContents);

            // Load islands from save
            foreach (PublicShopClass item in player.publicShop) {
                if (itemInventory[item.id].activeSelf == false)
                {
                    itemInventory[item.id].SetActive(true);                
                }
            }
        }
    }

    public void changePrice()
    {
        if (File.Exists(Application.persistentDataPath + "/save.json"))
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(Application.persistentDataPath + "/save.json");

            // Deserialize the JSON data 
            // into a pattern matching the PlayerData class.
            PlayerClass player = JsonUtility.FromJson<PlayerClass>(fileContents);

            // Load islands from save
            if((player.publicShop.Count - 1) > id ) { 
                printPrice.text = player.publicShop[id].price.ToString();
            }
        }
        
    }

    public void buyItem()
    {
  
        
        if (File.Exists(Application.persistentDataPath + "/save.json"))
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(Application.persistentDataPath + "/save.json");

            // Deserialize the JSON data 
            // into a pattern matching the PlayerData class.
            PlayerClass player = JsonUtility.FromJson<PlayerClass>(fileContents);

            // Load islands from save
            
            if (100 > player.mentoring)
            {
                return;
            }
            player.mentoring = player.mentoring - 100;
            List<PublicShopClass> publicShop = new List<PublicShopClass>();
            foreach (PublicShopClass item in player.publicShop)
            {
                if (item.id != id)
                {
                    publicShop.Add(item);
                }
            }
            player.publicShop = publicShop;
            string json = JsonUtility.ToJson(player);
            File.WriteAllText(Application.persistentDataPath + "/save.json", json);

        }
    }
}
