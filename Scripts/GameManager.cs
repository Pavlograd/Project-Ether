using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject healthText;
    public GameObject victoryCanvas;
    public GameObject defeatCanvas;
    public GameObject pauseCanvas;

    [System.NonSerialized] public bool IsPaused = false;
    private static readonly int Travel = Animator.StringToHash("Travel");

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Time.timeScale = 1;
        victoryCanvas.SetActive(false);
        defeatCanvas.SetActive(false);
        pauseCanvas.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            TogglePause();
    }

    public void TogglePause()
    {
        IsPaused = !IsPaused;

        Time.timeScale = (IsPaused ? 0 : 1);
        pauseCanvas.SetActive(IsPaused);
    }

    public void QuitGame()
    {
        Application.Quit(0);
    }

    public void Win(GameObject player)
    {
        victoryCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    public void LoadNextDungeon()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void KillPlayer(GameObject player)
    {
        defeatCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    public void ToggleTravel(GameObject travelObject)
    {
        var animator = travelObject.GetComponent<Animator>();

        if (animator)
            animator.SetBool(Travel, !animator.GetBool(Travel));
    }
}
