using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
        int i = 0;

        foreach (API_inventory item in InventoryList.inventories)
        {
            if ((Int32.Parse(item.quantity)) > 0)
            {

                Vector3 pos = new Vector3(355f, 120f, 0.0f);
                pos.y = pos.y - (60 * i);
                i = i + 1;

                GameObject newInventoryItem = Instantiate(prefab, pos, transform.rotation);

                newInventoryItem.transform.SetParent(GameObject.FindGameObjectWithTag("InventoryItemList").transform, false);
                newInventoryItem.SetActive(true);

                Image itemImage = newInventoryItem.transform.Find("Picture").GetComponent<Image>();
                itemImage.sprite = Resources.Load<Sprite>("Textures/Shop/Item/" + System.IO.Path.GetFileNameWithoutExtension(item._id));

                Text itemName = newInventoryItem.transform.Find("Name").GetComponent<Text>();
                itemName.text = item.name;

                Text itemQuantity = newInventoryItem.transform.Find("Quantity").GetComponent<Text>();
                itemQuantity.text = item.quantity;
            }
        };
    }
}
