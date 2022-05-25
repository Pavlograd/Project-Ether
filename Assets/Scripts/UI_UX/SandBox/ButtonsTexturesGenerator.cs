using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsTexturesGenerator : MonoBehaviour
{
    private List<Sprite> _textures;
    [SerializeField] private GameObject _content;
    [SerializeField] private GameObject _button;
    [SerializeField] private LoadTextures _loadTextures;

    // Use start not awake please
    void Start()
    {
        _textures = _loadTextures.textures;

        // Position of the first button
        Vector3 position = new Vector3(80.0f, -150.0f, 0.0f);

        // Futur parent of buttons
        RectTransform contentTransform = _content.GetComponent<RectTransform>();

        foreach (Sprite texture in _textures)
        {
            GameObject newButton = Instantiate(_button, position, Quaternion.identity, contentTransform);

            newButton.transform.localPosition = position;

            // Change Sprite Image
            newButton.GetComponent<Image>().sprite = texture;

            position.x += 120.0f;

            // Next life if too on the right
            if (position.x >= 1200.0f)
            {
                position.x = 80.0f;
                position.y -= 120.0f;
            }
        }

        // Script is now useless
        Destroy(this);
    }
}
