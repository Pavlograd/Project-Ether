using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using TMPro;

public class InventorySpellLoad : MonoBehaviour
{
    public GameObject prefab = null;
    public RectTransform rectTransform;
    public GameObject father;

    [SerializeField] private SkillsLevelUpPanel _LevelUpPanel;

    // Start is called before the first frame update
    void Start()
    {
        setUpItem();
    }

    public void setUpItem()
    {
        List<Ability> _abilitiesAvailableData = PlayerSpellInventory.instance.getAbilities();

        if (_abilitiesAvailableData.Count > 0) {
            foreach (Ability item in _abilitiesAvailableData) {
                if (item != null && item.name != "BasicProjectile" && item.name != "FireProjectileMob") {
                    GameObject newInventoryItem = Instantiate(prefab);
                    newInventoryItem.transform.SetParent(GameObject.FindGameObjectWithTag("InventorySpellList").transform, false);
                    newInventoryItem.SetActive(true);

                    Image itemImage = newInventoryItem.transform.Find("Image").GetComponent<Image>();
                    itemImage.sprite = item.thumbnail;

                    TMP_Text itemName = newInventoryItem.transform.Find("Name").GetComponent<TMP_Text>();
                    itemName.text = item.name;

                    TMP_Text itemLvl = newInventoryItem.transform.Find("LVL").GetComponent<TMP_Text>();
                    itemLvl.text = item.lvl.ToString();

                    TMP_Text itemid = newInventoryItem.transform.Find("ID").GetComponent<TMP_Text>();
                    itemid.text = item.parentId.ToString();

                }
            };
        }
    }

    public void lvlup(int id, AbilityType type)
    {
        PlayerSpellInventory.instance.lvlUp(id);
        refresh();
        setUpItem();
    }

    public void ShowAbilityPanel(TMP_Text id)
    {
        _LevelUpPanel.ShowAbilityPanel(Int32.Parse(id.text), this);
    }

    public void refresh()
    {
        int i = 0;
        foreach (Transform child in father.transform) {
            if (i > 0) {
                GameObject.Destroy(child.gameObject);
            }
            i = i + 1;
        }
    }
}
