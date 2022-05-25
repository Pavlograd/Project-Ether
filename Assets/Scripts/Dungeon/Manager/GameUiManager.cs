using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUiManager : MonoBehaviour
{
    public void PauseGame(bool state)
    {
        GameObject obj = GameObject.Find("GameManager");
        GameLoopManager gameLoopManager = obj.GetComponent<GameLoopManager>();
        if (gameLoopManager != null)
            gameLoopManager.Pause(state);
    }

    public void GoBackToPrevisu()
    {
        GameObject obj = GameObject.Find("GameManager");

        if (obj != null)
        {
            GameManager gameManager = obj.GetComponent<GameManager>();
            if (gameManager != null)
                gameManager.SetLevelState(LevelState.PREVISUALISATION);
        }
    }
    
    public void GoBackToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }
}