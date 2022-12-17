using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] ToggleController _sfxToggle;
    [SerializeField] ToggleController _musicToggle;

    private void Start()
    {
        bool sfxSettings = Convert.ToBoolean(PlayerPrefs.GetInt("sfxSettings", 1));
        bool musicSettings = Convert.ToBoolean(PlayerPrefs.GetInt("musicSettings", 1));
        _sfxToggle.InitToogle(sfxSettings);
        _musicToggle.InitToogle(musicSettings);
    }

    public void HandleSFXToggleChange(bool value)
    {
        PlayerPrefs.SetInt("sfxSettings", Convert.ToInt32(value));
        AudioManager.instance?.MuteSfx(!value);
    }

    public void HandleMusicToggleChange(bool value)
    {
        PlayerPrefs.SetInt("musicSettings", Convert.ToInt32(value));
        AudioManager.instance?.MuteMusic(!value);
    }
}
