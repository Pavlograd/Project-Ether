using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContentTraps : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] LoadTextures loadTextures;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(loadTextures.textures.Count);

        foreach (Sprite texture in loadTextures.textures)
        {
            GameObject newObject = Instantiate(prefab, Vector3.zero, Quaternion.identity, transform);

            newObject.GetComponent<Image>().sprite = texture;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
