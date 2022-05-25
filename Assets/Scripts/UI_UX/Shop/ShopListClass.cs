using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class ShopListClass
{
    public List<ShopItemClass> items = new List<ShopItemClass>();
    public List<ShopTextureClass> textures = new List<ShopTextureClass>();
    public List<ShopColorClass> colors = new List<ShopColorClass>();
}
