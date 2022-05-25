using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pikes : Trap
{
    protected override void InflicteDamage(EntityData player)
    {
        player.entityHealthManager.TakeDamage(_damage);
    }

    protected override void TriggerEnterNotPlayer(Collider2D collider)
    {
    }
}
