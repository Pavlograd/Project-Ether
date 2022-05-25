using System.IO;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

public class SaveRoom : MonoBehaviour
{
    [SerializeField] private Tilemap _walls;
    [SerializeField] private Tilemap _ground;
    [SerializeField] private Text _name;
    [SerializeField] private GameObject _traps;
    [SerializeField] private GameObject _mobs;
    [SerializeField] private GameObject _decors;
    [SerializeField] private GameObject _portals;
    string _room;

    public RoomClass SetRoomAsString(bool save)
    {
        _room = "{Room:{";
        SetWallsAsString();
        SetGroundAsString();
        _room += "}}";

        RoomClass room = new RoomClass();

        room.room = _room;
        room.name = (_name != null) ? _name.text : "room";
        room.traps = SetTrapsAsString();
        room.mobs = SetMobsAsString();
        room.decors = SetDecorsAsString();
        room.portals = SetPortalsAsString();

        // Now add to save
        if (save)
        {
            string fileContents = File.ReadAllText(Application.persistentDataPath + "/save.json");
            PlayerClass player = JsonUtility.FromJson<PlayerClass>(fileContents);

            player.donjon.rooms.Add(room);
            player.donjon.tested = false;

            string json = JsonUtility.ToJson(player);
            File.WriteAllText(Application.persistentDataPath + "/save.json", json);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        return room;

    }

    public void SetRoomAsString()
    {
        _room = "{Room:{";
        SetWallsAsString();
        SetGroundAsString();
        _room += "}}";

        RoomClass room = new RoomClass();

        room.room = _room;
        room.name = (_name != null) ? _name.text : "room";

        // Now add to save
        string fileContents = File.ReadAllText(Application.persistentDataPath + "/save.json");
        PlayerClass player = JsonUtility.FromJson<PlayerClass>(fileContents);

        player.donjon.rooms.Add(room);
        player.donjon.tested = false;

        string json = JsonUtility.ToJson(player);
        File.WriteAllText(Application.persistentDataPath + "/save.json", json);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SaveEdition()
    {
        _room = "{Room:{";
        SetWallsAsString();
        SetGroundAsString();
        _room += "}}";

        // Now add to save
        string fileContents = File.ReadAllText(Application.persistentDataPath + "/save.json");
        PlayerClass player = JsonUtility.FromJson<PlayerClass>(fileContents);
        RoomClass room = new RoomClass();

        room.room = _room;
        room.name = _name.text;
        room.traps = SetTrapsAsString();
        room.mobs = SetMobsAsString();
        room.decors = SetDecorsAsString();
        room.portals = SetPortalsAsString();

        player.donjon.rooms[GameObject.Find("Load").GetComponent<LoadRoom>().activeRoom] = room;
        player.donjon.tested = false;

        string json = JsonUtility.ToJson(player);
        File.WriteAllText(Application.persistentDataPath + "/save.json", json);
        SceneManager.LoadScene("SandBox");
    }

    public string SetPortalsAsString()
    {
        Portal[] childScripts = _portals.GetComponentsInChildren<Portal>();

        string portals = "";

        for (int i = 0; i < childScripts.Length; i++)
        {
            Portal myChildScript = childScripts[i];

            portals += myChildScript.transform.position.x.ToString("F1", CultureInfo.InvariantCulture) + ":" + myChildScript.transform.position.y.ToString("F1", CultureInfo.InvariantCulture) + ":" + myChildScript.roomConnected + ":" + myChildScript.portalConnected + ",";
        }

        return portals;
    }

    public string SetTrapsAsString()
    {
        Trap[] childScripts = _traps.GetComponentsInChildren<Trap>();

        string traps = "";

        for (int i = 0; i < childScripts.Length; i++)
        {
            Trap myChildScript = childScripts[i];

            traps += myChildScript.transform.position.x.ToString("F1", CultureInfo.InvariantCulture) + ":" + myChildScript.transform.position.y.ToString("F1", CultureInfo.InvariantCulture) + ":" + myChildScript.id + ",";
        }

        return traps;
    }

    public string SetMobsAsString()
    {
        // Get a component to be sure it's a mob
        AIAbilityManager[] childScripts = _mobs.GetComponentsInChildren<AIAbilityManager>();

        string mobs = "";

        for (int i = 0; i < childScripts.Length; i++)
        {
            AIAbilityManager myChildScript = childScripts[i];

            mobs += myChildScript.transform.position.x.ToString("F1", CultureInfo.InvariantCulture) + ":" + myChildScript.transform.position.y.ToString("F1", CultureInfo.InvariantCulture) + ":" + myChildScript.transform.name + ":" + myChildScript.elementaryType.ToString() + ",";
        }

        return mobs;
    }

    public string SetDecorsAsString()
    {
        GameObject[] childScripts = GameObject.FindGameObjectsWithTag("Decor");

        string decors = "";

        for (int i = 0; i < childScripts.Length; i++)
        {
            GameObject myChildScript = childScripts[i];

            decors += myChildScript.transform.position.x.ToString("F1", CultureInfo.InvariantCulture) + ":" + myChildScript.transform.position.y.ToString("F1", CultureInfo.InvariantCulture) + ":" + myChildScript.transform.name + ",";
        }

        return decors;
    }

    public void SetWallsAsString()
    {
        BoundsInt bounds = _walls.cellBounds;
        TileBase[] allTiles = _walls.GetTilesBlock(bounds);

        _room += "Walls:[";

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];

                if (tile != null)
                {
                    _room += x + ":" + y + ":" + tile.name + ",";
                }
            }
        }

        _room += "],";
    }

    public void SetGroundAsString()
    {
        BoundsInt bounds = _ground.cellBounds;
        TileBase[] allTiles = _ground.GetTilesBlock(bounds);

        _room += "Ground:[";

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];
                if (tile != null)
                {
                    _room += x + ":" + y + ":" + tile.name + ",";
                }
            }
        }

        _room += "],";
    }
}
