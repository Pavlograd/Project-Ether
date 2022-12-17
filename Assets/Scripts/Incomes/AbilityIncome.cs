using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityIncome : MonoBehaviour
{
    [SerializeField] GameObject ability;
    [SerializeField] Transform firepoint;

    public void ActivateAbility()
    {
        GameObject newObject = Instantiate(ability, firepoint.position, Quaternion.identity);

        newObject.AddComponent<ProjectileIncome>();
    }
}
