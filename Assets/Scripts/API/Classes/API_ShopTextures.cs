using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class API_ShopTextures
{
    public List<API_ShopTexture> shopTextures = new List<API_ShopTexture>();
}

[Serializable]
public class API_ShopTexture
{
    public string seller = "";
    public string id = "";
    public string texture = "";
    public int price = 0;
}

[Serializable]
public class API_ShopTextureToSell
{
    public string texture = "";
    public string price = "";
   
}