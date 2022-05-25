using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndUIManager : MonoBehaviour
{
    [SerializeField] private GameObject endMenu;
    [SerializeField] private GameObject victory;
    [SerializeField] private GameObject defeat;

    public void ShowVictory()
    {
        print("1");
        endMenu.SetActive(true);
        victory.SetActive(true);
        defeat.SetActive(false);
        Time.timeScale = 0;
    }

    public void ShowDefeat()
    {
        print("2");
        endMenu.SetActive(true);
        victory.SetActive(false);
        defeat.SetActive(true);
        Time.timeScale = 0;
    }

    public void GoBackToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }
}
