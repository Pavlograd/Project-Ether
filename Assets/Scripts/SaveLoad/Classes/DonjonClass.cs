using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class DonjonClass
{
    public bool tested = false;
    public GearClass gear = new GearClass();
    public List<RoomClass> rooms = new List<RoomClass>();
}