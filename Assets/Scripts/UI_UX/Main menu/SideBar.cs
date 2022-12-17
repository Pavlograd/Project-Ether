using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SideBar : MonoBehaviour
{
    private RectTransform _rect;
    private GameObject _background;
    [SerializeField] private GameObject _popupBackgroundTemplate;

    private void Awake()
    {
        _rect = GetComponent<RectTransform>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Open()
    {
        _rect.DOLocalMoveX(_rect.localPosition.x - 250, .2f)
            .SetEase(Ease.OutSine);
        if (_popupBackgroundTemplate) {
            AddBackground();
        }
    }

    public void Close()
    {
        _rect.DOLocalMoveX(_rect.localPosition.x + 250, .2f)
            .SetEase(Ease.OutSine);
        if (_background) {
            Destroy(_background);
        }
    }

    private void AddBackground()
    {
        _background = Instantiate(_popupBackgroundTemplate, this.transform.parent);

        Button button = _background.AddComponent<Button>();
        button.onClick.AddListener(this.Close);

        _background.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
    }
}
