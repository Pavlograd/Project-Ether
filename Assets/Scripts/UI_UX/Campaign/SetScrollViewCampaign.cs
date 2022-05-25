using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SetScrollViewCampaign : MonoBehaviour
{
    [SerializeField] private CampaignManager _campaignManager;
    [SerializeField] private GameObject _buttonLevel;
    [SerializeField] private float _spaceBetweenButtons = 125f;

    public void InitScrollView()
    {
        if (_campaignManager.levels.Count <= 1)
            return;
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, _campaignManager.levels.Count * _spaceBetweenButtons + 25f);
        for (int i = 1; i < _campaignManager.levels.Count; i++) {
            GameObject buttonCreate = Instantiate(_buttonLevel, transform);
            Vector3 buttonPosition = new Vector2(0f, -75f - i * _spaceBetweenButtons);
            buttonCreate.GetComponent<RectTransform>().anchoredPosition = buttonPosition;
            buttonCreate.transform.GetChild(0).GetComponent<Text>().text = "Level " + (i + 1).ToString();
        }
    }
}
