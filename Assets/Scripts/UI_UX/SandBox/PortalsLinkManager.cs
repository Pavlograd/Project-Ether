using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PortalsLinkManager : MonoBehaviour
{
    [SerializeField] private Dropdown _dropdownRooms;
    [SerializeField] private Dropdown _dropdownPortals;
    [SerializeField] private GameObject _content;
    [SerializeField] private GameObject _portals;
    [SerializeField] private SaveRoom _save;
    [SerializeField] private Toggle _toggleStartIsland;
    private ButtonLoadPortal _portal;
    private int[] _portalsInRoom;

    // Start is called before the first frame update
    void Start()
    {
        string fileContents = File.ReadAllText(Application.persistentDataPath + "/save.json");

        // Deserialize the JSON data 
        // into a pattern matching the PlayerData class.
        PlayerClass player = JsonUtility.FromJson<PlayerClass>(fileContents);

        _portalsInRoom = new int[player.donjon.rooms.Count];

        for (var i = 0; i < player.donjon.rooms.Count; i++)
        {
            //string[] portals = player.donjon.rooms[i].portals.Split(',');

            //_portalsInRoom[i] = portals.Length;
        }
    }

    public void ChangeCurrentPortal(ButtonLoadPortal portal)
    {
        _portal = portal;
        Debug.Log("before dropdown" + portal.roomConnected);
        SetDropdownToValues();
    }

    void SetDropdownToValues()
    {
        _toggleStartIsland.isOn = (_portal.roomConnected == -1);

        if (_portal.roomConnected != -1)
        {
            List<string> dropOptions = new List<string>();

            ToggleDropDowns(true);
            _dropdownRooms.value = _portal.roomConnected;
            _dropdownPortals.ClearOptions();

            for (var i = 0; i < _portalsInRoom[_portal.roomConnected]; i++)
            {
                dropOptions.Add(i.ToString());
            }

            _dropdownPortals.AddOptions(dropOptions);
            _dropdownPortals.value = _portal.portalConnected;
        }
        else
        {
            ToggleDropDowns(false);
        }
    }

    public void ToggleStartIslandConnection()
    {
        if (_toggleStartIsland.isOn)
        {
            _portal.roomConnected = -1;
            _portal.portalConnected = -1;
            ToggleDropDowns(false);
        }
        else
        {
            // Default perhaps change later
            ToggleDropDowns(true);
            ChangeRoomSelected(0);
            ChangePortalSelected(0);
        }
    }

    public void ChangeRoomSelected(int room)
    {
        _portal.roomConnected = room;
        _portal.portalConnected = 0;

        SetDropdownToValues();
    }

    public void ChangePortalSelected(int portal)
    {
        _portal.portalConnected = portal;
    }

    public void SavePortals()
    {
        ButtonLoadPortal[] buttonsPortals = _content.GetComponentsInChildren<ButtonLoadPortal>();
        Portal[] portals = _portals.GetComponentsInChildren<Portal>();

        for (int i = 0; i < portals.Length; i++)
        {
            Portal portal = portals[i];
            ButtonLoadPortal buttonPortal = buttonsPortals[i];

            portal.roomConnected = buttonPortal.roomConnected;
            portal.portalConnected = buttonPortal.portalConnected;

            Debug.Log(portal.roomConnected);
        }

        _save.SaveEdition();
    }

    void ToggleDropDowns(bool hide)
    {
        _dropdownRooms.gameObject.SetActive(hide);
        _dropdownPortals.gameObject.SetActive(hide);
    }
}
