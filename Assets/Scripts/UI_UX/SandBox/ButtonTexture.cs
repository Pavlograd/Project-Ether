using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonTexture : MonoBehaviour
{
    [SerializeField] Image image;
    private RoomEditor _roomEditor;

    void Start()
    {
        _roomEditor = GameObject.Find("RoomEditor").GetComponent<RoomEditor>();
    }

    public void ChangeTexture()
    {
        _roomEditor.ChangeCurrentTexture(image.sprite);
    }
}
