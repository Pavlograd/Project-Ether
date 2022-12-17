using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PopupManager : MonoBehaviour
{
    [SerializeField] GameObject _popupBackgroundTemplate;
    [SerializeField] float _timeToOpen = .1f;
    [SerializeField] float _timeToClose = .2f;
    GameObject _background;

    private void Awake()
    {
        transform.localScale = Vector3.zero;
    }

    public void Open()
    {
        gameObject.SetActive(true);
        if (_popupBackgroundTemplate) {
            AddBackground();
        }
        transform
            .DOScale(Vector3.one, _timeToOpen)
            .SetUpdate(true)
            .SetEase(Ease.OutBack);
    }

    public void Open(Action callback)
    {
        gameObject.SetActive(true);
        if (_popupBackgroundTemplate) {
            AddBackground();
        }
        transform
            .DOScale(Vector3.one, _timeToOpen)
            .SetUpdate(true)
            .SetEase(Ease.OutBack)
            .OnComplete(() => {
                callback();
            });
    }

    public void Close()
    {
        transform
            .DOScale(Vector3.zero, _timeToClose)
            .SetUpdate(true)
            .SetEase(Ease.InBack)
            .OnComplete(() => {
                gameObject.SetActive(false);
                if (_background) {
                    Destroy(_background);
                }
            });
    }

    public void Close(Action callback)
    {
        transform
           .DOScale(Vector3.zero, _timeToClose)
           .SetUpdate(true)
           .SetEase(Ease.InBack)
           .OnComplete(() => {
              gameObject.SetActive(false);
              Destroy(_background);
              callback();
          });
    }

    private void AddBackground()
    {
        _background = Instantiate(_popupBackgroundTemplate, this.transform.parent);

        Button button = _background.AddComponent<Button>();
        button.onClick.AddListener(this.Close);

        _background.transform.SetSiblingIndex(this.transform.GetSiblingIndex());
    }
}
