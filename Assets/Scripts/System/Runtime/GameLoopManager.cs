using System;
using System.Collections.Generic;
using UnityEngine;

public class GameLoopManager : MonoBehaviour
{
    [SerializeField] private PlayerHealthManager _playerHp;
    [SerializeField] private GameEndUIManager _uiManager;
    private API_inventories _inventory = null;
    private API_skills _abilitiesInventory = null;

    // Uncomment if necessary
    // Commented to prevent warnings

    //private bool _isOver = false;

    //private int _dungeonCompletion = 0; //TODO % completion of the dungeon (Room number/Room total number)

    //private AIBossHealthManager _boss;

    // [SerializeField] private float _countdownTimer = 3f;

    private Dictionary<string, LootInfo> _collectedLoots = new Dictionary<string, LootInfo>();
    private List<Ability> _lootedAbilities = null;

    private void Start()
    {
        _inventory = API.GetInventory();
        _abilitiesInventory = API.GetSkills();
    }

    public void AddLoots(List<LootInfo> loots, List<Ability> lootedAbilities)
    {
        _lootedAbilities = new List<Ability>(lootedAbilities);
        foreach (LootInfo loot in loots) {
            LootInfo existingValue;
            if (_collectedLoots.TryGetValue(loot.objData.objectName, out existingValue)) {
                existingValue.quantity += loot.quantity;
                _collectedLoots[loot.objData.objectName] = existingValue;
            } else {
                _collectedLoots.Add(loot.objData.objectName, loot);
            }
        }
    }

    public void PlayerDefeated()
    {
        _uiManager.ShowDefeat();
    }

    public void BossDefeated()
    {
        if (_lootedAbilities != null) {
            int userAbilityCount = _abilitiesInventory.skills.Count + 1;
            foreach (Ability item in _lootedAbilities) {
                Ability tmp = Instantiate(item);
                tmp.lvl = 1;
                tmp.id = userAbilityCount;
                tmp.geared = 0;
                API.PostSkill(tmp);
                userAbilityCount++;
            }
        }
        _uiManager.ShowVictory(_collectedLoots, _lootedAbilities);
        foreach (KeyValuePair<string, LootInfo> key in _collectedLoots) {
            if (key.Value.objData.objectName != "Coin") {
                API_inventory item = _inventory.inventories.Find(e => e._id == key.Value.objData.itemId.ToString());
                if (item == null) {
                    API.PostInventory(key.Value.objData.itemId, key.Value.objData.objectName, (int)key.Value.quantity);
                } else {
                    API.PostInventory(key.Value.objData.itemId, key.Value.objData.objectName, Int32.Parse(item.quantity) + (int)key.Value.quantity);
                }
            } else {
                API.AddCash((int)key.Value.quantity);
            }
        }
    }

    // private void RaiseCompletion(int percentage)
    // {
    //     _dungeonCompletion += percentage;
    //     //ERROR Management > 100
    // }

    public void Pause(bool state)
    {
        Time.timeScale = state ? 0 : 1;
    }
}