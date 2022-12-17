using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ToggleController : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] Image _targetImage;
    [SerializeField] Sprite _spriteState1;
    [SerializeField] Sprite _spriteState2;
    [SerializeField] UnityEvent<bool> method;
    bool _isOn = true;

    public void InitToogle(bool value)
    {
        if (_animator == null) return;
        _isOn = value;
        UpdateToogleState();
    }

    public void OnValueChange()
    {
        if (_animator == null) return;
        _isOn = !_isOn;
        AudioManager.instance?.PlaySoundEffect(SFX.UI_SWITCH);
        UpdateToogleState();
        method.Invoke(_isOn);
    }

    private void UpdateToogleState()
    {
        _animator.SetTrigger("isToggled");
        if (_isOn) {
            _targetImage.sprite = _spriteState1;
        } else {
            _targetImage.sprite = _spriteState2;
        }
    }
}
