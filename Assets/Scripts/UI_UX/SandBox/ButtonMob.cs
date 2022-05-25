using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonMob : MonoBehaviour
{
    [SerializeField] Image _image;
    [SerializeField] GameObject mob;
    private RoomEditor _roomEditor;

    // Start is called before the first frame update
    void Start()
    {
        _roomEditor = GameObject.Find("RoomEditor").GetComponent<RoomEditor>();
    }

    public void ChangeMob()
    {
        _roomEditor.ChangeCurrentMob(mob, _image.sprite);
    }
}
