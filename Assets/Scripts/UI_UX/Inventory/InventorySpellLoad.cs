using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InventorySpellLoad : MonoBehaviour
{
    public GameObject prefab = null;

    public RectTransform rectTransform;

    public Text lvlText;

    public GameObject father;

    // Start is called before the first frame update
    void Start()
    {
        setUpItem();
    }

    public void setUpItem()
    {
        int i = 0;
        List<Ability> _abilitiesAvailableData = PlayerSpellInventory.instance.getAbilities();

        if (_abilitiesAvailableData.Count > 0)
        {
            RectTransform NewRectTransform = rectTransform.GetComponent<RectTransform>();
            NewRectTransform.sizeDelta = new Vector2(NewRectTransform.sizeDelta.x, 300 + 30 * _abilitiesAvailableData.Count);

            foreach (Ability item in _abilitiesAvailableData)
            {
                if (item != null)
                {
                    Vector3 pos = new Vector3(355f, 130f, 0.0f);
                    pos.y = pos.y - (60 * i);
                    i = i + 1;

                    GameObject newInventoryItem = Instantiate(prefab, pos, transform.rotation);
                    newInventoryItem.transform.SetParent(GameObject.FindGameObjectWithTag("InventorySpellList").transform, false);
                    newInventoryItem.SetActive(true);

                    Image itemImage = newInventoryItem.transform.Find("Image").GetComponent<Image>();
                    itemImage.sprite = item.thumbnail;

                    Text itemName = newInventoryItem.transform.Find("Name").GetComponent<Text>();
                    itemName.text = item.name;

                    Text itemLvl = newInventoryItem.transform.Find("LVL").GetComponent<Text>();
                    itemLvl.text = item.lvl.ToString();

                    Text itemid = newInventoryItem.transform.Find("ID").GetComponent<Text>();
                    itemid.text = item.id.ToString();
                }
            };
        }
    }

    public void lvlup(Text _ID)
    {
        PlayerSpellInventory.instance.lvlUp(Int32.Parse(_ID.text));

        refresh();
        setUpItem();
    }

    public void refresh()
    {
        int i = 0;
        foreach (Transform child in father.transform)
        {
            if (i > 0)
            {
                GameObject.Destroy(child.gameObject);
            }
            i = i + 1;
        }
    }
}
