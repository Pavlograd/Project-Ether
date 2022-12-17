using System;
using UnityEngine;

public class Credits : MonoBehaviour
{
    [SerializeField] private Transform creditsContent;
    private Vector2 savePos;
    private float speed = 65f;
    private float speedMult = 4.5f;
    private bool isSpeedUp = false;

    private void Start()
    {
        savePos = creditsContent.localPosition;
    }

    private void Update()
    {
        creditsContent.position += new Vector3(0f, Time.deltaTime * speed, 0f);
        if (creditsContent.localPosition.y >= -savePos.y) {
            gameObject.SetActive(false);
            Reset();
        }
    }

    public void SpeedUp()
    {
        isSpeedUp = !isSpeedUp;
        if (isSpeedUp)
            speed *= speedMult;
        else
            speed /= speedMult;
    }

    public void Reset()
    {
        creditsContent.localPosition = savePos;
        if (isSpeedUp)
            SpeedUp();
    }
}
