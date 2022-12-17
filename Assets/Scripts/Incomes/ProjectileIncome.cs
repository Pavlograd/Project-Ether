using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileIncome : MonoBehaviour
{
    float speed = 2.0f;

    void FixedUpdate()
    {
        transform.position += Vector3.right * Time.deltaTime * speed;
    }
}
