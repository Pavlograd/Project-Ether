using UnityEngine;

public class ZaxisStaticSprite : MonoBehaviour
{
    void Update()
    {
        if (gameObject.transform.rotation.z != 0) {
            gameObject.transform.rotation = new Quaternion(0, 0, 0, 1);
        }
    }
}
