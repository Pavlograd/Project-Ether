using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonPrevisualisationManager : MonoBehaviour
{
    public void Attack() {
        GameObject obj = GameObject.Find("GameManager");

        if (obj != null) {
            GameManager gameManager = obj.GetComponent<GameManager>();
            if (gameManager != null) {
                gameManager.SetLevelState(LevelState.INGAME);
            }
        }
    }

    public void PauseGame()
    {
        GameObject obj = GameObject.Find("GameManager");
        GameLoopManager gameLoopManager = obj.GetComponent<GameLoopManager>();
        if (gameLoopManager != null)
            gameLoopManager.Pause(true);
    }
}
