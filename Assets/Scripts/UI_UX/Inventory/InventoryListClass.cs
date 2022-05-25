using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class InventoryListClass
{
    public List<InventoryItemClass> items = new List<InventoryItemClass>();
    public List<InventorySpellClass> spells = new List<InventorySpellClass>();
}
