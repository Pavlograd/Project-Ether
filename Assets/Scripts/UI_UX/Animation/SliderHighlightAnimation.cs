using UnityEngine;

public class SliderHighlightAnimation : MonoBehaviour
{
    private Animator _animator;
    private float _startTime;
    private float _timeBeforeActivate;
    private float _occurence = 3f;
    // private RectTransform _parentRect;

    void Start()
    {
        // _parentRect = transform.parent.GetComponent<RectTransform>();
        // Debug.Log(_parentRect.localPosition.x);
        // GetComponent<RectTransform>().anchoredPosition = new Vector3(-200, 0, transform.position.z);
        // transform.position = new Vector3(-(_parentRect.anchoredPosition.x * 2), 0, transform.position.z);
        _animator = GetComponent<Animator>();
        _startTime = Time.time;
    }

    private void Update()
    {
        if ((Time.time - _startTime) > _occurence) {
            _animator.Play("SlideEffect");
            _startTime = Time.time;
        }
    }
}
