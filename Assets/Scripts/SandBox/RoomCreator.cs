using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RoomCreator : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] SandBoxManager sandBoxManager;
    [SerializeField] DonjonLoaderV2 donjonLoaderV2;
    [Header("Variables")]
    [SerializeField] Room temporaryRoom;
    Vector3 startPos;
    Vector3 finalPos;
    public bool panning = false;

    private void Update()
    {
        // When LMB clicked get mouse click position and set panning to true
        if (Input.GetMouseButtonDown(0) && !panning && !Global.IsPointerOverUIObject())
        {
            Debug.Log("not on ui");
            sandBoxManager.uIManagerWorldSpace.HideElement("RoomCreation");
            startPos = sandBoxManager.GetMousePosition();
            panning = true;
        }

        if (panning)
        {
            if (finalPos != sandBoxManager.GetMousePosition()) // Optimization prevent creating room everyFrame
            {
                finalPos = sandBoxManager.GetMousePosition();
                CreateTemporaryRoom(startPos, finalPos);
            }
        }

        // If LMB is released, stop moving the camera
        if (Input.GetMouseButtonUp(0) && panning)
        {
            sandBoxManager.uIManagerWorldSpace.ShowElement("RoomCreation");

            RectTransform roomCreation = sandBoxManager.uIManagerWorldSpace.GetElement("RoomCreation");

            roomCreation.position = (finalPos + startPos) / 2f; // Calculate midpoint

            panning = false;
        }
    }

    // Create a temporary room to show player the room they want
    public void CreateTemporaryRoom(Vector3 startPos, Vector3 finalPos)
    {
        // Clear old temporary room
        temporaryRoom.DestroyElements();

        List<TileClass> floors = new List<TileClass>();
        List<TileClass> borders = new List<TileClass>();

        // BORDERS

        Vector3 position = startPos;

        while (position.x != finalPos.x + (startPos.x < finalPos.x ? 1 : -1)) // reach finalPos and add one for final row
        {
            borders.Add(new TileClass(position, "dungeon_textures_7"));
            borders.Add(new TileClass(new Vector3(position.x, finalPos.y), "dungeon_textures_7"));

            position.x += startPos.x < finalPos.x ? 1 : -1;
        }

        position = startPos;

        while (position.y != finalPos.y)
        {
            if (position.y != startPos.y) // don't do first occurence as it has been done just before
            {
                borders.Add(new TileClass(position, "dungeon_textures_7"));
                borders.Add(new TileClass(new Vector3(finalPos.x, position.y), "dungeon_textures_7"));
            }

            position.y += startPos.y < finalPos.y ? 1 : -1;
        }

        // FLOORS

        if (Vector3.Distance(startPos, finalPos) >= 3f)
        {
            startPos.x += startPos.x < finalPos.x ? 1 : -1;
            startPos.y += startPos.y < finalPos.y ? 1 : -1;
            finalPos.y += startPos.y < finalPos.y ? -1 : 1;

            position = startPos;

            while (position.x != finalPos.x)
            {
                position.y = startPos.y;

                while (position.y != finalPos.y + (startPos.y < finalPos.y ? 1 : -1))
                {
                    floors.Add(new TileClass(position, "dungeon_textures_24"));
                    position.y += startPos.y < finalPos.y ? 1 : -1;
                }

                position.x += startPos.x < finalPos.x ? 1 : -1;
            }
        }

        // Loads elements
        donjonLoaderV2.LoadTiles(temporaryRoom, borders, RoomElement.BORDER);
        donjonLoaderV2.LoadTiles(temporaryRoom, floors, RoomElement.FLOOR);
    }

    // Function to check if possible to create a room
    // Only create the room yet
    // Need to check later collisions with other rooms
    public void TryCreatingRoomFromTemporary()
    {
        Debug.Log("Try create room");
        RoomClass room = new RoomClass();

        room.floors = temporaryRoom.GetTiles(temporaryRoom.elements[RoomElement.FLOOR]);
        room.borders = temporaryRoom.GetTiles(temporaryRoom.elements[RoomElement.BORDER]);

        foreach (TileClass item in room.floors)
        {
            if (donjonLoaderV2.GetElementAtPosition(new Vector3(item.x, item.y, 0)) != RoomElement.NONE)
            {
                // Error
                sandBoxManager.popUps.ShowError("Elements are blocking the room");
                return;
            }
        }

        foreach (TileClass item in room.borders)
        {
            if (donjonLoaderV2.GetElementAtPosition(new Vector3(item.x, item.y, 0)) != RoomElement.NONE)
            {
                // Error
                sandBoxManager.popUps.ShowError("Elements are blocking the room");
                return;
            }
        }

        donjonLoaderV2.LoadRoomFromRoom(room);
        sandBoxManager.SwitchSelection("TEXTURE");

        HideCreation();
    }

    public void HideCreation()
    {
        sandBoxManager.uIManagerWorldSpace.HideElement("RoomCreation");
        temporaryRoom.DestroyElements();
    }
}

public class Global
{
    public static bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }
}