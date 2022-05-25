using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomInit : MonoBehaviour
{
    [SerializeField] GameObject portals;

    // Start is called before the first frame update
    void Start()
    {
        portals.transform.localPosition = Vector3.back;
    }
}
