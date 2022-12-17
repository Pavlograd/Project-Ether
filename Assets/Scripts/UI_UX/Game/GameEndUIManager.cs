using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEndUIManager : MonoBehaviour
{
    [SerializeField] private PopupManager _victoryPopup;
    [SerializeField] private PopupManager _defeatPopup;
    [SerializeField] private GameObject _lootIconTemplate;
    [SerializeField] private Transform _lootGrid;

    public void ShowVictory(Dictionary<string, LootInfo> collectedLoot, List<Ability> lootedAbilities)
    {
        DisplayLoot(collectedLoot, lootedAbilities);
        SceneManager.UnloadSceneAsync("GameUi");
        _victoryPopup.Open();
        Time.timeScale = 0;
        MainMenu.doCoinsAnimation = true;
    }

    private void DisplayLoot(Dictionary<string, LootInfo> collectedLoot, List<Ability> lootedAbilities)
    {
        foreach (KeyValuePair<string, LootInfo> key in collectedLoot) {
            GameObject obj = Instantiate(_lootIconTemplate, _lootGrid);
            LootIconManager mgr = obj.GetComponent<LootIconManager>();
            mgr.SetIcon(key.Value.objData.picture, $"x{key.Value.quantity.ToString()}", key.Value.objData.objectName);
        }
        if (lootedAbilities != null) {
            for (int i = 0; i < lootedAbilities.Count; i++) {
                GameObject obj = Instantiate(_lootIconTemplate, _lootGrid);
                LootIconManager mgr = obj.GetComponent<LootIconManager>();
                mgr.SetIcon(lootedAbilities[i].thumbnail, "x1", lootedAbilities[i].name);
            }
        }
    }

    public void ShowDefeat()
    {
        _defeatPopup.Open();
        Time.timeScale = 0;
    }

    public void Retry()
    {
        Time.timeScale = 1f;
        FindObjectOfType<LevelLoader>().LoadLevel(SceneManager.GetActiveScene().name);
    }

    public void GoBackToMainMenu()
    {
        Time.timeScale = 1f;
        FindObjectOfType<LevelLoader>().LoadLevel("Main Menu");
    }
}
