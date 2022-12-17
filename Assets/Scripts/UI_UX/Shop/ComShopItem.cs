using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class ComShopItem : MonoBehaviour
{
    public GameObject prefab = null;
    public GameObject Mentoring = null;

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
        if (prefab != null)
        {
            setUpItem();
        }
    }

    private void setUpItem()
    {
        //int i = 0;

        foreach (ComShopItemClass item in ShopList.items)
        {
            /* Vector3 pos = new Vector3(100.0f, 16.0f, 0.0f);
             pos.x = pos.x + (175 * i);
             i = i + 1;
             GameObject newShopItem = Instantiate(prefab, pos, transform.rotation);
             newShopItem.transform.SetParent(GameObject.FindGameObjectWithTag("ComItemList").transform, false);
             newShopItem.SetActive(true);

             Image itemImage = newShopItem.transform.Find("Picture").GetComponent<Image>();
             itemImage.sprite = Resources.Load<Sprite>("Textures/Shop/Item/" + System.IO.Path.GetFileNameWithoutExtension(item.id.ToString()));

             Text itemName = newShopItem.transform.Find("Name").GetComponent<Text>();
             itemName.text = item.name;

             setItemCurrency(newShopItem, item);*/
        };
    }

    private void setItemCurrency(GameObject newShopItem, ComShopItemClass item)
    {
        /*GameObject contener = newShopItem.transform.Find("Price").gameObject;
        buyInformation buyInfo = newShopItem.transform.Find("Price").GetComponent<buyInformation>();

        Image image = contener.transform.Find("Type").GetComponent<Image>();
        image.sprite = Resources.Load<Sprite>("Textures/Shop/Money/2");
        Text price = contener.transform.Find("Number").GetComponent<Text>();
        buyInfo.price = item.price;
        buyInfo.type = item.moneyType;
        price.text = item.price.ToString();*/

    }

    public void buyOnClick(GameObject prefab)
    {
        /*buyInformation buyInfo = prefab.transform.Find("Price").GetComponent<buyInformation>();

        MoneyInventory MentoringCount = Mentoring.transform.Find("GameObject").GetComponent<MoneyInventory>(); //change mentoring to cash
        MentoringCount.removeCrystal(buyInfo.price);
        MentoringCount.saveCrystal();*/
    }
}
