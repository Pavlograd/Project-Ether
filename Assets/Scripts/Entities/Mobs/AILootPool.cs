using System;
using System.Collections.Generic;
using UnityEngine;

public struct Loot
{
    public LootManager lootObject;
    public Vector3 off;
}

public class AILootPool : MonoBehaviour
{
    private System.Random random = new System.Random();
    public List<LootObjectData> loots = new List<LootObjectData>();
    private List<Loot> _confirmedLoots = new List<Loot>();

    private GameLoopManager _glm;

    void Start()
    {
        foreach (LootObjectData loot in loots) {
            float finalAmount = loot.minAmount + ComputeAdditionalAmount(loot.minAmount, loot.maxAmount, loot.dropRate);
            for (int i = 0; i < finalAmount; i++) {
                GameObject obj = Instantiate(loot.prefab);
                LootManager lootManager = obj.GetComponent<LootManager>();
                GameObject parentPool = GameObject.Find("PooledObjects");
                if (parentPool) {
                    obj.transform.parent = parentPool.transform;
                }
                lootManager.gameObject.SetActive(false);
               _confirmedLoots.Add(new Loot {
                    lootObject = lootManager,
                    off = new Vector3(UnityEngine.Random.Range(-.6f, .6f), UnityEngine.Random.Range(-.25f, .25f), 0)
                });
            }
        }
        _glm = GameObject.Find("GameManager").GetComponent<GameLoopManager>();
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

    public void ActiveLoot()
    {
        Transform ground = gameObject.transform.Find("Ground");
        if (ground) {
            foreach (Loot loot in _confirmedLoots) {
                loot.lootObject.gameObject.transform.position = ground.position;
                loot.lootObject.ActiveObject(ground.position + loot.off);
            }
        }
        _glm.AddLoots(_confirmedLoots);
    }
}
