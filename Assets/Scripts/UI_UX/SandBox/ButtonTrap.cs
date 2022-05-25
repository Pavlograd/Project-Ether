using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTrap : MonoBehaviour
{
    [SerializeField] Image _image;
    [SerializeField] GameObject trap;
    private RoomEditor _roomEditor;

    void Start()
    {
        _roomEditor = GameObject.Find("RoomEditor").GetComponent<RoomEditor>();
    }

    public void ChangeTrap()
    {
        _roomEditor.ChangeCurrentTrap(trap, _image.sprite);
    }
}
