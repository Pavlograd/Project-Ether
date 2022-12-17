using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

[Serializable]
public enum Selection
{
    TEXTURE, TRAP, MOB, PORTAL, DECOR, DELETE, ROOM_CREATOR, ROOM, NONE
}

public class SandBoxManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] DonjonLoaderV2 donjonLoaderV2;
    [SerializeField] ClickAndDrag clickAndDrag;
    [SerializeField] RoomCreator roomCreator;
    [SerializeField] RoomMover roomMover;
    [SerializeField] TrapMover trapMover;
    [SerializeField] EditElement editElement;
    [Header("ComponentsUI")]
    public UIManager uIManager;
    public UIManager uIManagerWorldSpace;
    public PopUps popUps;
    [Header("Edition")]
    public GameObject selectedObject = null;
    public Selection selection = Selection.NONE;
    [SerializeField] Room temporaryRoom;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void SwitchSelection(string _selection)
    {
        selection = (Selection)Enum.Parse(typeof(Selection), _selection);

        // Enable or disable components
        clickAndDrag.enabled = selection == Selection.NONE;
        clickAndDrag.panning = false;
        roomCreator.enabled = selection == Selection.ROOM_CREATOR;
        roomCreator.panning = false;
        roomMover.enabled = selection == Selection.ROOM;
        roomMover.panning = false;
        trapMover.enabled = selection == Selection.TRAP || selection == Selection.MOB;
        trapMover.panning = false;

        if (selection == Selection.TRAP)
        {
            uIManager.ShowElement("Traps");
        }
        else if (selection == Selection.MOB)
        {
            uIManager.ShowElement("Mobs");
        }
    }

    public Vector3 GetMousePosition()
    {
        Vector3 mousePosF = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int mousePos = new Vector3Int(Mathf.RoundToInt(mousePosF.x), Mathf.RoundToInt(mousePosF.y));

        return mousePos;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && !Global.IsPointerOverUIObject() && selection == Selection.TEXTURE)
        {
            GameObject tmpSelectedObject = donjonLoaderV2.GetObjectAtPosition(GetMousePosition());

            if (tmpSelectedObject != null)
            {
                SelectElement(tmpSelectedObject);
            }
        }
    }

    public void SelectElement(GameObject element)
    {
        donjonLoaderV2.UnSelectAll();
        selectedObject = element;

        if (selectedObject.GetComponent<SpriteRenderer>() != null)
        {
            selectedObject.GetComponent<SpriteRenderer>().material.color = Color.cyan;
        }

        uIManagerWorldSpace.ShowElement("EditElement");
        editElement.ShowButtons(donjonLoaderV2.GetElement(selectedObject)); // only shows buttons for this type of element
        editElement.target = selectedObject.transform;
    }

    public void SwitchLayer()
    {
        if (selectedObject == null) return;

        Room room = donjonLoaderV2.GetRoomFromPosition(selectedObject.transform.position);
        RoomElement elementType = room.SwitchLayer(selectedObject);

        if (elementType != RoomElement.DOOR)
        {
            // Switch sprite to wall or floor now
            selectedObject.GetComponent<SpriteRenderer>().sprite = donjonLoaderV2.GetTexture(selectedObject.GetComponent<SpriteRenderer>().sprite.name, elementType != RoomElement.FLOOR);
        }

        donjonLoaderV2.CreateDoorsPath();
    }


    // MOB AND TRAP

    public void ChangeTrap()
    {
        uIManager.ShowElement("Traps");
    }

    public void ChangeMob()
    {
        uIManager.ShowElement("Mobs");
    }

    public void PlaceTemporaryTrap(string elementName)
    {
        uIManager.HideElement("Traps");
        PlaceTemporaryElement(elementName, RoomElement.TRAP);
    }

    public void PlaceTemporaryMob(string elementName)
    {
        // UI
        uIManager.HideElement("Mobs");

        // Check if there is already a boss in the donjon
        if (elementName == "BossNotPlayer" && GameObject.Find("BossNotPlayer") != null)
        {
            //UI
            popUps.ShowError("There is already a Boss placed");

            return;
        }
        PlaceTemporaryElement(elementName, RoomElement.MOB);
    }

    void PlaceTemporaryElement(string elementName, RoomElement elementType)
    {
        Vector3 position = Camera.main.ScreenToViewportPoint(new Vector2(Screen.height / 2, Screen.width / 2));
        position += clickAndDrag.transform.position;
        Vector3Int roundedPos = new Vector3Int(Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.y));

        temporaryRoom.DestroyElements(); // Empty just in case
        donjonLoaderV2.CreateElement(temporaryRoom, new TileClass(roundedPos, elementName), true, elementType);

        editElement.ShowButtons(elementType); // only shows buttons for this type of element
        trapMover.target = selectedObject.transform;
    }

    public void TryPlaceTrap()
    {
        TryPlaceElement(RoomElement.TRAP);
    }

    public void TryPlaceMob()
    {
        TryPlaceElement(RoomElement.MOB);
    }

    void TryPlaceElement(RoomElement elementType)
    {
        Vector3 position = selectedObject.transform.position;

        // Check elements in rooms
        // GetELement check first traps and mobs so if == FLOOR then it's not on a trap or mob

        if (donjonLoaderV2.GetElementAtPosition(position) == RoomElement.FLOOR)
        {
            Room room = donjonLoaderV2.GetRoomFromPosition(position);
            room.AddElement(selectedObject, elementType);
            selectedObject.transform.parent = room.parents[(int)elementType];
            temporaryRoom.elements[elementType].Clear(); // Clear for future element or else previous one will be destroyed
            Unselect();
            SwitchSelection("NONE");
        }
        else if (donjonLoaderV2.GetElementAtPosition(position) == RoomElement.NONE)
        {
            Debug.Log("Incorrect not inside a room");
            popUps.ShowError("place element inside a room");
        }
        else if (donjonLoaderV2.GetElementAtPosition(position) == elementType)
        {
            Debug.Log("Switch elements");
            GameObject currentElement = donjonLoaderV2.GetObjectAtPosition(position);

            Room room = donjonLoaderV2.GetRoomFromPosition(position);
            room.RemoveElement(currentElement, elementType);
            Destroy(currentElement); // Destroy element which was there
            room.AddElement(selectedObject, elementType);

            selectedObject.transform.parent = room.parents[(int)elementType];
            temporaryRoom.elements[elementType].Clear(); // Clear for future element or else previous one will be destroyed

            Unselect();
            SwitchSelection("NONE");
        }
        else
        {
            popUps.ShowError("place element on an empty floor");
        }
    }

    public void DeleteElement()
    {
        Room room = donjonLoaderV2.GetRoomFromPosition(selectedObject.transform.position);

        if (room != null)
        {
            room.RemoveElement(selectedObject, room.GetElementType(selectedObject));
        }

        DestroyElement();
    }


    // TEXTURES


    private bool allTextures;


    public void SwitchTexture()
    {
        allTextures = false;
        uIManager.ShowElement("Textures");
    }

    public void SwitchTextures()
    {
        allTextures = true;
        uIManager.ShowElement("Textures");
    }

    public void ChangeTexture(Sprite texture)
    {
        Room room = donjonLoaderV2.GetRoomFromPosition(selectedObject.transform.position);

        if (allTextures)
        {
            room.ChangeTextures(selectedObject, texture);
        }
        else
        {
            room.ChangeTexture(selectedObject, texture);
        }
        uIManager.HideElement("Textures");
    }


    // ROOM


    public void SelectRoom()
    {
        if (selectedObject == null) return;

        SwitchSelection("ROOM");
        donjonLoaderV2.UnSelectAll();
        uIManagerWorldSpace.HideElement("EditElement");

        Room room = donjonLoaderV2.GetRoomFromPosition(selectedObject.transform.position);
        room.SelectAll();

        roomMover.target = room.transform;
    }

    public void DestroyElement()
    {
        Destroy(selectedObject);
        Unselect();
    }

    public void Unselect()
    {
        selectedObject = null;
        editElement.target = null;
        donjonLoaderV2.UnSelectAll();
        uIManagerWorldSpace.HideElement("EditElement");
    }
}
