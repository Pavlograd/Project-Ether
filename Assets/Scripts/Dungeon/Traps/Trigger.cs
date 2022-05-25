using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    // Start is called before the first frame update
    Collider m_ObjectCollider;
    void Start()
    {
        m_ObjectCollider = GetComponent<Collider>();
        m_ObjectCollider.isTrigger = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger On : " + m_ObjectCollider.isTrigger);
    }

}
