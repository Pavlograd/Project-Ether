using UnityEngine;

public class AIBossHealthManager : AIHealthManager
{
    private void OnDestroy()
    {
        GameObject obj = GameObject.Find("GameManager");

        if (obj != null) {
            GameLoopManager gameLoopManager = obj.GetComponent<GameLoopManager>();
            if (gameLoopManager != null) {
                gameLoopManager.BossDefeated();
            }
        }
    }
}
