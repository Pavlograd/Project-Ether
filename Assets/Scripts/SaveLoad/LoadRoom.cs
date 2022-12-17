using System.Net.Mime;
using System.IO;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using TMPro;


// LOAD A ROOM OF THE DONJON
// USE IT FOR PLAYING OR LOADING ONE ROOM
// USED BY LoadDonjon.cs TO LOAD ROOMS ONE BY ONE
// USED BY SANDBOX's scene to LOAD INFOS OF ONE ROOM


public class LoadRoom : MonoBehaviour
{
    [SerializeField] private Tilemap _walls;
    [SerializeField] private Tilemap _ground;
    [SerializeField] private GameObject _traps;
    [SerializeField] private GameObject _mobs;
    [SerializeField] private GameObject _decors;
    [SerializeField] private GameObject _prefabDecors;
    [SerializeField] private GameObject _portals;
    [SerializeField] private GameObject[] _trapsPrefab;
    [SerializeField] private GameObject[] _mobsPrefab;
    [SerializeField] private GameObject _portalPrefab;
    [SerializeField] private LoadTextures _loadTextures;
    public Vector2Int _size;
    public int activeRoom = 0;
    public bool clear = true;
    public bool preLoad = true;
    public bool isStarted = false;
    public int offset = 0;
    private bool groundTile = false;
    [SerializeField] private TMP_Text text;
    public int initialY = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (preLoad)
        {
            LoadRoomFromSave(activeRoom);
        }

