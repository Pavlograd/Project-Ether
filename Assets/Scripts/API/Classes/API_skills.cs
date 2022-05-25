using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class API_skills
{
    public List<API_skill> skills = new List<API_skill>();
}

[Serializable]
public class API_skill
{
    public string _id = "";
    public string _parentId = "";
    public string name = "";
    public string level = "";
    public string equipped = "";
}