using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flames : Trap
{
    protected override void InflicteDamage(EntityData player)
    {
        player.entityStateManager.ActiveHarmfullEffects(States.FIRE);
    }

    protected override void TriggerEnterNotPlayer(Collider2D collider)
    {
        if (collider.gameObject.name == "Arrow")
        {
            collider.gameObject.GetComponent<Arrow>().SetOnFire();
        }
    }
}