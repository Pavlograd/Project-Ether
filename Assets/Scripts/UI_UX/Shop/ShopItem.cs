using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

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
        ShopList = API.getShop();
        if (ShopList != null  && prefab != null)
        {
            setUpItem();
        }
        else
        {
            Debug.Log("ShopItem.Json not found");
        }

    }

    private void setUpItem()
    {
        int i = 0;
        if (type == 1)
        {
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

                    TMP_Text itemName = newShopItem.transform.Find("Name").GetComponent<TMP_Text>();
                    itemName.SetText(item.name);

                    TMP_Text itemID = newShopItem.transform.Find("ID").GetComponent<TMP_Text>();
                    itemID.SetText(item.id.ToString());

                    setItemCurrency(newShopItem, item);
                }
            };
        }
        else if (type == 2)
        {
            foreach (ShopTextureClass item in ShopList.textures)
            {
                
                if (item.quantity > 0)
                {
                    Debug.Log(item.name);
                    Vector3 pos = new Vector3(100.0f, 16.0f, 0.0f); ;
                    pos.x = pos.x + (175 * i);
                    i = i + 1;
                    GameObject newShopItem = Instantiate(prefab, pos, transform.rotation);
                    newShopItem.transform.SetParent(GameObject.FindGameObjectWithTag("ShopTextureList").transform, false);
                    newShopItem.SetActive(true);

                    Image itemImage = newShopItem.transform.Find("Picture").GetComponent<Image>();
                    itemImage.sprite = Resources.Load<Sprite>("Textures/Shop/Texture/" + System.IO.Path.GetFileNameWithoutExtension(item.id.ToString()));

                    TMP_Text itemName = newShopItem.transform.Find("Name").GetComponent<TMP_Text>();
                    itemName.text = item.name;

                    TMP_Text itemID = newShopItem.transform.Find("ID").GetComponent<TMP_Text>();
                    itemID.SetText(item.id.ToString());

                    setItemCurrency2(newShopItem, item);
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
        TMP_Text price = contener.transform.Find("Number").GetComponent<TMP_Text>();
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
        TMP_Text price = contener.transform.Find("Number").GetComponent<TMP_Text>();
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
        TMP_Text price = contener.transform.Find("Number").GetComponent<TMP_Text>();
        buyInfo.price = item.price;
        buyInfo.type = 3;
        price.text = item.price.ToString();

    }

    public void buyOnClick(GameObject prefab)
    {
        buyInformation buyInfo = prefab.transform.Find("Price").GetComponent<buyInformation>();

        TMP_Text itemID = prefab.transform.Find("ID").GetComponent<TMP_Text>();

        int maxTextureNbr = 0;
        int TextureNbr = 0;

        BuyItem(Int32.Parse(itemID.text));

        if (buyInfo.type == 1)
        {
            MoneyInventory CrystalCount = Crystal.transform.Find("GameObject").GetComponent<MoneyInventory>();
            CrystalCount.removeCrystal(buyInfo.price);
            CrystalCount.saveCrystal();
        }
        if (buyInfo.type == 2)
        {
            maxTextureNbr = API.GetMaxTextureSlot();
            TextureNbr = API.GetTextureSlot();
            if (TextureNbr < maxTextureNbr)
            {
                MoneyInventory CashCount = Mentoring.transform.Find("GameObject").GetComponent<MoneyInventory>(); //change mentoring to cash
                CashCount.removeCrystal(buyInfo.price);
                CashCount.saveCrystal();
                API.PostTextureSlot();
            }
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
        //int maxTextureNbr = 0;
        //int TextureNbr = 0;

        if (type == 1)
        {
            Debug.Log("test");
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
                    ShopList.items[i].quantity = ShopList.items[i].quantity - 1;
                    API.postShop(ShopList);
                }
                i = i + 1;
                

            };
            
        }
        else if (type == 2)
        {
            Texture2D texture = new Texture2D(16, 16);
            texture = DeCompress(Resources.Load<Texture2D>("Textures/Shop/Texture/" + System.IO.Path.GetFileNameWithoutExtension(ShopList.textures[id].id.ToString())));
            
            API.PostTexture("", System.Convert.ToBase64String(texture.EncodeToPNG()));
            ShopList.textures[id].quantity = ShopList.textures[id].quantity - 1;
            API.postShop(ShopList);
           
        }
        
        refresh();
        setUpItem();
    }

    public Texture2D DeCompress(Texture2D source)
    {
        RenderTexture renderTex = RenderTexture.GetTemporary(
                    source.width,
                    source.height,
                    0,
                    RenderTextureFormat.Default,
                    RenderTextureReadWrite.Linear);

        Graphics.Blit(source, renderTex);
        RenderTexture previous = RenderTexture.active;
        RenderTexture.active = renderTex;
        Texture2D readableText = new Texture2D(source.width, source.height);
        readableText.ReadPixels(new Rect(0, 0, renderTex.width, renderTex.height), 0, 0);
        readableText.Apply();
        RenderTexture.active = previous;
        RenderTexture.ReleaseTemporary(renderTex);
        return readableText;
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
