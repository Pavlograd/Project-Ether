using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct GenerationRoom
{
    public int minimumLevel; // Prevent low level player to be lost in huge room
    public int minSize;
    public int maxSize;
    public int minTraps;
    public int maxTraps;
    public int minMobs;
    public int maxMobs;
    public string name;
    public float probability; // Last room should have 100 here
    public float probabilityGreenTrap;
    public float probabilityRedTrap;
}

[System.Serializable]
public struct Pattern
{
    public Sprite[] sprites;
}

[System.Serializable]
public struct GenerationTrap
{
    public int minimumLevel; // Prevent low level player to fight powerful entities
    public GameObject prefab;
    public float probability; // Last trap should be 100
}

// Different struct in case we change one
// Boss is also set with that but an higher level than possible
[System.Serializable]
public struct GenerationMob
{
    public int minimumLevel; // Prevent low level player to fight powerful entities
    public GameObject prefab;
    public float probability; // Last mob should be 100
}

[System.Serializable]
public struct TexturePack
{
    public Sprite[] grounds;
    public Sprite[] walls;
}

public class DonjonGenerator : MonoBehaviour
{
    [SerializeField] DonjonGeneratorData data;
    [SerializeField] RoomGenerator _roomGenerator;
    [SerializeField] SaveRoom _saveRoom;
    [SerializeField] Transform portals;
    [SerializeField] Transform mobs;
    [SerializeField] Transform traps;

    [SerializeField] private Tilemap _ground;
    [SerializeField] private Tilemap _walls;
    public int donjonLevel = 0;
    int numberRoom = 0;
    States donjonState;

    // Start is called before the first frame update
    void Start()
    {
        PlayerClass player = new PlayerClass();

        donjonLevel = player.level;

        numberRoom = ((int)Mathf.Sqrt(donjonLevel) * 2) + 3 + Random.Range(-1 * donjonLevel / 10, donjonLevel / 10);
        donjonState = (States)Random.Range(0, (int)States.DOT); // Type of the donjon

        Debug.Log("Donjon State : " + donjonState);

        player.donjon.rooms = GenerateRooms();

        Debug.Log("Number of rooms:" + player.donjon.rooms.Count);

        string json = JsonUtility.ToJson(player);

        string path = Application.persistentDataPath + "/" + Random.Range(0, 256) + "randomLevel.json";
        File.WriteAllText(path, json);
        CrossSceneInfos.donjonPath = path;
        SceneManager.LoadScene("Donjon");
    }

    List<RoomClass> GenerateRooms()
    {
        List<RoomClass> rooms = new List<RoomClass>();

        for (int i = 0; i < numberRoom; i++)
        {
            GenerationRoom gRoom = GetRandomRoom();
            Vector2Int roomSize = RandomSizeRoom(gRoom);

            roomSize.y = roomSize.x;
            RoomGenerator.Form form = (RoomGenerator.Form)Random.Range(0, 2);

            if (form == RoomGenerator.Form.Circle) roomSize += Vector2Int.one * 2;

            TexturePack pack = data._texturesPacks[Random.Range(0, data._texturesPacks.Length)];

            _roomGenerator.ChangeTiles(pack.grounds[Random.Range(0, pack.grounds.Length)], pack.walls[Random.Range(0, pack.walls.Length)]);
            _roomGenerator.ChangeWidth(roomSize.x);
            _roomGenerator.ChangeHeigth(roomSize.y);
            _roomGenerator.ChangeForm((int)form);

            // put tiles of different colors to get a unique room
            _roomGenerator.SetRandomTiles(pack);

            GenerateGroundPattern(pack, roomSize, form, gRoom);

            if (i < numberRoom - 1) // Generate Traps and mobs only on rooms before Boss' room
            {
                GenerateMobs(gRoom);
                GenerateTraps(gRoom);
            }
            GeneratePortals(i, roomSize);

            RoomClass room = _saveRoom.SetRoomAsString(false);

            rooms.Add(room);

            //Debug.Log(room.room);
        }

        return rooms;
    }

