using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class API_inventories
{
    public List<API_inventory> inventories = new List<API_inventory>();
}

[Serializable]
public class API_inventory
{
    public string _id = "";
    public string name = "";
    public string quantity = "";
    public string comment = "";
}