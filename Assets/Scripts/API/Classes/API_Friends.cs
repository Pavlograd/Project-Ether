using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class API_FriendsList
{
    public string user_username;
    public List<string> friends = new List<string>();
    public List<string> friends_username = new List<string>();
    public List<All_Users> all_users = new List<All_Users>();
}

[Serializable]
public class API_FriendsUsernames
{
    public string username = "";
}

[Serializable]
public class All_Users
{
    public string username = "";
    public string id = "";
}

public class API_FriendToAdd
{
    public List<string> friends = new List<string>();
}