    void GeneratePortals(int i, Vector2Int roomSize)
    {
        Vector3 pos1 = Random.Range(0, 2) == 0 ? new Vector3(2.5f, (roomSize.y / 2.0f), 0.0f) : new Vector3((roomSize.x / 2.0f), 2.5f, 0.0f);
        Vector3 pos2 = Random.Range(0, 2) == 0 ? new Vector3(roomSize.x - 2.5f, (roomSize.y / 2.0f), 0.0f) : new Vector3((roomSize.x / 2.0f), roomSize.y - 2.5f, 0.0f);
        bool reversePos = Random.Range(0, 2) == 0;
        GameObject goPortal1 = Instantiate(data.portal, reversePos ? pos2 : pos1, Quaternion.identity, portals);
        GameObject goPortal2 = Instantiate(data.portal, reversePos ? pos1 : pos2, Quaternion.identity, portals);

        //Debug.Log(goPortal1.transform.position);

        goPortal1.GetComponent<Portal>().roomConnected = i - 1;
        goPortal1.GetComponent<Portal>().portalConnected = (i == 0) ? -1 : 0; // Case for startIsland
        goPortal2.GetComponent<Portal>().roomConnected = i + 1;
        goPortal2.GetComponent<Portal>().portalConnected = 1;

        _walls.SetTile(Vector3Int.FloorToInt(goPortal1.transform.position), null);
        _walls.SetTile(Vector3Int.FloorToInt(goPortal2.transform.position), null);

        Trap[] childScripts = traps.gameObject.GetComponentsInChildren<Trap>();

        for (int y = 0; y < childScripts.Length; y++)
        {
            Vector3 pos = childScripts[y].gameObject.transform.position;

            if (pos == goPortal1.transform.position || pos == goPortal2.transform.position) DestroyImmediate(childScripts[y].gameObject);
        }

        if (i == numberRoom - 1)
        {
            GameObject boss = InstantiateMob(data.boss, goPortal2.transform.position); // Spawn Boss on last element
            AbilitiesHolder abilitiesHolder = boss.GetComponent<AbilitiesHolder>();

            abilitiesHolder.defaultAttack = GetRandomAbility();
            abilitiesHolder.abilities.Add(GetRandomAbility());
            abilitiesHolder.abilities.Add(GetRandomAbility());
            abilitiesHolder.abilities.Add(GetRandomAbility());
            abilitiesHolder.abilities.Add(GetRandomAbility());
            DestroyImmediate(goPortal2);
        }
    }

    Ability GetRandomAbility()
    {
        return data._abilitiesAvailableData[Random.Range(0, data._abilitiesAvailableData.Count)];
    }

    // Need to nerge GenerateMobs and GenerateTraps later
    void GenerateMobs(GenerationRoom room)
    {
        BoundsInt bounds = _ground.cellBounds;
        TileBase[] allTiles = _ground.GetTilesBlock(bounds);

        int numberMob = GetNumberMobs(room);
        int actualNumberMob = 0;

        while (actualNumberMob != numberMob)
        {
            Vector3Int pos = new Vector3Int(Random.Range(0, bounds.size.x), Random.Range(0, bounds.size.y), 0);
            TileBase tile = allTiles[pos.x + pos.y * bounds.size.x];

            if (tile != null && _walls.GetTile(pos) == null)
            {
                // Instantiate mob here

                foreach (GenerationMob mob in data.mobs)
                {
                    if (donjonLevel >= mob.minimumLevel && Random.Range(0f, 100f) <= mob.probability)
                    {
                        InstantiateMob(mob.prefab, pos);
                    }
                }
                actualNumberMob++;
            }
        }
    }

    GameObject InstantiateMob(GameObject prefab, Vector3 pos)
    {
        GameObject mobGO = Instantiate(prefab, new Vector3(pos.x + 0.5f, pos.y + 0.5f, 0.0f), Quaternion.identity, mobs);

        mobGO.name = mobGO.name.Replace("(Clone)", "");

        return mobGO;
    }

    void GenerateTraps(GenerationRoom room)
    {
        BoundsInt bounds = _ground.cellBounds;
        TileBase[] allTiles = _ground.GetTilesBlock(bounds);

        int numberTrap = GetNumberTraps(room);
        int actualNumberTrap = 0;

        while (actualNumberTrap != numberTrap)
        {
            Vector3Int pos = new Vector3Int(Random.Range(0, bounds.size.x), Random.Range(0, bounds.size.y), 0);
            TileBase tile = allTiles[pos.x + pos.y * bounds.size.x];

            if (tile != null && _walls.GetTile(pos) == null)
            {
                // Instantiate trap here

                foreach (GenerationTrap trap in data.traps)
                {
                    if (donjonLevel >= trap.minimumLevel && Random.Range(0f, 100f) <= trap.probability)
                    {
                        GameObject trapGO = Instantiate(trap.prefab, new Vector3(pos.x + 0.5f, pos.y + 0.5f, 0.0f), Quaternion.identity, traps);
                    }
                }
                actualNumberTrap++;
            }
        }
    }

