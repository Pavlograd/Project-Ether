using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum LevelState {
    NONE,
    PREVISUALISATION,
    INGAME,
}

public class GameManager : MonoBehaviour
{
    #pragma warning disable 0414
    [SerializeField] private GameObject _player;
    [SerializeField] private LevelState _levelState = LevelState.PREVISUALISATION;

    private void Start()
    {
        SetLevelState(_levelState);
    }

    public AsyncOperation LoadAdditiveScene(int sceneIndex)
    {
        return SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
    }

    public AsyncOperation LoadAdditiveScene(string sceneIndex)
    {
        return SceneManager.LoadSceneAsync(sceneIndex, LoadSceneMode.Additive);
    }

    public void UnLoadAdditiveScene(int sceneIndex)
    {
        SceneManager.UnloadSceneAsync(sceneIndex);
    }

    public void UnLoadAdditiveScene(string sceneIndex)
    {
        SceneManager.UnloadSceneAsync(sceneIndex);
    }

    private void SetupPlayerAbilitiesSet()
    {
        if (LevelData.instance == null) return;
        PlayerAbilityManager playerAbilityManager = _player.GetComponent<PlayerAbilityManager>();
        if (playerAbilityManager == null) return;
        if (LevelData.instance.playerDefaultAttack != null) {
            playerAbilityManager._abilitiesHolder.SetNewAbilities(LevelData.instance.playerDefaultAttack);
        }
        if (LevelData.instance.playerAbilities != null) {
            playerAbilityManager._abilitiesHolder.SetNewAbilities(LevelData.instance.playerAbilities);
        }
    }

    private void LinkPlayerToUI()
    {
        GameObject playerUi = GameObject.Find("PlayerUi");
        if (playerUi != null) {
            PlayerUi playerUiScript = playerUi.GetComponent<PlayerUi>();
            if (playerUiScript != null) {
                playerUiScript.LinkToPlayer(_player);
            }
        }
    }

    private IEnumerator SetupIngameEnvironment()
    {
        _levelState = LevelState.INGAME;
        if (SceneManager.GetSceneByName("DungeonPrevisuUi").isLoaded) {
            UnLoadAdditiveScene("DungeonPrevisuUi");
        }
        AsyncOperation scene = LoadAdditiveScene("GameUi");
        yield return new WaitUntil(() => scene.isDone);
        SetupPlayerAbilitiesSet();
        LinkPlayerToUI();
    }

    private void SetupPrevisualisationEnvironment()
    {
        _levelState = LevelState.PREVISUALISATION;
        AudioManager.instance?.PlayMusic(Music.IN_GAME);
        LoadAdditiveScene("DungeonPrevisuUi");
        if (SceneManager.GetSceneByName("GameUi").isLoaded) {
            UnLoadAdditiveScene("GameUi");
        }
    }

    public void SetLevelState(LevelState state)
    {
        switch (state) {
            case LevelState.PREVISUALISATION: SetupPrevisualisationEnvironment();
                break;
            case LevelState.INGAME: StartCoroutine(SetupIngameEnvironment());
                break;
            default:
                break;
        }
    }
}