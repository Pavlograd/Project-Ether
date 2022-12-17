using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class API_Users
{
    public List<API_User> users = new List<API_User>();
}

[Serializable]
public class API_User
{
    public string email = "";
    public string id = "";
    public string username = "";
}