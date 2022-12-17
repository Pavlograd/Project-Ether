using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class CameraTargetMover : MonoBehaviour
{
    [SerializeField] Transform target;
    Vector3 offset;
    Vector3 oldPosition;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position;
        oldPosition = transform.position;
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, oldPosition) >= 10f)
        {
            ReScan();
        }
    }

    void FixedUpdate()
    {
        transform.position = target.position + offset;
    }

    void ReScan()
    {
        GridGraph gg = AstarPath.active.data.gridGraph; // This get the first (and unique) graph

        oldPosition = transform.position; // Reset oldPosition

        gg.center = transform.position; // Set center to current player positions

        AstarPath.active.Scan(); // Rescan to manually update graph
    }
}
