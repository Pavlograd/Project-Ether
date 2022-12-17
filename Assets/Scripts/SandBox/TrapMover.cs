using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrapMover : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] SandBoxManager sandBoxManager;
    [Header("Variables")]
    Vector3 startPos;
    Vector3 finalPos;
    public bool panning = false;
    public Transform target;

    private void Update()
    {
        // When LMB clicked get mouse click position and set panning to true
        if (Input.GetMouseButtonDown(0) && !panning && !Global.IsPointerOverUIObject())
        {
            startPos = sandBoxManager.GetMousePosition();

            panning = true;
        }
        // If LMB is already clicked, move the camera following the mouse position update
        if (panning)
        {
            if (finalPos != sandBoxManager.GetMousePosition() && target != null) // Optimization
            {
                finalPos = sandBoxManager.GetMousePosition();
                Vector3 distance = finalPos - startPos;

                target.position += new Vector3Int((int)distance.x, (int)distance.y, 0);

                // Reset startPos
                startPos = sandBoxManager.GetMousePosition();
            }
        }

        // If LMB is released, stop moving the camera
        if (Input.GetMouseButtonUp(0))
            panning = false;
    }
}
