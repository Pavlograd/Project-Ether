using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelsGenerator : MonoBehaviour
{
    [SerializeField] GameObject Pixel;
    int startX = -250;
    int startY = -525;

    // Script necessary because build doesn't order prefabs
    void Start()
    {
        Quaternion rot = Quaternion.Euler(0.0f, 0.0f, 0.0f);

        for (int i = 0; i < 16; i++)
        {
            startY = -525;

            for (int y = 0; y < 16; y++)
            {
                GameObject pixel = Instantiate(Pixel, Vector3.zero, rot, transform);

                // Can't set position from world
                pixel.transform.localPosition = new Vector3(startX, startY, 0);

                startY += 75;
            }

            startX += 75;
        }

        // Script is now useless
        Destroy(this);
    }
}
