using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Pathfinding;

public class DonjonSaverV2 : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] DonjonLoaderV2 donjonLoaderV2;
    [SerializeField] private Seeker seeker;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // Check if the donjon is correct for the save
    bool CheckIfCanSave()
    {
        if (donjonLoaderV2.GetElementAtPosition(Vector3.zero) != RoomElement.FLOOR)
        {
            // UI
            donjonLoaderV2.sandBoxManager.popUps.ShowError("Spawn is not on a floor");

            return false;
        }

        // Check if there is a boss
        // Be careful to call it before checking path between spawn and boss
        if (GameObject.Find("BossNotPlayer") == null)
        {
            // UI
            donjonLoaderV2.sandBoxManager.popUps.ShowError("Place a boss in a room");

            return false;
        }

        // Check path here
        if (!CheckPath())
        {
            // UI
            donjonLoaderV2.sandBoxManager.popUps.ShowError("Create a correct path to the boss");

            return false;
        }

        return true;
    }

    // Return true if can travel to boss
    bool CheckPath()
    {
        GridGraph gg = AstarPath.active.data.gridGraph; // This get the first (and unique) graph
        List<GameObject> doors = donjonLoaderV2.GetDoors();

        // Create fake walls for path
        // Prevent boss in a room not connected to spawn
        foreach (GameObject item in doors)
        {
            Door door = item.GetComponent<Door>();

            door.OpenInstant();
        }

        AstarPath.active.Scan(); // Rescan to manually update graph

        GraphNode node1 = AstarPath.active.GetNearest(Vector3.zero, NNConstraint.Default).node;
        GraphNode node2 = AstarPath.active.GetNearest(GameObject.Find("BossNotPlayer").transform.position, NNConstraint.Default).node;

        // Destroy fake path
        foreach (GameObject item in doors)
        {
            Door door = item.GetComponent<Door>();

            door.Close();
        }

        // Clean path
        donjonLoaderV2.CreateDoorsPath();

        // No path found between the position ?
        if (!PathUtilities.IsPathPossible(node1, node2))
        {
            return false;
        }

        return true;
    }

    public void Save()
    {
        if (!CheckIfCanSave()) return; // Check is unsucessfull;

        donjonLoaderV2.sandBoxManager.uIManager.ShowText("TextWait");

        Invoke("SaveLater", 0.0f);
    }

    void SaveLater()
    {
        if (File.Exists(Application.persistentDataPath + "/save.json"))
        {
            string fileContents = File.ReadAllText(Application.persistentDataPath + "/save.json");
            PlayerClass player = JsonUtility.FromJson<PlayerClass>(fileContents);

            DonjonClass donjonClass = new DonjonClass();

            donjonClass.tested = true;

            foreach (Room room in donjonLoaderV2.rooms)
            {
                donjonClass.rooms.Add(room.GetRoomClass());
            }

            // API CALL
            API.PostUserDonjon(JsonUtility.ToJson(donjonClass));

            player.donjon = donjonClass;

            string json = JsonUtility.ToJson(player);
            File.WriteAllText(Application.persistentDataPath + "/save.json", json);
            SceneManager.LoadScene("Main Menu");
        }
    }
}
