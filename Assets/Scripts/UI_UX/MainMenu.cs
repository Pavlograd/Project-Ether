using System;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public static bool doCoinsAnimation = false;
    [SerializeField] private CoinsAnimation coinsAnimationScript;

    void Start()
    {
        // Audio
        AudioManager.instance?.PlayMusic(Music.MAIN_MENU);
        bool sfxSettings = Convert.ToBoolean(PlayerPrefs.GetInt("sfxSettings", 1));
        bool musicSettings = Convert.ToBoolean(PlayerPrefs.GetInt("musicSettings", 1));
        AudioManager.instance?.MuteSfx(!sfxSettings);
        AudioManager.instance?.MuteMusic(!musicSettings);

        // Gold/items display management
        MainItemsRecap script = GameObject.Find("ItemsRecap")?.GetComponent<MainItemsRecap>();
        script?.updateTexts();
        if (doCoinsAnimation) {
            doCoinsAnimation = false;
            StartCoroutine(coinsAnimationScript.Animate());
            StartCoroutine(script?.updateGoldWithAnimation(coinsAnimationScript.timeToTravel, coinsAnimationScript.timeToTravel));
        } else {
            script?.updateGoldText();
        }
    }
}
