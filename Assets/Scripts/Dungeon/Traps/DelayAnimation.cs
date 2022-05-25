using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayAnimation : MonoBehaviour
{
    [SerializeField] private float _delay = 0.0f;
    [SerializeField] private List <Animator> _animators;

    void Start()
    {
        SetDelayAnimation();
    }

    private void SetDelayAnimation()
    {
        if (_delay == 0.0f) {
            _delay = Random.Range(0.0F, 1.5F);
        }
        Invoke("ReabledAnimation", _delay);
    }

    void ReabledAnimation()
    {
        foreach(Animator _animator in _animators)
        {
            _animator.enabled = true;
        }
        Destroy(this);
    }
}
