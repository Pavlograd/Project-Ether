using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonsTexturesGenerator : MonoBehaviour
{
    private List<Sprite> _textures;
    [SerializeField] private GameObject _content;

    [SerializeField] private GameObject _button;
    // [SerializeField] private LoadTextures _loadTextures;

    [SerializeField] private playerInformation _pI;
    private Dictionary<int, PlayerAnimationController> _pacList;

    public int skinIdSelected = 0;

    // Use start not awake please
    void Start()
    {
        _pacList = _pI.GetPacList();

        // Position of the first button
        Vector3 position = new Vector3(80.0f, -150.0f, 0.0f);

        // Futur parent of buttons
        RectTransform contentTransform = _content.GetComponent<RectTransform>();

        for (int i = 0; i < _pacList.Count; ++i)
        {
            GameObject newButton = Instantiate(_button, position, Quaternion.identity, contentTransform);
            newButton.transform.localPosition = position;
            TMP_Text itemID = newButton.transform.Find("ID").GetComponent<TMP_Text>();
            itemID.SetText("" + i);
            newButton.GetComponent<Button>().onClick.AddListener(delegate { SetSkinId(itemID); });

            // Change Sprite Image
            newButton.GetComponent<Image>().sprite = _pacList[i].preview;

            position.x += 120.0f;

            // Next life if too on the right
            if (position.x >= 1200.0f)
            {
                position.x = 80.0f;
                position.y -= 120.0f;
            }
        }
    }

    private void SetSkinId(TMP_Text text)
    {
        // Debug.Log(text);
        CrossSceneInfos.skinId = text.text;
        _pI.SetPreview();
    }
}