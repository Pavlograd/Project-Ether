using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickAndDrag : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] Transform particles;
    Vector2 mouseClickPos;
    Vector2 mouseCurrentPos;
    public bool panning = false;
    float scrollSpeed = 2f;

    private void Update()
    {
        // When LMB clicked get mouse click position and set panning to true
        if (Input.GetMouseButtonDown(0) && !panning && !Global.IsPointerOverUIObject())
        {
            mouseClickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            panning = true;
        }
        // If LMB is already clicked, move the camera following the mouse position update
        if (panning)
        {
            mouseCurrentPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var distance = mouseCurrentPos - mouseClickPos;
            transform.position += new Vector3(-distance.x, -distance.y, 0);
        }

        // If LMB is released, stop moving the camera
        if (Input.GetMouseButtonUp(0))
            panning = false;

        float scroll = 0f;

        if (Input.touchSupported)
        {
            // Pinch to zoom
            if (Input.touchCount == 2)
            {
                // get current touch positions
                Touch tZero = Input.GetTouch(0);
                Touch tOne = Input.GetTouch(1);
                // get touch position from the previous frame
                Vector2 tZeroPrevious = tZero.position - tZero.deltaPosition;
                Vector2 tOnePrevious = tOne.position - tOne.deltaPosition;

                float oldTouchDistance = Vector2.Distance(tZeroPrevious, tOnePrevious);
                float currentTouchDistance = Vector2.Distance(tZero.position, tOne.position);

                // get offset value
                float deltaDistance = oldTouchDistance - currentTouchDistance;

                scroll = deltaDistance / 10f;
            }
        }
        else
        {
            scroll = Input.GetAxis("Mouse ScrollWheel");
        }

        Camera.main.orthographicSize -= scroll * scrollSpeed;
        particles.localScale = Vector3.one * Camera.main.orthographicSize / 10f;
    }
}
