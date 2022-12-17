using UnityEngine;
using System.Collections.Generic;
using System;

public class addLoot : MonoBehaviour
{
    // !!!!!!!
    // I AM NOT SURE THIS SCRIPT IS USED ANYMORE
    // !!!!!!!
    [SerializeField] private List<Ability> _abilitiesAvailableData;// TEMPORAIRE : en attente de la BDD pour stocker les abilities que le joueur peut équiper
    private API_inventories InventoryList;

    void Start()
    {
        // addLootToInv();
    }

    void addLootToInv() {
        int newSpellId = PlayerSpellInventory.instance.getAbilities().Count + 1;
        int newItemId = UnityEngine.Random.Range(0, 4);
        int newParentId = UnityEngine.Random.Range(1, _abilitiesAvailableData.Count - 1);

        Ability tmp = Instantiate(_abilitiesAvailableData[newParentId]);
        tmp.lvl = 1;
        tmp.id = newSpellId;
        tmp.geared = 0;
        API.PostSkill(tmp);

        InventoryList = API.GetInventory();

        foreach (API_inventory item in InventoryList.inventories)
        {
            if ((Int32.Parse(item._id)) == newItemId)
            {
                API.PostInventory(Int32.Parse(item._id), item.name, Int32.Parse(item.quantity) + 1);
            }
        };
    }
}
