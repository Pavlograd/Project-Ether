using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class RoomClass
{
    public string name = "";
    public string decors = "";
    public List<TileClass> walls = new List<TileClass>();
    public List<TileClass> floors = new List<TileClass>();
    public List<TileClass> borders = new List<TileClass>();
    public List<TileClass> doors = new List<TileClass>();
    public List<TileClass> traps = new List<TileClass>();
    public List<TileClass> mobs = new List<TileClass>();
}

[Serializable]
public class TileClass
{
    public TileClass(Vector3 position, string _name)
    {
        x = (int)position.x;
        y = (int)position.y;
        name = _name;
    }

    public int x = 0;
    public int y = 0;
    public string name = "";
}