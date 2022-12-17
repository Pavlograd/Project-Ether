using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public enum RoomElement
{
    WALL = 0,
    TRAP,
    MOB,
    BORDER,
    FLOOR,
    DOOR,
    NONE,
}

public class Room : MonoBehaviour
{
    [Header("Parents")]
    public List<Transform> parents;
    [Header("Elements")]
    public Dictionary<RoomElement, List<GameObject>> elements = new Dictionary<RoomElement, List<GameObject>>();

    // Start is called before the first frame update
    void Awake()
    {
        // Init lists

        for (int i = 0; i < 6; i++)
        {
            elements.Add((RoomElement)i, new List<GameObject>());
        }
    }

    public GameObject GetElement(Vector3 position)
    {
        foreach (var item in elements)
        {
            foreach (GameObject element in item.Value)
            {
                if (element.transform.position == position)
                {
                    return element;
                }
            }
        }

        return null;
    }

    public RoomElement GetElementType(Vector3 position)
    {
        return GetElementType(GetElement(position));
    }

    public RoomElement GetElementType(GameObject elementObject)
    {
        foreach (var item in elements)
        {
            if (item.Value.Contains(elementObject))
            {
                return item.Key;
            }
        }

        return RoomElement.NONE;
    }

    public void AddElement(GameObject elementObject, RoomElement elementType)
    {
        elements[elementType].Add(elementObject);
    }

    public void RemoveElement(GameObject elementObject, RoomElement elementType)
    {
        elements[elementType].Remove(elementObject);
    }


    // LAYERS


    // Switch object between floor and wall
    public RoomElement SwitchLayer(GameObject elementObject)
    {
        Debug.Log("Switch");

        if (elements[RoomElement.WALL].Contains(elementObject))
        {
            return SwitchParent(RoomElement.WALL, RoomElement.FLOOR, elementObject);
        }
        else if (elements[RoomElement.FLOOR].Contains(elementObject))
        {
            return SwitchParent(RoomElement.FLOOR, RoomElement.WALL, elementObject);
        }
        else if (elements[RoomElement.DOOR].Contains(elementObject))
        {
            DonjonLoaderV2.instance.CreateBorder(this, new TileClass(elementObject.transform.position, "dungeon_textures_7"), true);
            elements[RoomElement.DOOR].Remove(elementObject);
            Destroy(elementObject);

            return RoomElement.BORDER;
        }
        else if (elements[RoomElement.BORDER].Contains(elementObject))
        {
            DonjonLoaderV2.instance.CreateDoor(this, new TileClass(elementObject.transform.position, "dungeon_textures_7"), true);
            elements[RoomElement.BORDER].Remove(elementObject);
            Destroy(elementObject);

            return RoomElement.DOOR;
        }
        return RoomElement.NONE;
    }

    RoomElement SwitchParent(RoomElement previousType, RoomElement newType, GameObject elementObject)
    {
        elements[previousType].Remove(elementObject);
        elements[newType].Add(elementObject);
        elementObject.transform.parent = parents[(int)newType];

        return newType;
    }


    // TEXTURES


    public void ChangeTexture(GameObject elementObject, Sprite texture)
    {
        if (elements[RoomElement.FLOOR].Contains(elementObject)) // FLOORS
        {
            elementObject.GetComponent<SpriteRenderer>().sprite = DonjonLoaderV2.instance.GetTexture(texture.name, false);
        }
        else // WALLS OR BORDERS
        {
            elementObject.GetComponent<SpriteRenderer>().sprite = DonjonLoaderV2.instance.GetTexture(texture.name, true);
        }
    }

    public void ChangeTextures(GameObject elementObject, Sprite texture)
    {
        if (elements[RoomElement.FLOOR].Contains(elementObject)) // FLOORS
        {
            texture = DonjonLoaderV2.instance.GetTexture(texture.name, false);

            ChangeTexturesList(elements[RoomElement.FLOOR], texture);
        }
        else // WALLS OR BORDERS
        {
            texture = DonjonLoaderV2.instance.GetTexture(texture.name, true);

            ChangeTexturesList(elements[RoomElement.WALL], texture);
            ChangeTexturesList(elements[RoomElement.BORDER], texture);
        }
    }

    void ChangeTexturesList(List<GameObject> list, Sprite texture)
    {
        foreach (GameObject item in list)
        {
            item.GetComponent<SpriteRenderer>().sprite = texture;
        }
    }

    public List<GameObject> GetWalls()
    {
        return elements[RoomElement.FLOOR];
    }

    public List<GameObject> GetFloors()
    {
        return elements[RoomElement.FLOOR];
    }

    void ChangeColor(Color color)
    {
        foreach (var item in elements)
        {
            foreach (GameObject element in item.Value)
            {
                try
                {
                    element.GetComponent<SpriteRenderer>().material.color = color;
                }
                catch (Exception error)
                {
                    DumpError(error);
                }
            }
        }
    }

    public void UnselectedAll()
    {
        ChangeColor(Color.white);
    }

    public void SelectAll()
    {
        ChangeColor(Color.cyan);
    }

    // Destroyers

    public void DestroyElements()
    {
        foreach (var item in elements)
        {
            DestroyList(item.Value);
        }
    }

    public void DestroyList(List<GameObject> list)
    {
        foreach (GameObject item in list)
        {
            Destroy(item);
        }

        list.Clear();
    }


    // TRASH


    void DumpError(Exception error)
    {
        return;
    }


    // SAVING

    public List<TileClass> GetTiles(List<GameObject> list)
    {
        List<TileClass> tiles = new List<TileClass>();

        foreach (var item in list)
        {
            tiles.Add(new TileClass(item.transform.position, item.GetComponent<SpriteRenderer>().sprite.name));
        }

        return tiles;
    }

    public List<TileClass> GetTilesElements(List<GameObject> list)
    {
        List<TileClass> tiles = new List<TileClass>();

        foreach (var item in list)
        {
            tiles.Add(new TileClass(item.transform.position, item.name));
        }

        return tiles;
    }

    public RoomClass GetRoomClass()
    {
        RoomClass roomClass = new RoomClass();

        roomClass.walls = GetTiles(elements[RoomElement.WALL]);
        roomClass.floors = GetTiles(elements[RoomElement.FLOOR]);
        roomClass.borders = GetTiles(elements[RoomElement.BORDER]);
        roomClass.doors = GetTilesElements(elements[RoomElement.DOOR]);
        roomClass.traps = GetTilesElements(elements[RoomElement.TRAP]);
        roomClass.mobs = GetTilesElements(elements[RoomElement.MOB]);

        return roomClass;
    }


    // GAMEPLAY


    public void MobKilled(GameObject mob)
    {
        //Mob killed
        elements[RoomElement.MOB].Remove(mob);

        TryOpenDoors();
    }

    public void TryOpenDoors()
    {
        if (elements[RoomElement.MOB].Count == 0)
        {
            // No mor mobs open doors

            foreach (GameObject item in elements[RoomElement.DOOR])
            {
                Door door = item.GetComponent<Door>();

                door.Open();
            }
        }
    }
}
