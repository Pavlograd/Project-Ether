using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class API_User_Textures
{
    public List<API_User_Texture> textures = new List<API_User_Texture>();
}

[Serializable]
public class API_User_Texture
{
    public string id = "";
    public string texture = "";
}