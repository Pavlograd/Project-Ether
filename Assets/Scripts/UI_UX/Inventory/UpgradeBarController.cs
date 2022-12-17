using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

public class UpgradeBarController : MonoBehaviour
{
    [SerializeField] private RectTransform _barRect;
    [SerializeField] private ParticleSystem _successParticles;

    public void Active(bool isSuccess, Action onComplete = null)
    {
        _barRect.DOSizeDelta(new Vector2(_barRect.sizeDelta.x, 100), 1f)
            .SetEase(Ease.InCirc)
            .OnComplete(() => {
                StartCoroutine(Reset());
                if (onComplete != null)
                    onComplete();
                if (isSuccess) {
                    _successParticles.Play();
                    AudioManager.instance?.PlaySoundEffect(SFX.POWER_UP_SUCCESS);
                } else {
                    AudioManager.instance?.PlaySoundEffect(SFX.POWER_UP_FAILURE);
                }
            });
    }

    private IEnumerator Reset()
    {
        yield return new WaitForSeconds(.35f);
        _barRect.DOSizeDelta(new Vector2(_barRect.sizeDelta.x, 0), .05f)
            .SetEase(Ease.InQuint);
    }
}
