using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SetScrollviewUsers : MonoBehaviour
{
    [SerializeField] private UsersAttackManager _userAttackManager;
    [SerializeField] private GameObject _buttonLevel;
    [SerializeField] private float _spaceBetweenButtons = 125f;

    public void InitScrollView()
    {
        if (_userAttackManager.donjons.Count < 1) return;

        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();

        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, _userAttackManager.donjons.Count * _spaceBetweenButtons + 25f);

        for (int i = 0; i < _userAttackManager.donjons.Count; i++)
        {
            GameObject buttonCreate = Instantiate(_buttonLevel, transform);

            buttonCreate.SetActive(true);
            Vector3 buttonPosition = new Vector2(0f, -75f - i * _spaceBetweenButtons);

            buttonCreate.GetComponent<RectTransform>().anchoredPosition = buttonPosition;
            buttonCreate.transform.GetChild(0).GetComponent<Text>().text = _userAttackManager.names[i];
        }
    }
}
