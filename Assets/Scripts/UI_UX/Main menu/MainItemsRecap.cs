using System.Collections;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class MainItemsRecap : MonoBehaviour
{
    private API_inventories _inventory;
    private API_User_Datas _user_data;
    private static int _currentGold;
    private bool _isClosed = true;
    private Vector3 _closedRotate = new Vector3(0, 0, 0);
    private Vector3 _openRotate = new Vector3(0, 0, 180);
    [SerializeField] private RectTransform _dropdown;
    [SerializeField] private RectTransform _button;
    [SerializeField] private TMP_Text _goldText;
    [SerializeField] private TMP_Text _soulsText;
    [SerializeField] private TMP_Text _abilityDustText;
    [SerializeField] private TMP_Text _crystalsText;
    [SerializeField] private TMP_Text _mentoringText;
    public bool goldIncreaseAnimation = false;

    void Start()
    {
        _user_data = API.GetUserDatas();
        _inventory = API.GetInventory();
        updateTexts();
    }

    public void updateTexts()
    {
        _crystalsText.text = _user_data.crystal.ToString();
        _mentoringText.text = _user_data.mentoring.ToString();
        for (int i = 0; i < _inventory.inventories.Count; i++) {
            if (_inventory.inventories[i].name == "Ability dust") {
                _abilityDustText.text = _inventory.inventories[i].quantity;
            } else if (_inventory.inventories[i].name == "Souls") {
                _soulsText.text = _inventory.inventories[i].quantity;
            }
        }
    }

    public void updateGoldText()
    {
        _goldText.text = _user_data.cash.ToString();
        _currentGold = _user_data.cash;
    }

    public IEnumerator updateGoldWithAnimation(float duration, float delayBeforeStart)
    {
        float stepDelay = 0.1f;
        int newGoldAmount = _user_data.cash;
        float goldAdded = 0;

        int amountDiff = newGoldAmount - _currentGold;
        float stepGoldIncrease = (float)amountDiff * stepDelay / duration;

        _goldText.text = _currentGold.ToString();
        yield return new WaitForSeconds(delayBeforeStart);
        while (goldAdded <= amountDiff) {
            yield return new WaitForSeconds(stepDelay);
            _goldText.text = (_currentGold + (int)goldAdded).ToString();
            goldAdded += stepGoldIncrease;
        }
        updateGoldText();
    }

    public void OpenDropdown()
    {
        _dropdown
            .DOScaleY(_isClosed ? 1 : 0, .1f)
            .SetEase(Ease.InOutSine);
        _button
            .DOLocalRotate(_isClosed ? _openRotate : _closedRotate, .15f)
            .SetEase(Ease.InOutSine);
        _isClosed = !_isClosed;
    }
}
