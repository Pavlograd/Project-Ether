using System;
using System.Collections.Generic;
using UnityEngine;

public class LootInfo
{
    public float quantity;
    public LootObjectData objData;
}

public struct LootObjects
{
    public LootManager lootManager;
    public Vector3 off;
}

public class AILootPool : MonoBehaviour
{
    private System.Random random = new System.Random();
    [SerializeField] private Sprite _abilityDustPicture; // Temporary just to make the thing work you know
    public List<LootObjectData> loots = new List<LootObjectData>();
    // this list contains only unique object data with quantity
    private List<LootInfo> _lootSummary = new List<LootInfo>();
    private List<Ability> _lootedAbility = new List<Ability>();
    // this list contains all the gameObjects instantiate to make the animations
    private List<LootObjects> _allLootObjects = new List<LootObjects>();
    private GameLoopManager _glm;
    private API_inventories _inventory = null;

    private void Awake() {
        _inventory = API.GetInventory();
    }

    void Start()
    {
        foreach (LootObjectData loot in loots) {
            float finalAmount = loot.minAmount + ComputeAdditionalAmount(loot.minAmount, loot.maxAmount, loot.dropRate);
            InstantiateLootGameObject(loot);
            if (loot.objectName == "Coin") {
                for (int i = 0; i < 4; i++) {
                    InstantiateLootGameObject(loot);
                }
            }
            _lootSummary.Add(new LootInfo {
                objData = loot,
                quantity = finalAmount,
            });
        }
        _glm = GameObject.Find("GameManager")?.GetComponent<GameLoopManager>();
    }

    private void InstantiateLootGameObject(LootObjectData loot)
    {
        GameObject obj = Instantiate(loot.prefab);
        LootManager lootManager = obj.GetComponent<LootManager>();
        GameObject parentPool = GameObject.Find("PooledObjects");
        if (parentPool) {
            obj.transform.parent = parentPool.transform;
        }
        lootManager.gameObject.SetActive(false);
        _allLootObjects.Add(new LootObjects {
            lootManager = lootManager,
            off = new Vector3(UnityEngine.Random.Range(-.8f, .8f), UnityEngine.Random.Range(-.5f, .5f), 0)
        });
    }

    private float ComputeAdditionalAmount(float minValue, float maxValue, float rate)
    {
        float amount = 0;
        for (int i = 0; i < maxValue - minValue; i++) {
            if (random.NextDouble() < rate)
                amount++;
        }
        return amount;
    }

    public void SetAbilityPool(List<Ability> abilityList)
    {
        foreach (Ability ability in abilityList) {
            float drop = ComputeAdditionalAmount(0, 1f, 1f);
            if (drop >= 0) {
                // check if ability is already own so we directly set the abilityDust as loot else, fill the lootable abilities
                Ability item = PlayerSpellInventory.instance.getAbilities().Find(e => e.parentId == ability.parentId);
                if (item != null) {
                    ItemData data;
                    if (ItemsDictionary.TryGetItem(Items.AbiliyDust, out data)) {
                        API_inventory abilityDustOwned = _inventory.inventories.Find(e => e.name == data.name);
                        int lootIdx = _lootSummary.FindIndex(e => e.objData.objectName.ToString() == data.name);
                        if (lootIdx == -1) {
                            LootObjectData abilityDust = (LootObjectData)ScriptableObject.CreateInstance<LootObjectData>();
                            abilityDust.itemId = data.id;
                            abilityDust.objectName = data.name;
                            abilityDust.picture = _abilityDustPicture;
                            _lootSummary.Add(new LootInfo() {
                                objData = abilityDust,
                                quantity = 30
                            });
                        } else {
                            _lootSummary[lootIdx].quantity += 30;
                        }
                    }
                } else {
                    _lootedAbility.Add(ability);
                }
            }
        }
    }

    public void ActiveLoot()
    {
        Transform ground = gameObject.transform.Find("Ground");
        if (ground) {
            _glm?.AddLoots(_lootSummary, _lootedAbility);
            foreach (LootObjects loot in _allLootObjects) {
                loot.lootManager.gameObject.transform.position = ground.position;
                loot.lootManager.ActiveObject(ground.position + loot.off);
            }
        }
    }
}
