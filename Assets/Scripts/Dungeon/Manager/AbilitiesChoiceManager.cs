using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilitiesChoiceManager : MonoBehaviour
{
    private int _nbAbility = 0;
    private List<Ability> _finalAbilitiesScriptableObj = new List<Ability>(4);
    private List<Ability> _abilitiesInventory;
    [SerializeField] private List<GameObject> _finalAbilitiesImage;
    [SerializeField] private Transform _tableAbilitiesAvailable;
    [SerializeField] private GameObject _buttonAbilityData;
    [SerializeField] private Sprite _emptyAbility;
    [SerializeField] private PlayerSpellInventory _playerSpell;

    private void Start()
    {
        _abilitiesInventory = PlayerSpellInventory.instance.getAbilities();

        for (int i = 0; i < LevelData.instance.playerAbilities.Count; i++) {
            if (LevelData.instance.playerAbilities[i] != null)
                _finalAbilitiesImage[i].GetComponent<Image>().sprite = LevelData.instance.playerAbilities[i].thumbnail;
            _finalAbilitiesScriptableObj.Add(LevelData.instance.playerAbilities[i]);
        }

        for (int i = 3; i >= _abilitiesInventory.Count; i--) {
            _finalAbilitiesImage[i].transform.parent.GetComponent<Button>().interactable = false;
            _finalAbilitiesImage[i].GetComponent<Image>().color = new Color32(200, 200, 200, 128);
        }

        if (_abilitiesInventory.Count != 0) {
            PrepareAbilitiesAvailable();
            InitScrollView();
        }
    }

    private void PrepareAbilitiesAvailable()
    {
        int x = 0;
        int y = 0;
        for (int i = 1; i < _abilitiesInventory.Count; i++) {
            int iCopy = i;
            x++;
            if (x == 3) {
                x = 0;
                y--;
            }
            GameObject newButtonAbilityData = Instantiate(_buttonAbilityData, _tableAbilitiesAvailable);
            newButtonAbilityData.transform.localPosition = new Vector2(x * 65 + 35, y * 65 - 35);
            newButtonAbilityData.transform.GetChild(0).GetComponent<Image>().sprite = _abilitiesInventory[i].thumbnail;
            newButtonAbilityData.GetComponent<Button>().onClick.AddListener(delegate{ChooseNewAbility(_abilitiesInventory[iCopy]);});
        }
        _buttonAbilityData.transform.GetChild(0).GetComponent<Image>().sprite = _abilitiesInventory[0].thumbnail;
        _buttonAbilityData.GetComponent<Button>().onClick.AddListener(delegate{ChooseNewAbility(_abilitiesInventory[0]);});
    }

    public void ChangeOldAbility(int nbAbility)
    {
        _nbAbility = nbAbility;
    }

    public void ChooseNewAbility(Ability ability)
    {
        for (int i = 0; i < 4; i++) {
            if (i == _nbAbility || _finalAbilitiesScriptableObj[i] == null)
                continue;
            if (_finalAbilitiesScriptableObj[i].name == ability.name) {
                _finalAbilitiesImage[i].GetComponent<Image>().sprite = _emptyAbility;
                _finalAbilitiesScriptableObj[i] = null;
            }
        }
        _finalAbilitiesImage[_nbAbility].GetComponent<Image>().sprite = ability.thumbnail;
        _finalAbilitiesScriptableObj[_nbAbility] = ability;
    }

    public void Validate()
    {
        LevelData.instance.playerAbilities = _finalAbilitiesScriptableObj;
    }

    private void InitScrollView()
    {
        int _sizeOfTableAbilitiesAvailable = (int) Mathf.Ceil((float)_abilitiesInventory.Count / 3);
        if (_sizeOfTableAbilitiesAvailable > 1) {
            RectTransform rectTransform = _tableAbilitiesAvailable.GetComponent<RectTransform>();
            rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, _sizeOfTableAbilitiesAvailable * 65 + 5);
        }
    }
}
