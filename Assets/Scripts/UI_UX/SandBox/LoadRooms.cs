using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using TMPro;

public class LoadRooms : MonoBehaviour
{
    [SerializeField] private GameObject _content;
    [SerializeField] private GameObject _button;
    [SerializeField] private Dropdown _dropdown;

    // Start is called before the first frame update
    void Start()
    {
        LoadRoomsFromSave();
    }

    void LoadRoomsFromSave()
    {
        // Read the entire file and save its contents.
        string fileContents = File.ReadAllText(Application.persistentDataPath + "/save.json");

        // Deserialize the JSON data 
        // into a pattern matching the PlayerData class.
        PlayerClass player = JsonUtility.FromJson<PlayerClass>(fileContents);
        Vector3 position = new Vector3(-40.0f, -150.0f, 0.0f);
        RectTransform contentTransform = _content.GetComponent<RectTransform>();

        List<string> dropOptions = new List<string>();

        // For portalsMenu
        _dropdown.ClearOptions();

        for (var i = 0; i < player.donjon.rooms.Count; i++)
        {
            RoomClass room = player.donjon.rooms[i];
            GameObject newButton = Instantiate(_button, Vector3.zero, Quaternion.identity, contentTransform);
            newButton.transform.localPosition = position;

            GameObject button = newButton.transform.GetChild(0).gameObject;

            // Set Index Room inn save to button
            button.GetComponent<ButtonLoadRoom>().indexRoom = i;
            // Set room's name as text for button
            button.transform.Find("Text").GetComponent<TMP_Text>().text = room.name;

            dropOptions.Add(room.name);

            position.y += -200.0f;
        }

        _dropdown.AddOptions(dropOptions);

        // Script is now useless
        Destroy(this);
    }
}
