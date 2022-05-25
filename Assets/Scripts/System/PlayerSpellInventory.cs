using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class PlayerSpellInventory : Singleton<PlayerSpellInventory>
{
    private List<Ability> abilities = new List<Ability>();

    public void clearInventory()
    {
        this.abilities = new List<Ability>();
    }

    public void addAbility(Ability _abilitiesAvailableData)
    {
        this.abilities.Add(_abilitiesAvailableData);
    }

    public List<Ability> getAbilities()
    {
        return (this.abilities);
    }

    public void lvlUp(int id)
    {
        foreach (Ability item in abilities)
        {
            if (item.id == id)
            {
                item.lvl = item.lvl + 1;
                API.PostSkill(item);
            }
        }
    }
}
