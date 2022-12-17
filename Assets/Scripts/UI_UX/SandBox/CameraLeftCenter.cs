using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLeftCenter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Invoke("MoveCamera", 0.2f);
    }

    void MoveCamera()
    {
        transform.position = new Vector3(transform.position.x * 3f, transform.position.y, transform.position.z);
        Destroy(this);
    }
}
