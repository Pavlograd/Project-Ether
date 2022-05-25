using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LoadDonjon : MonoBehaviour
{
    [SerializeField] private LoadRoom roomLoader;
    [SerializeField] private GameObject _mobs;
    [SerializeField] private GameObject _player;
    [SerializeField] private Tilemap _ground;
    [SerializeField] private RoomClass _startIsland;
    bool _testDonjon = false;
    string _donjonPath;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Coroutine());

        _testDonjon = CrossSceneInfos.testDonjon;

        // If there is no level to load keep it to default
        _donjonPath = CrossSceneInfos.donjonPath == null ? Application.persistentDataPath + "/save.json" : CrossSceneInfos.donjonPath;
    }

    public IEnumerator Coroutine()
    {
        while (!roomLoader.isStarted)
        {
            yield return new WaitForSeconds(0.05f);
        }

        if (File.Exists(_donjonPath))
        {
            // Read the entire file and save its contents.
            string fileContents = File.ReadAllText(_donjonPath);

            // Deserialize the JSON data 
            // into a pattern matching the PlayerData class.
            PlayerClass player = JsonUtility.FromJson<PlayerClass>(fileContents);
            // Debug.Log(player.donjon.rooms[0].name);

            roomLoader.offset = 100;

            for (int i = 0; i < player.donjon.rooms.Count; i++)
            {
                roomLoader.activeRoom = i;
                roomLoader.LoadRoomFromSave(i);
                roomLoader.offset += 100;
            }

            LoadStartIsland();

            ReactivateMobs();

            // Useless if first room is always start island
            //SetInitialPositionPlayer();

            // Need to change later
            if (_testDonjon) ValidateDonjon();
        }
    }

    void LoadStartIsland()
    {
        roomLoader.offset = 0;
        roomLoader.activeRoom = -1;
        roomLoader.LoadRoomFromRoom(_startIsland);

        GameObject portals = GameObject.Find("Portals");
        Portal portalStartIsland;
        Portal portalConnectToStartIsland;

        Portal[] childScripts = portals.GetComponentsInChildren<Portal>();

        // Prevent compilation error
        portalStartIsland = childScripts[0];
        portalConnectToStartIsland = childScripts[0];

        for (int i = 0; i < childScripts.Length; i++)
        {
            Portal portal = childScripts[i];

            if (portal.room == -1) portalStartIsland = portal;

            if (portal.roomConnected == -1 && portal.room != -1) portalConnectToStartIsland = portal;
        }

        portalStartIsland.roomConnected = portalConnectToStartIsland.room;
        portalStartIsland.portalConnected = portalConnectToStartIsland.portal;
    }

    void ReactivateMobs()
    {
        AIHealthManager[] childScripts = _mobs.GetComponentsInChildren<AIHealthManager>();

        for (int i = 0; i < childScripts.Length; i++)
        {
            GameObject myChild = childScripts[i].gameObject;

            myChild.GetComponent<AIMovementManager>().enabled = true;
            myChild.GetComponent<AIAbilityManager>().enabled = true;
            myChild.GetComponent<BoxCollider2D>().enabled = true;
        }
    }

    // Set position of player to first tile
    void SetInitialPositionPlayer()
    {
        BoundsInt bounds = _ground.cellBounds;
        TileBase[] allTiles = _ground.GetTilesBlock(bounds);

        foreach (var position in _ground.cellBounds.allPositionsWithin)
        {
            if (!_ground.HasTile(position))
            {
                continue;
            }

            _player.transform.position = position;
            return;
        }
    }

    void ValidateDonjon()
    {
        string fileContents = File.ReadAllText(Application.persistentDataPath + "/save.json");
        PlayerClass player = JsonUtility.FromJson<PlayerClass>(fileContents);

        player.donjon.tested = true;

        string json = JsonUtility.ToJson(player);
        File.WriteAllText(Application.persistentDataPath + "/save.json", json);
    }
}
