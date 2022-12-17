using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    [SerializeField] private Animator transition;
    [SerializeField] private Image image;
    private float transitionTime = 0.75f;
    private float timer;
    // public GameObject loadingScreen;
    // public Slider slider;

    private void Start()
    {
        timer = transitionTime;
    }

    private void Update()
    {
        if (timer <= 0f || image == null)
            return;
        timer -= Time.deltaTime;
        if (timer <= 0f)
            image.enabled = false;
    }

    public void LoadLevel(string sceneName)
    {
        StartCoroutine(LoadSceneWithTransition(sceneName));
        // StartCoroutine(LoadAsynchronously(sceneName));
    }

    IEnumerator LoadSceneWithTransition(string sceneName)
    {
        if (image == null || transition == null) {
            SceneManager.LoadSceneAsync(sceneName);
        } else {
            image.enabled = true;
            transition.SetTrigger("End");
            yield return new WaitForSeconds(transitionTime);
            SceneManager.LoadSceneAsync(sceneName);
        }
    }

    // IEnumerator LoadAsynchronously(string sceneName)
    // {
    //     AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

    //     loadingScreen.SetActive(true);

    //     while (!operation.isDone)
    //     {
    //         float progress = Mathf.Clamp01(operation.progress / .9f);
    //         slider.value = progress;
    //         yield return null;
    //     }
    // }
}
