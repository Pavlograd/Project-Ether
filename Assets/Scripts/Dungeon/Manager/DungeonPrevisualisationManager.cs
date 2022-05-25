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
    
    public void SetAnAbility()
    {
        /*
        This function or bundle of functions will have the job manage the chosen abilities from the user through Ui.
        The chosen abitilies will be set to the LevelData static class so the game scene will be able to read it and set the player.
        */
    }
}
