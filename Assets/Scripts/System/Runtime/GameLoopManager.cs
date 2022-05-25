using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoopManager : MonoBehaviour
{
    [SerializeField] private PlayerHealthManager _playerHp;
    [SerializeField] private GameEndUIManager _uiManager;

    private bool _isOver = false;

    private int _dungeonCompletion = 0; //TODO % completion of the dungeon (Room number/Room total number)

    private AIBossHealthManager _boss;

    // [SerializeField] private float _countdownTimer = 3f;

    private List<Loot> _collectedLoots = new List<Loot>();

    private bool _isBossAlive = true;
    private bool _isPlayerAlive = true;
    
    public void AddLoots(List<Loot> loots)
    {
        foreach (Loot loot in loots)
            _collectedLoots.Add(loot);
    }

    public void PlayerDefeated()
    {
        _uiManager.ShowDefeat();
    }

    public void BossDefeated()
    {
        _uiManager.ShowVictory();
    }

    // private void RaiseCompletion(int percentage)
    // {
    //     _dungeonCompletion += percentage;
    //     //ERROR Management > 100
    // }

    public void Pause(bool state)
    {
        Time.timeScale = state ? 0 : 1;
    }
}