        isStarted = true;
    }

    void DestroyAllChild(GameObject parent)
    {
        foreach (Transform child in parent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    void MoveCamera(string[] map)
    {
        // Center Room in the middle of the camera

        GameObject.Find("Main Camera").transform.position = new Vector3(_size.x / 2.0f, _size.y / 2.0f, -10.0f);
    }

    Tile LoadTexture(string name)
    {
        Object[] _tiles = groundTile ? _loadTextures.tilesGround : _loadTextures.tilesWall;

        if (_tiles != null)
        {
            foreach (var tile in _tiles)
            {
                if (name == tile.name)
                {
                    return (Tile)tile;
                }
            }
        }

        Debug.Log("No texture");

        return null;
    }

    void LoadTileMap(string[] map, Tilemap tilemap)
    {
        Vector3Int[] positions = new Vector3Int[map.Length - 1];
        TileBase[] tileArray = new TileBase[positions.Length];

        for (int x = 0; x < map.Length - 1; x++)
        {
            string[] tile = map[x].Split(':');

            positions[x] = new Vector3Int(int.Parse(tile[0]) + offset, int.Parse(tile[1]), 0);

            _size.x = (positions[x].x > _size.x) ? positions[x].x : _size.x;
            _size.y = (positions[x].y > _size.y) ? positions[x].y : _size.y;

            tileArray[x] = LoadTexture(tile[2]);
        }

        // Empty then replace the tiles
        if (text != null)
        {
            text.text += positions[0].ToString();
            PrintDebug.Maurin("first tile loaded:" + text.text);
        }

        if (clear) tilemap.ClearAllTiles();
        tilemap.SetTiles(positions, tileArray);

        TestSave();
    }

    void TestSave()
    {
        BoundsInt bounds = _walls.cellBounds;

        PrintDebug.Maurin(bounds);
        TileBase[] allTiles = _walls.GetTilesBlock(bounds);
        bool debugValue = false;

        for (int x = 0; x < bounds.size.x; x++)
        {
            for (int y = 0; y < bounds.size.y; y++)
            {
                TileBase tile = allTiles[x + y * bounds.size.x];

                if (tile != null)
                {
                    if (text != null && !debugValue)
                    {
                        debugValue = true;
                        PrintDebug.Maurin("first tile saved test: " + x + ":" + y + ":" + tile.name + ",");
                    }
                }
            }
        }
    }

    void LoadTraps(string[] traps)
    {
        Vector3 position = new Vector3(0.0f, 0.0f, 0.0f);
        int id = 0;

        for (int x = 0; x < traps.Length - 1; x++)
        {
            string[] trap = traps[x].Split(':');

            position.x = float.Parse(trap[0], CultureInfo.InvariantCulture) + offset;
            position.y = float.Parse(trap[1], CultureInfo.InvariantCulture);
            id = int.Parse(trap[2]);

            Instantiate(_trapsPrefab[id], position, Quaternion.identity, _traps.transform);
        }
    }

    void LoadMobs(string[] traps)
    {
        Vector3 position = new Vector3(0.0f, 0.0f, 0.0f);
        string name = "";

        for (int x = 0; x < traps.Length - 1; x++)
        {
            string[] trap = traps[x].Split(':');

            position.x = float.Parse(trap[0], CultureInfo.InvariantCulture) + offset;
            position.y = float.Parse(trap[1], CultureInfo.InvariantCulture);
            name = trap[2];

            for (int i = 0; i < _mobsPrefab.Length; i++)
            {
                if (_mobsPrefab[i].name == name)
                {
                    GameObject newMob = Instantiate(_mobsPrefab[i], position, Quaternion.identity, _mobs.transform);

                    // Delete the clone from the name
                    newMob.name = newMob.name.Replace("(Clone)", "");

                    // Disable components to stop movements
                    newMob.GetComponent<AIMovementManager>().enabled = false;
                    newMob.GetComponent<AIAbilityManager>().enabled = false;
                    newMob.GetComponent<BoxCollider2D>().enabled = false;

                    break;
                }
            }
        }
    }

    void LoadDecors(string[] decors)
    {
        List<Sprite> sprites = _loadTextures.textures;
        Vector3 position = new Vector3(0.0f, 0.0f, 0.0f);
        string name = "";


        for (int x = 0; x < decors.Length - 1; x++)
        {
            string[] decor = decors[x].Split(':');

            position.x = float.Parse(decor[0], CultureInfo.InvariantCulture) + offset;
            position.y = float.Parse(decor[1], CultureInfo.InvariantCulture);
            name = decor[2];

            GameObject newDecor = Instantiate(_prefabDecors, position, Quaternion.identity, _decors.transform);

            for (int i = 0; i < sprites.Count; i++)
            {
                if (sprites[i].name == name)
                {
                    newDecor.GetComponent<SpriteRenderer>().sprite = sprites[i];
                    newDecor.name = name;
                }
            }

        }
    }

    void LoadPortals(string[] portals)
    {
        Vector3 position = new Vector3(0.0f, 0.0f, -1.0f);

        for (int x = 0; x < portals.Length - 1; x++)
        {
            string[] trap = portals[x].Split(':');

            position.x = float.Parse(trap[0], CultureInfo.InvariantCulture) + offset;
            position.y = float.Parse(trap[1], CultureInfo.InvariantCulture);

            GameObject newObject = Instantiate(_portalPrefab, position, Quaternion.identity, _portals.transform);
            Portal portal = newObject.GetComponent<Portal>();

            portal.room = activeRoom;
            portal.portal = x + 1;
            portal.roomConnected = int.Parse(trap[2]);
            portal.portalConnected = int.Parse(trap[3]);
        }
    }

    public void LoadRoomFromRoom(RoomClass roomClass)
    {
        /*string room = roomClass.room;

        room = room.Replace("{Room:{Walls:[", "");
        room = room.Replace(",Ground:[", "");

        string[] array = room.Split(']');

        string[] walls = array[0].Split(',');
        string[] ground = array[1].Split(',');

        groundTile = false;
        initialY = int.Parse(walls[0].Split(':')[1]);
        LoadTileMap(walls, _walls);
        groundTile = true;
        LoadTileMap(ground, _ground);
        LoadTraps(roomClass.traps.Split(','));
        LoadMobs(roomClass.mobs.Split(','));
        LoadDecors(roomClass.decors.Split(','));
        LoadPortals(roomClass.portals.Split(','));

        if (clear) MoveCamera(walls);*/
    }

    public void LoadRoomFromSave(int index)
    {
        //Debug.Log(index);
        activeRoom = index;

        if (clear)
        {
            DestroyAllChild(_traps);
            DestroyAllChild(_mobs);
            DestroyAllChild(_portals);
        }

        string fileContents = File.ReadAllText(CrossSceneInfos.donjonPath == null ? Application.persistentDataPath + "/save.json" : CrossSceneInfos.donjonPath);

        // Read the entire file and save its contents.

        // Deserialize the JSON data 
        // into a pattern matching the PlayerData class.
        PlayerClass player = JsonUtility.FromJson<PlayerClass>(fileContents);

        if (index < player.donjon.rooms.Count) LoadRoomFromRoom(player.donjon.rooms[index]);

        //if (clear || index == 0) MoveCamera(walls);
        /*else
        {
            Debug.Log("Save doesn't exists");

            PlayerClass player = new PlayerClass();
            player.initValues();

            string json = JsonUtility.ToJson(player);

            Debug.Log(Application.persistentDataPath);
            File.WriteAllText(Application.persistentDataPath + "/save.json", json);
        }*/
    }
}