    int GetNumberMobs(GenerationRoom room)
    {
        return Random.Range(room.minMobs, room.maxMobs);
    }

    int GetNumberTraps(GenerationRoom room)
    {
        return Random.Range(room.minTraps, room.maxTraps);
    }

    void GenerateGroundPattern(TexturePack pack, Vector2Int roomSize, RoomGenerator.Form form, GenerationRoom gRoom)
    {
        Texture2D texturePattern = GetPattern(roomSize);
        BoundsInt bounds = _ground.cellBounds;

        // Offsets put pattern in the middle of the room
        int offsetX = form == RoomGenerator.Form.Square ? roomSize.x - texturePattern.width : roomSize.x - texturePattern.width - 1;
        int offsetY = form == RoomGenerator.Form.Square ? roomSize.y - texturePattern.height : roomSize.y - texturePattern.height - 1;

        //int offsetX = 0;
        //int offsetY = 0;

        TileBase[] allTiles = _ground.GetTilesBlock(bounds);

        Debug.Log("Size is : " + roomSize.x + " " + roomSize.y);

        for (int x = 0; x < texturePattern.width; x++)
        {
            for (int y = 0; y < texturePattern.height; y++)
            {
                //TileBase tile = allTiles[x + y * bounds.size.x];
                int indexWithOffset = ((x + offsetX) + (y + offsetY) * bounds.size.x);
                Color patternColor = texturePattern.GetPixel(x, y);
                Vector3 position = new Vector3Int((roomSize.x / 2) + x - (texturePattern.width / 2), (roomSize.y / 2) + y - (texturePattern.height / 2), 0);

                Debug.Log(patternColor);

                if (indexWithOffset < allTiles.Length && indexWithOffset >= 0 && allTiles[indexWithOffset] != null && _walls.GetTile(Vector3Int.FloorToInt(position)) == null)
                {
                    if (patternColor == Color.black) _walls.SetTile(Vector3Int.FloorToInt(position), _roomGenerator.GetTileFromSprite(pack.walls[Random.Range(0, pack.walls.Length)]));
                    else if (patternColor == Color.green && Random.Range(0f, 100f) <= gRoom.probabilityGreenTrap)
                    {
                        Debug.Log("Spawn Pikes");
                        Instantiate(data.traps[2].prefab, position + new Vector3(0.5f, 0.5f, 0.0f), Quaternion.identity, traps);  // Spawn Pikes
                    }
                    else if (patternColor == Color.red && Random.Range(0f, 100f) <= gRoom.probabilityRedTrap)
                    {

                    }
                }
            }
        }
    }

    Texture2D GetPattern(Vector2Int roomSize)
    {
        Pattern pattern = data._patterns[Random.Range(0, data._patterns.Length)];
        int maxSize = roomSize.x > roomSize.y ? roomSize.x : roomSize.y;
        int index = 0;

        if (maxSize < 21) // Was 15
        {
            index = 0;
        }
        else if (maxSize < 30) // Was 21
        {
            index = 1;
        }
        else
        {
            index = 2;
        }

        return pattern.sprites[index].texture;
    }

    GenerationRoom GetRandomRoom()
    {
        foreach (GenerationRoom randomSizeRoom in data._randomSizeRooms)
        {
            if (donjonLevel >= randomSizeRoom.minimumLevel && Random.Range(0f, 100f) <= randomSizeRoom.probability)
            {
                return randomSizeRoom;
            }
        }
        return new GenerationRoom();
    }

    Vector2Int RandomSizeRoom(GenerationRoom room)
    {
        Vector2Int size = new Vector2Int(Random.Range(room.minSize, room.maxSize), Random.Range(room.minSize, room.maxSize));

        while (size.x % 2 == 0 || size.y % 2 == 0)
        {
            size = new Vector2Int(Random.Range(room.minSize, room.maxSize), Random.Range(room.minSize, room.maxSize));
        }
        return size;
    }
}
