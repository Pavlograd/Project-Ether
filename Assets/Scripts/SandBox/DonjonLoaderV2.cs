using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonjonLoaderV2 : MonoBehaviour
{
    public static DonjonLoaderV2 instance;
    [Header("Components")]
    [SerializeField] private LoadTextures loadTextures;
    public SandBoxManager sandBoxManager;
    //[Header("Graphic")]
    //[SerializeField] Material baseMaterial; // base material for all objects to create material from it
    [Header("Prefabs")]
    [SerializeField] GameObject prefabRoom;
    [SerializeField] GameObject[] prefabsElements;
    [SerializeField] GameObject prefabDoorPath;
    [SerializeField] public List<GameObject> sandBoxprefabsTrapAndMobs;
    [SerializeField] public List<GameObject> prefabsTrapAndMobs;
    [Header("Donjon")]
    [SerializeField] GameObject donjon;
    [SerializeField] GameObject player;
    public List<Room> rooms = new List<Room>();
    [Header("Variables")]
    public bool sandBox = true;
    public List<GameObject> doorsPaths = new List<GameObject>();

    protected virtual void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        string path = Application.persistentDataPath + "/save.json";
        DonjonClass donjonClass = null;

        // Path will be either the name of the player or the level path
        if (!sandBox)
        {
            path = null;

            if (CrossSceneInfos.donjonPath != null)
            {
                path = CrossSceneInfos.donjonPath;
                CrossSceneInfos.donjonPath = null;
            }
        }
        else
        {
            // User connected trying to modify their donjon
            path = API.GetUser().username;
        }

        // File.Exists is for campaign only
        if (File.Exists(path))
        {
            // For player and campaign
            string fileContents = File.ReadAllText(path);
            PlayerClass player = JsonUtility.FromJson<PlayerClass>(fileContents);

            donjonClass = player.donjon;
        }
        else
        {
            // GET Donjon from API
            // CrossSceneInfos.donjonPath is the username is that context
            API_Donjon objectDonjon = API.GetUserDonjon(path);

            if (objectDonjon != null)
            {
                // Convert to DonjonClass
                donjonClass = JsonUtility.FromJson<DonjonClass>(objectDonjon.data);
            }
            else
            {
                donjonClass = new DonjonClass();
            }
        }

        if (donjonClass != null)
        {
            foreach (RoomClass item in donjonClass.rooms)
            {
                LoadRoomFromRoom(item);
            }

            CreateDoorsPath();

            if (!sandBox)
            {
                foreach (Room room in rooms)
                {
                    room.TryOpenDoors(); // Open doors for rooms without mobs
                }
            }
        }
    }

    public Room GetRoomFromPosition(Vector3 position)
    {
        foreach (Room room in rooms)
        {
            if (room.GetElement(position) != null)
            {
                return room;
            }
        }

        return null;
    }

    public GameObject GetObjectAtPosition(Vector3 position)
    {
        foreach (Room room in rooms)
        {
            if (room.GetElement(position) != null)
            {
                return room.GetElement(position);
            }
        }

        return null;
    }

    public RoomElement GetElementAtPosition(Vector3 position)
    {
        foreach (Room room in rooms)
        {
            if (room.GetElement(position) != null)
            {
                return room.GetElementType(position);
            }
        }

        return RoomElement.NONE;
    }

    public RoomElement GetElement(GameObject elementObject)
    {
        foreach (Room room in rooms)
        {
            if (room.GetElement(elementObject.transform.position) != null)
            {
                return room.GetElementType(elementObject);
            }
        }

        return RoomElement.NONE;
    }

    public void UnSelectAll()
    {
        foreach (Room room in rooms)
        {
            room.UnselectedAll();
        }
    }

    public Sprite GetTexture(string name, bool wall)
    {
        List<Sprite> _tiles = wall ? loadTextures.texturesWall : loadTextures.textures;

        if (_tiles != null)
        {
            foreach (var tile in _tiles)
            {
                if (name == tile.name)
                {
                    return tile;
                }
            }
        }

        Debug.Log("No texture");

        return null;
    }

    public void LoadRoomFromRoom(RoomClass roomClass)
    {
        GameObject objectRoom = Instantiate(prefabRoom, Vector3.zero, Quaternion.identity, donjon.transform);
        Room room = objectRoom.GetComponent<Room>();

        rooms.Add(room);

        LoadTiles(room, roomClass.borders, RoomElement.BORDER);
        LoadTiles(room, roomClass.walls, RoomElement.WALL);
        LoadTiles(room, roomClass.floors, RoomElement.FLOOR);
        LoadDoors(room, roomClass);
        LoadElements(room, roomClass.traps, RoomElement.TRAP);
        LoadElements(room, roomClass.mobs, RoomElement.MOB);
    }

    public void LoadTiles(Room room, List<TileClass> tiles, RoomElement elementType)
    {
        foreach (TileClass tile in tiles)
        {
            GameObject newObject = Instantiate(prefabsElements[(int)elementType], new Vector3(tile.x, tile.y, 0), Quaternion.identity, room.parents[(int)elementType]);

            newObject.GetComponent<SpriteRenderer>().sprite = GetTexture(tile.name, elementType != RoomElement.FLOOR);
            room.AddElement(newObject, elementType);
        }
    }

    void LoadDoors(Room room, RoomClass roomClass)
    {
        List<TileClass> tiles = roomClass.doors;

        foreach (TileClass tile in tiles)
        {
            CreateDoor(room, tile, false);
        }
    }

    void LoadElements(Room room, List<TileClass> tiles, RoomElement elementType)
    {
        foreach (TileClass tile in tiles)
        {
            CreateElement(room, tile, false, elementType);
        }
    }

    public GameObject GetPrefabByName(string prefabName, List<GameObject> list)
    {
        foreach (var item in list)
        {
            if (item.name == prefabName) return item;
        }

        return null;
    }

    public void CreateElement(Room room, TileClass tile, bool select, RoomElement elementType)
    {
        GameObject newObject = Instantiate(GetPrefabByName(tile.name, sandBox ? sandBoxprefabsTrapAndMobs : prefabsTrapAndMobs), new Vector3(tile.x, tile.y, 0), Quaternion.identity, room.parents[(int)elementType]);

        newObject.name = tile.name;

        room.AddElement(newObject, elementType);

        if (select) sandBoxManager.SelectElement(newObject);
    }

    public void CreateDoor(Room room, TileClass tile, bool select)
    {
        GameObject newObject = Instantiate(prefabsElements[(int)RoomElement.DOOR], new Vector3(tile.x, tile.y, 0), Quaternion.identity, room.parents[(int)RoomElement.DOOR]);

        newObject.name = "Door";

        room.AddElement(newObject, RoomElement.DOOR);

        if (select) sandBoxManager.SelectElement(newObject);
    }

    public void CreateBorder(Room room, TileClass tile, bool select)
    {
        GameObject newObject = Instantiate(prefabsElements[(int)RoomElement.BORDER], new Vector3(tile.x, tile.y, 0), Quaternion.identity, room.parents[(int)RoomElement.BORDER]);

        newObject.GetComponent<SpriteRenderer>().sprite = GetTexture(tile.name, true);
        room.AddElement(newObject, RoomElement.BORDER);

        if (select) sandBoxManager.SelectElement(newObject);
    }


    // DOORS PATHS


    public List<GameObject> GetDoors()
    {
        List<GameObject> doors = new List<GameObject>();

        foreach (Room room in rooms)
        {
            doors.AddRange(room.elements[RoomElement.DOOR]);
        }

        return doors;
    }

    public void CreateDoorsPath()
    {
        List<GameObject> doors = GetDoors();
        List<GameObject> paths = new List<GameObject>();
        Vector3 position = Vector3.zero;
        bool correct = true;

        DestroyList(doorsPaths);

        // Reset variables in component
        foreach (GameObject item in doors)
        {
            item.GetComponent<Door>().otherDoor = null;
        }

        for (int i = 0; i < doors.Count; i++)
        {
            GameObject door = doors[i];

            for (int y = 0; y < doors.Count; y++)
            {
                GameObject otherDoor = doors[y];

                if (y > i) // prevent checking path in the other way
                {
                    correct = true;
                    paths.Clear();
                    position = door.transform.position;

                    if (door != otherDoor)
                    {
                        if (door.transform.position.x == otherDoor.transform.position.x)
                        {
                            position.y += door.transform.position.y < otherDoor.transform.position.y ? 1 : -1;

                            while (position.y != otherDoor.transform.position.y)
                            {
                                if (GetObjectAtPosition(position) != null)
                                {
                                    correct = false;
                                    DestroyList(paths);

                                    break;
                                }

                                if (sandBox)
                                {
                                    GameObject newPath = Instantiate(prefabDoorPath, position, Quaternion.identity);
                                    paths.Add(newPath);
                                }

                                position.y += door.transform.position.y < otherDoor.transform.position.y ? 1 : -1;
                            }

                            if (correct)
                            {
                                door.GetComponent<Door>().otherDoor = otherDoor.GetComponent<Door>();
                                otherDoor.GetComponent<Door>().otherDoor = door.GetComponent<Door>();
                                doorsPaths.AddRange(paths);
                            }
                        }
                        else if (door.transform.position.y == otherDoor.transform.position.y)
                        {
                            position.x += door.transform.position.x < otherDoor.transform.position.x ? 1 : -1;

                            while (position.x != otherDoor.transform.position.x)
                            {
                                if (GetObjectAtPosition(position) != null)
                                {
                                    correct = false;
                                    DestroyList(paths);

                                    break;
                                }

                                if (sandBox)
                                {
                                    GameObject newPath = Instantiate(prefabDoorPath, position, Quaternion.identity);
                                    paths.Add(newPath);
                                }

                                position.x += door.transform.position.x < otherDoor.transform.position.x ? 1 : -1;
                            }

                            if (correct)
                            {
                                door.GetComponent<Door>().otherDoor = otherDoor.GetComponent<Door>();
                                otherDoor.GetComponent<Door>().otherDoor = door.GetComponent<Door>();
                                doorsPaths.AddRange(paths);
                            }
                        }
                    }
                }
            }
        }
    }

    void DestroyList(List<GameObject> list)
    {
        foreach (var item in list)
        {
            Destroy(item);
        }

        list.Clear();
    }
}
