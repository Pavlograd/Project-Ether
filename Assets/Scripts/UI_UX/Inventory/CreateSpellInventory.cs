using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

public class CreateSpellInventory : MonoBehaviour
{
    [SerializeField] private List<Ability> _abilitiesAvailableData;// TEMPORAIRE : en attente de la BDD pour stocker les abilities que le joueur peut équiper
    
    private API_skills InventoryList;
    
    private List<Ability> gearedAbilities;

    public GameObject father;

    // Start is called before the first frame update
    void Start()
    {
        PlayerSpellInventory.instance.clearInventory();
        setInventory();
        getAbilities();
    }

    public void setInventory()
    {
        InventoryList = API.GetSkills();
        if (InventoryList != null)
        {            
            gearedAbilities = LevelData.instance.playerAbilities;

            foreach (API_skill item in InventoryList.skills)
            {
                for (int j = 0; j < _abilitiesAvailableData.Count; j++)
                {
                    if (_abilitiesAvailableData[j].parentId == (Int32.Parse(item._parentId)))
                    {
                        Ability tmp = Instantiate(_abilitiesAvailableData[j]);
                        tmp.lvl = (Int32.Parse(item.level));
                        tmp.id = (Int32.Parse(item._id));
                        tmp.geared = (Int32.Parse(item.equipped));
                        PlayerSpellInventory.instance.addAbility(tmp);
                        if (tmp.geared > 0)
                        {
                            gearedAbilities[tmp.geared - 1] = tmp;
                        }
                        break;
                    }
                }

            };
            LevelData.instance.playerAbilities = gearedAbilities;
        }
        else
        {
            Debug.Log("No Json");
        }


    }

    public List<Ability> getAbilities()
    {
        Debug.Log(PlayerSpellInventory.instance.getAbilities().Count);
        return (PlayerSpellInventory.instance.getAbilities());
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

