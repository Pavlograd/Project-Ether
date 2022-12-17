using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootPrevisuManager : MonoBehaviour
{
    [SerializeField] private Transform lootPrevisu;
    private AILootPool[] allLoots;
    private List<int> lootsId = new List<int>();
    private void Start()
    {
        allLoots = FindObjectsOfType(typeof(AILootPool)) as AILootPool[];
        lootsId.Add(allLoots[0].loots[0].itemId);
        foreach (AILootPool loot in allLoots)
            foreach (LootObjectData lootData in loot.loots)
                if (!lootsId.Contains(lootData.itemId)) {
                    GameObject newLoot = Instantiate(lootPrevisu.gameObject, lootPrevisu.parent);
                    newLoot.transform.GetChild(0).GetComponent<Image>().sprite = lootData.picture;
                    lootsId.Add(lootData.itemId);
                }
        lootPrevisu.GetChild(0).GetComponent<Image>().sprite = allLoots[0].loots[0].picture;
    }
}
