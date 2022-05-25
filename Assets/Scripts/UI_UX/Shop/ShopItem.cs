using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ShopItem : MonoBehaviour
{
    public GameObject prefab = null;
    public GameObject Crystal = null;
    public GameObject Cash = null;
    public GameObject Mentoring = null;
    public int type = 0;

    private ShopListClass ShopList;

    public GameObject father;

    public Texture2D textureToSend;

    // Start is called before the first frame update
    void Start()
    {
        if (File.Exists(Application.persistentDataPath + "/ShopItem.json" ) && prefab != null)
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(Application.persistentDataPath + "/ShopItem.json");

            // Deserialize the JSON data 
            // into a pattern matching the PlayerData class.
            ShopList = JsonUtility.FromJson<ShopListClass>(fileContents);
            setUpItem();
        } else
        {
            Debug.Log("ShopItem.Json not found");
        }
        
    }

    private void setUpItem()
    {
        int i = 0;
        if (type == 1) {
            foreach (ShopItemClass item in ShopList.items)
            {
                if (item.quantity > 0)
                {
                    Vector3 pos = new Vector3(100.0f, 16.0f, 0.0f); ;
                    pos.x = pos.x + (175 * i);
                    i = i + 1;
                    GameObject newShopItem = Instantiate(prefab, pos, transform.rotation);
                    newShopItem.transform.SetParent(GameObject.FindGameObjectWithTag("ShopItemList").transform, false);
                    newShopItem.SetActive(true);

                    Image itemImage = newShopItem.transform.Find("Picture").GetComponent<Image>();
                    itemImage.sprite = Resources.Load<Sprite>("Textures/Shop/Item/" + System.IO.Path.GetFileNameWithoutExtension(item.id.ToString()));

                    Text itemName = newShopItem.transform.Find("Name").GetComponent<Text>();
                    itemName.text = item.name;

                    Text itemID = newShopItem.transform.Find("ID").GetComponent<Text>();
                    itemID.text = item.id.ToString();

                    setItemCurrency(newShopItem, item);
                }
            };
        } else if (type == 2) {
            foreach (ShopTextureClass item in ShopList.textures)
            {
                if (item.quantity > 0)
                {
                    Vector3 pos = new Vector3(100.0f, 16.0f, 0.0f); ;
                    pos.x = pos.x + (175 * i);
                    i = i + 1;
                    GameObject newShopItem = Instantiate(prefab, pos, transform.rotation);
                    newShopItem.transform.SetParent(GameObject.FindGameObjectWithTag("ShopTextureList").transform, false);
                    newShopItem.SetActive(true);

                    Image itemImage = newShopItem.transform.Find("Picture").GetComponent<Image>();
                    itemImage.sprite = Resources.Load<Sprite>("Textures/Shop/Texture/" + System.IO.Path.GetFileNameWithoutExtension(item.id.ToString()));

                    Text itemName = newShopItem.transform.Find("Name").GetComponent<Text>();
                    itemName.text = item.name;

                    Text itemID = newShopItem.transform.Find("ID").GetComponent<Text>();
                    itemID.text = item.id.ToString();

                    setItemCurrency2(newShopItem, item);
                }
            }
        } else if (type == 3) {
            foreach (ShopColorClass item in ShopList.colors)
            {
                if (item.quantity > 0)
                {
                    Vector3 pos = new Vector3(275.0f, 16.0f, 0.0f); ;
                    pos.x = pos.x + (175 * i);
                    i = i + 1;
                    GameObject newShopItem = Instantiate(prefab, pos, transform.rotation);
                    newShopItem.transform.SetParent(GameObject.FindGameObjectWithTag("ShopColorList").transform, false);
                    newShopItem.SetActive(true);

                    //Image itemImage = newShopItem.transform.Find("Picture").GetComponent<Image>();
                    //itemImage.sprite = Resources.Load<Sprite>("Textures/Shop/Color/" + System.IO.Path.GetFileNameWithoutExtension(item.id.ToString()));

                    Text itemName = newShopItem.transform.Find("Name").GetComponent<Text>();
                    itemName.text = item.name;

                    Text itemID = newShopItem.transform.Find("ID").GetComponent<Text>();
                    itemID.text = item.id.ToString();

                    setItemCurrency3(newShopItem, item);
                }
            }
        }


    }

    private void setItemCurrency(GameObject newShopItem, ShopItemClass item)
    {
        GameObject contener = newShopItem.transform.Find("Price").gameObject;
        buyInformation buyInfo = newShopItem.transform.Find("Price").GetComponent<buyInformation>();

        Image image = contener.transform.Find("Type").GetComponent<Image>();
        image.sprite = Resources.Load<Sprite>("Textures/Shop/Money/0");
        Text price = contener.transform.Find("Number").GetComponent<Text>();
        buyInfo.price = item.price;
        buyInfo.type = 1;
        price.text = item.price.ToString();
        
    }

    private void setItemCurrency2(GameObject newShopItem, ShopTextureClass item)
    {
        GameObject contener = newShopItem.transform.Find("Price").gameObject;
        buyInformation buyInfo = newShopItem.transform.Find("Price").GetComponent<buyInformation>();

        Image image = contener.transform.Find("Type").GetComponent<Image>();
        image.sprite = Resources.Load<Sprite>("Textures/Shop/Money/1");
        Text price = contener.transform.Find("Number").GetComponent<Text>();
        buyInfo.price = item.price;
        buyInfo.type = 2;
        price.text = item.price.ToString();

    }

    private void setItemCurrency3(GameObject newShopItem, ShopColorClass item)
    {
        GameObject contener = newShopItem.transform.Find("Price").gameObject;
        buyInformation buyInfo = newShopItem.transform.Find("Price").GetComponent<buyInformation>();

        Image image = contener.transform.Find("Type").GetComponent<Image>();
        image.sprite = Resources.Load<Sprite>("Textures/Shop/Money/2");
        Text price = contener.transform.Find("Number").GetComponent<Text>();
        buyInfo.price = item.price;
        buyInfo.type = 3;
        price.text = item.price.ToString();

    }

    public void buyOnClick(GameObject prefab)
    {
        buyInformation buyInfo = prefab.transform.Find("Price").GetComponent<buyInformation>();

        Text itemID = prefab.transform.Find("ID").GetComponent<Text>();

        BuyItem(Int32.Parse(itemID.text));
        
        if (buyInfo.type == 1)
        {
            MoneyInventory CrystalCount = Crystal.transform.Find("GameObject").GetComponent<MoneyInventory>();
            CrystalCount.removeCrystal(buyInfo.price);
            CrystalCount.saveCrystal();
        }
        if (buyInfo.type == 2)
        {
            MoneyInventory CashCount = Mentoring.transform.Find("GameObject").GetComponent<MoneyInventory>(); //change mentoring to cash
            CashCount.removeCrystal(buyInfo.price);
            CashCount.saveCrystal();
        }
        if (buyInfo.type == 3)
        {
            MoneyInventory MentoringCount = Cash.transform.Find("GameObject").GetComponent<MoneyInventory>(); //change mentoring to cash
            MentoringCount.removeCrystal(buyInfo.price);
            MentoringCount.saveCrystal();
        }
    }

    public void BuyItem(int id)
    {
        int i = 0;
        if (type == 1)
        {
            API_inventories ShopListToSend = API.GetInventory();
            foreach (API_inventory item in ShopListToSend.inventories)
            {
                if (Int32.Parse(item._id) == id)
                {
                    API.PostInventory(Int32.Parse(item._id), item.name, Int32.Parse(item.quantity) + 1);
                }
            };

            foreach (ShopItemClass item in ShopList.items)
            {                
                if (item.id == id)
                {
                    if (File.Exists(Application.persistentDataPath + "/ShopItem.json"))
                    {
                        ShopList.items[i].quantity = ShopList.items[i].quantity - 1;
                        string json = JsonUtility.ToJson(ShopList);
                        File.WriteAllText(Application.persistentDataPath + "/ShopItem.json", json);
                    }
                }
                i = i + 1;
            };
        }
        else if (type == 2)
        {
            if (File.Exists(Application.persistentDataPath + "/ShopItem.json"))
            {
                API.PostTexture(System.Convert.ToBase64String(textureToSend.EncodeToPNG()));
                ShopList.textures[0].quantity = ShopList.textures[0].quantity - 1;
                string json = JsonUtility.ToJson(ShopList);
                File.WriteAllText(Application.persistentDataPath + "/ShopItem.json", json);
            }
        }
        refresh();
        setUpItem();
    }

    public void refresh()
    {
        int i = 0;
        foreach (Transform child in father.transform)
        {
            if (i > 0)
            {
                GameObject.Destroy(child.gameObject);
            }
            i = i + 1;
        }
    }
}
