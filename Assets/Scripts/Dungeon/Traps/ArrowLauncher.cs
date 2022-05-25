using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowLauncher : Trap
{
    [SerializeField] GameObject _arrow;

    void Start()
    {
        InvokeRepeating("LaunchArrow", 2.0f, 1.5f);
    }

    void LaunchArrow()
    {
        //Debug.Log("launch arrow");
        GameObject arrow = Instantiate(_arrow, transform.position, Quaternion.Euler(0.0f, 0.0f, 0.0f));

        arrow.name = "Arrow";
    }

    protected override void TriggerEnterNotPlayer(Collider2D colider)
    {
    }

    protected override void InflicteDamage(EntityData player)
    {
    }
}