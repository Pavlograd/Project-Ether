using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SkillsLevelUpPanel : MonoBehaviour
{
    private System.Random random = new System.Random();
    private PopupManager _popupManager;
    private Button _upgradeButton;

    private List<GameObject> _requirementsObj = new List<GameObject>();
    private List<TierRequirements> _requirementsData = null;

    private int _parentId = -1;
    private int _id = -1;
    private InventorySpellLoad _selectedAbilityRef;
    private Ability _currentAbility;
    private API_User_Datas _objectUserDatas;
    private API_inventories _inventory = null;

    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _skillLevel;
    [SerializeField] private TMP_Text _upgradedSkillLevel;
    [SerializeField] private TMP_Text _value;
    [SerializeField] private TMP_Text _upgradedValue;
    [SerializeField] private TMP_Text _damages;
    [SerializeField] private TMP_Text _increasedAmount;
    [SerializeField] private TMP_Text _buttonCostText;
    [SerializeField] private TMP_Text _playerMoney;
    [SerializeField] private Image _skillIcon;
    [SerializeField] private TMP_Text _tierTextArrow;
    [SerializeField] private TMP_Text _valueTextArrow;

    [SerializeField] private UpgradeBarController _upgradeBar;
    [SerializeField] private GameObject _requirementItemsList;
    [SerializeField] private GameObject _itemPrefab;

    private void SetTierTexts(int currentLevel) {
        if (currentLevel == 5) {
            _skillLevel.text = "";
            _upgradedSkillLevel.text = "5";
            _tierTextArrow.text = "";
            _upgradeButton.interactable = false;
        } else {
            _skillLevel.text = currentLevel.ToString();
            _tierTextArrow.text = "->";
            _upgradedSkillLevel.text = (currentLevel + 1).ToString();
            _upgradeButton.interactable = true;
        }
    }

    private void SetEffectTexts(Ability ability) {
        _damages.gameObject.SetActive(!(ability.abilityType == AbilityType.BUFF));
        _increasedAmount.gameObject.SetActive(ability.abilityType == AbilityType.BUFF);
        if (ability.lvl == 5) {
            _value.text = "";
            _valueTextArrow.text = "";
            _upgradedValue.text = (ability.GetValueEffect() + ability.tierUpgradesValue[ability.lvl - 1]).ToString();
            return;
        }
        _value.text = (ability.GetValueEffect() + ability.tierUpgradesValue[ability.lvl - 1]).ToString();
        _valueTextArrow.text = "->";
        _upgradedValue.text = (ability.GetValueEffect() + ability.tierUpgradesValue[ability.lvl]).ToString();
    }

    private void SetRequirements(int tier, States state)
    {
        if (_requirementsObj.Count > 0) {
            foreach (var item in _requirementsObj) {
                Destroy(item);
            }
            _requirementsObj.Clear();
        }
        #nullable enable
        _requirementsData = AbilityTiersUpdateInfo.GetTier(tier, state);
        if (_requirementsData != null) {
            for (int i = 0; i < _requirementsData.Count; i++) {
                if (_requirementsData[i].itemData.id == 10) {
                    if (_objectUserDatas.cash < _requirementsData[i].quantity) {
                        _upgradeButton.interactable = false;
                        _buttonCostText.color = Color.red;
                    }
                    _buttonCostText.text = _requirementsData[i].quantity.ToString();
                } else {
                    // Get loot information
                    string name = ItemsDictionary.GetNameById(_requirementsData[i].itemData.id);
                    string? inventoryQuantity = _inventory.inventories.Find(e => e.name == _requirementsData[i].itemData.name)?.quantity;
                    string quantity = $"{(inventoryQuantity == null ? "0" : inventoryQuantity)}/{_requirementsData[i].quantity.ToString()}";
                    Sprite sprite = Resources.Load<Sprite>($"Textures/Shop/Item/{_requirementsData[i].itemData.id}");

                    // Add object to a list
                    GameObject item = Instantiate(_itemPrefab, _requirementItemsList.transform);
                    item.name = name;
                    _requirementsObj.Add(item);

                    // Set the data of the object
                    LootIconManager data = item.GetComponent<LootIconManager>();
                    data.SetIcon(sprite, quantity, name);
                    if (Int32.Parse(inventoryQuantity == null ? "0" : inventoryQuantity) >= _requirementsData[i].quantity) {
                        data.ChangeTextColor(Color.green);
                    } else {
                        data.ChangeTextColor(Color.red);
                        _upgradeButton.interactable = false;
                    }
                }
            }
        } else {
            _buttonCostText.text = "MAX";
            _upgradeButton.interactable = false;
        }
        #nullable disable
    }

    private void UpdateTextToNextTier()
    {
        Ability ability = PlayerSpellInventory.instance.getAbilities().Find(e => e.parentId == _parentId);
        SetupAbilityInformation(ability);
    }

    private void RemoveCostUpgrade()
    {
        API.RemoveCash(int.Parse(_buttonCostText.text));
        for (int i = 0; i < _requirementsData.Count; i++) {
            if (_requirementsData[i].itemData.id == 10) continue;
            int idx = _inventory.inventories.FindIndex(e => e.name == _requirementsData[i].itemData.name);
            int itemId = Int32.Parse(_inventory.inventories[idx]._id);
            string itemName = _inventory.inventories[idx].name;
            int newQuantity = Int32.Parse(_inventory.inventories[idx].quantity) - _requirementsData[i].quantity;
            API.PostInventory(itemId, itemName, newQuantity);
            _inventory.inventories[idx].quantity = newQuantity.ToString();
        }
    }

    private void SetupAbilityInformation(Ability ability)
    {
        _currentAbility = ability;
        _name.text = ability.name;
        _skillIcon.sprite = ability.thumbnail;
        _id = ability.id;
        SetTierTexts(ability.lvl);
        SetEffectTexts(ability);
        SetRequirements(ability.lvl, ability.state);
        _playerMoney.text = $"Current: {_objectUserDatas.cash.ToString()}";
    }

    public void ShowAbilityPanel(int parentId, InventorySpellLoad inventoryRef)
    {
        if (!TryGetComponent<PopupManager>(out _popupManager)) {
            return;
        }
        if (_objectUserDatas == null) {
            _objectUserDatas = API.GetUserDatas();
        }
        if (_inventory == null) {
            _inventory = API.GetInventory();
        }
        if (_upgradeButton == null) {
            _upgradeButton = _buttonCostText.transform.parent.gameObject.GetComponent<Button>();
        }

        _popupManager.Open();
        _selectedAbilityRef = inventoryRef;
        Ability ability = PlayerSpellInventory.instance.getAbilities().Find(e => e.parentId == parentId);
        SetupAbilityInformation(ability);
        _parentId = parentId;
    }

    public void LevelUp()
    {
        if (_upgradeButton.interactable && _selectedAbilityRef != null && _parentId != -1 && _requirementsData != null && _inventory != null && _id != -1) {
            _upgradeButton.interactable = false;
            float rate = AbilityTiersUpdateInfo.GetTierRate(int.Parse(_skillLevel.text));
            if (rate != -1) {
                bool success = random.NextDouble() < rate;
                _upgradeBar.Active(success, () => {
                    _upgradeButton.interactable = true;
                    UpdateTextToNextTier();
                });
                _objectUserDatas.cash -= int.Parse(_buttonCostText.text);
                _playerMoney.text = $"Current: {_objectUserDatas.cash.ToString()}";
                RemoveCostUpgrade();
                if (success) {
                    _selectedAbilityRef.lvlup(_id, _currentAbility.abilityType);
                }
            }
        } else {
            Debug.LogError("Something went wrong during the setup of upgrade process");
        }
    }

    public void OnClose()
    {
        _popupManager.Close(() => {
            foreach (var item in _requirementsObj) {
                Destroy(item);
            }
            _requirementsObj.Clear();
        });
        _selectedAbilityRef = null;
        _parentId = -1;
    }
}
