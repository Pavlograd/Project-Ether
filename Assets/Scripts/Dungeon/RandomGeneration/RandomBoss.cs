using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBoss : MonoBehaviour
{
    [SerializeField] DonjonGeneratorData data;
    [SerializeField] AbilitiesHolder abilitiesHolder;

    // Start is called before the first frame update
    void Awake() // Set default attacks and four random abilities
    {
        abilitiesHolder.defaultAttack = GetRandomAbility();
        abilitiesHolder.abilities.Add(GetRandomAbility());
        abilitiesHolder.abilities.Add(GetRandomAbility());
        abilitiesHolder.abilities.Add(GetRandomAbility());
        abilitiesHolder.abilities.Add(GetRandomAbility());
    }

    void Start()
    {
        Destroy(this);

    }

    Ability GetRandomAbility()
    {
        return data._abilitiesAvailableData[Random.Range(0, data._abilitiesAvailableData.Count)];
    }
}
