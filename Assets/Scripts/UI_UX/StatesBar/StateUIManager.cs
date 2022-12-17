using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StateUIManager : MonoBehaviour
{
    private Transform _blackCover;
    private StatesIconHandler _parent;
    private Image _blackCoverImageCpmt;
    private float _delaysSeconds = 5f;
    private float _animationTime;
    public States state;

    private void Awake()
    {
        Transform blackCoverObj = transform.GetChild(0).GetChild(0);
        _blackCoverImageCpmt = blackCoverObj.gameObject.GetComponent<Image>();
        _parent = transform.parent.gameObject.GetComponent<StatesIconHandler>();
    }

    public void StartTimer(float duration)
    {
        _delaysSeconds = duration;
        StartCoroutine(StateTimer());
    }

    public void StartTimer(float duration, float elaspedTime)
    {
        _delaysSeconds = duration;
        _animationTime = _delaysSeconds - elaspedTime;
        StartCoroutine(StateTimerFromValue());
    }

    IEnumerator StateTimer()
    {
        _animationTime = _delaysSeconds;
        for (; _animationTime > 0; _animationTime -= Time.deltaTime) {
            _blackCoverImageCpmt.fillAmount = _animationTime / _delaysSeconds;
            yield return null;
        }
        gameObject.SetActive(false);
        _blackCoverImageCpmt.fillAmount = 1;
        _parent.RemoveState(state);
    }

    IEnumerator StateTimerFromValue()
    {
        for (; _animationTime > 0; _animationTime -= Time.deltaTime) {
            _blackCoverImageCpmt.fillAmount = _animationTime / _delaysSeconds;
            yield return null;
        }
        gameObject.SetActive(false);
        _blackCoverImageCpmt.fillAmount = 1;
        _parent.RemoveState(state);
    }

    public void ResetFillAmount()
    {
        _animationTime = _delaysSeconds;
        _blackCoverImageCpmt.fillAmount = 1;
    }
}
