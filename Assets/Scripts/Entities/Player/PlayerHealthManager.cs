using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : EntityHealthManager
{
    protected override void Die()
    {
        GameObject obj = GameObject.Find("GameManager");

        if (obj != null) {
            GameLoopManager gameLoopManager = obj.GetComponent<GameLoopManager>();
            if (gameLoopManager != null) {
                gameLoopManager.PlayerDefeated();
            }
        }
        // TO REWORK
        base.Die();
        Destroy(this);
    }
}