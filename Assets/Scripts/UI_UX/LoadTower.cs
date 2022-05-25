using System.IO;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LoadTower : MonoBehaviour
{
    [SerializeField] private Tilemap _walls;
    [SerializeField] private Tilemap _ground;
    [SerializeField] private GameObject _traps;
    [SerializeField] private GameObject _portals;
    private Object[] _tiles;
    [SerializeField] private GameObject[] _trapsPrefab;
    [SerializeField] private GameObject _portalPrefab;
    [SerializeField] private string LvL;
    Vector2Int _size;
    public bool preLoad = true;
    public bool isStarted = false;
    public int offset = 0;

    // Start is called before the first frame update
    void Start()
    {
        Object[] sprites = Resources.LoadAll("Textures", typeof(Sprite));
        _tiles = new Object[sprites.Length];

        for (int i = 0; i < sprites.Length; i++)
        {
            Tile tile = ScriptableObject.CreateInstance<Tile>();
            tile.sprite = (Sprite)sprites[i];

            tile.name = sprites[i].name;

            _tiles[i] = tile;
        }

        if (preLoad)
        {
            LoadTowerFromSave();
        }

        isStarted = true;
    }

    void MoveCamera(string[] map)
    {
        // Center Room in the middle of the camera

        GameObject.Find("Main Camera").transform.position = new Vector3(_size.x / 2.0f, _size.y / 2.0f, -10.0f);
    }

    Tile LoadTexture(string name)
    {
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

        return null;
    }

    void LoadTileMap(string[] map, Tilemap tilemap)
    {
        Vector3Int[] positions = new Vector3Int[map.Length - 1];
        TileBase[] tileArray = new TileBase[positions.Length];

        for (int x = 0; x < map.Length - 1; x++)
        {
            string[] tile = map[x].Split(':');

            positions[x] = new Vector3Int(int.Parse(tile[0]) + offset, int.Parse(tile[1]) + offset, 0);

            _size.x = (positions[x].x > _size.x) ? positions[x].x : _size.x;
            _size.y = (positions[x].y > _size.y) ? positions[x].y : _size.y;

            tileArray[x] = LoadTexture(tile[2]);
        }

        // Empty then replace the tiles

        tilemap.SetTiles(positions, tileArray);
    }

    public void LoadTowerFromSave()
    {
    
        if (File.Exists(Application.persistentDataPath + "/save.json"))
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(Application.persistentDataPath + "/save.json");

            // Deserialize the JSON data 
            // into a pattern matching the PlayerData class.
            PlayerClass player = JsonUtility.FromJson<PlayerClass>(fileContents);

            string room = "";
            foreach (DonjonClass donjon in player.tower) {
                if (donjon.rooms[0].name == LvL) {
                    room = donjon.rooms[0].room;  
                    room = room.Replace("{Room:{Walls:[", "");
                    room = room.Replace(",Ground:[", "");
                    string[] array = room.Split(']');
                    
                    string[] walls = array[0].Split(',');
                    string[] ground = array[1].Split(',');

                    LoadTileMap(walls, _walls);
                    LoadTileMap(ground, _ground);
                }                                      
            }
        }
    }
}
