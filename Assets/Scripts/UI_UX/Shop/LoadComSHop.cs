using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadComSHop : MonoBehaviour
{
    public GameObject prefab = null;
    public int type = 0;
    public GameObject Cash = null;
    public RectTransform content = null;
    
    private API_ShopTextures ShopList;


    // Start is called before the first frame update
    void Start()
    {
        ShopList = API.GetShopTextures();

        if (ShopList != null && prefab != null)
        {
            setUpItem();
        }
        else
        {
            Debug.Log("ShopItem.Json not found");
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void setUpItem()
    {
        int i = 0;
        foreach (API_ShopTexture item in ShopList.shopTextures)
        {
            if (item.price > 0)
            {
                Vector3 pos = new Vector3(100.0f, 16.0f, 0.0f);
                pos.x = pos.x + (175 * i);
                content.sizeDelta = new Vector2(175 * (i - 2), 300);
                i = i + 1;
                GameObject newShopItem = Instantiate(prefab, pos, transform.rotation);
                newShopItem.transform.SetParent(GameObject.FindGameObjectWithTag("ComItemList").transform, false);
                newShopItem.SetActive(true);

                Image itemImage = newShopItem.transform.Find("Picture").GetComponent<Image>();
                byte[] imageBytes = System.Convert.FromBase64String(item.texture);
                Texture2D textureSprite = new Texture2D(16, 16);

                // Texture blurry if not set
                textureSprite.filterMode = FilterMode.Point;

                textureSprite.LoadImage(imageBytes);

                // Texture 16*16, pivot is center and pixel per unity is 16
                itemImage.sprite = Sprite.Create(textureSprite, new Rect(0, 0, 16, 16), new Vector2(0.5f, 0.5f), 16);

                TMP_Text itemID = newShopItem.transform.Find("ID").GetComponent<TMP_Text>();
                itemID.SetText(item.id.ToString());

                TMP_Text seller = newShopItem.transform.Find("Seller").GetComponent<TMP_Text>();
                seller.SetText(item.seller);

                TMP_Text texture = newShopItem.transform.Find("TextureString").GetComponent<TMP_Text>();
                texture.SetText(item.texture);

                setItemCurrency(newShopItem, item);
            }
        };
    }

    private void setItemCurrency(GameObject newShopItem, API_ShopTexture item)
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
        TMP_Text Seller = prefab.transform.Find("Seller").GetComponent<TMP_Text>();
        TMP_Text Texture = prefab.transform.Find("TextureString").GetComponent<TMP_Text>();

        buyItem(Int32.Parse(itemID.text), Texture.text, Seller.text);

        MoneyInventory MentoringCount = Cash.transform.Find("GameObject").GetComponent<MoneyInventory>(); //change mentoring to cash
        MentoringCount.removeCrystal(buyInfo.price);
        MentoringCount.saveCrystal();

    }

    private void buyItem(int id, string texture, string Seller)
    {
        API_ShopTexture selleditem = new API_ShopTexture();
        foreach (API_ShopTexture item in ShopList.shopTextures)
        {

            if (Int32.Parse(item.id) == id)
            {
                Debug.Log(item.id);
                selleditem.price = 0;
                selleditem.seller = Seller;
                selleditem.texture = texture;
                selleditem.id = id.ToString();

                API.PostBuyedShopTextures(selleditem);
                
                API.PostTexture("", texture);
            }
        }
    }

    public static Texture2D DeCompress(Texture2D source)
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
}
