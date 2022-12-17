using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class InventoryItemLoad : MonoBehaviour
{
    public GameObject prefab = null;

    private API_inventories InventoryList;

    // Start is called before the first frame update
    void Start()
    {
        InventoryList = API.GetInventory(); 
        if (prefab != null)
        {
            setUpItem();
        }
    }

    private void setUpItem()
    {
        foreach (API_inventory item in InventoryList.inventories)
        {
            if ((Int32.Parse(item.quantity)) > 0)
            {
                GameObject newInventoryItem = Instantiate(prefab);

                newInventoryItem.transform.SetParent(GameObject.FindGameObjectWithTag("InventoryItemList").transform, false);
                newInventoryItem.SetActive(true);

                Image itemImage = newInventoryItem.transform.Find("Picture").GetComponent<Image>();
                itemImage.sprite = Resources.Load<Sprite>("Textures/Shop/Item/" + System.IO.Path.GetFileNameWithoutExtension(item._id));

                TextMeshProUGUI itemName = newInventoryItem.transform.Find("Name").GetComponent<TextMeshProUGUI>();
                itemName.text = item.name;

                TextMeshProUGUI itemQuantity = newInventoryItem.transform.Find("Quantity").GetComponent<TextMeshProUGUI>();
                itemQuantity.text = item.quantity;
            }
        };
    }
}
