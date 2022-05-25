using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLoadRoom : MonoBehaviour
{
    public int indexRoom = 0;
    private LoadRoom _roomLoader;

    // Start is called before the first frame update
    void Start()
    {
        _roomLoader = GameObject.Find("Load").GetComponent<LoadRoom>();
    }

    public void LoadRoom()
    {
        _roomLoader.LoadRoomFromSave(indexRoom);
    }
}
