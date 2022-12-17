using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SFX {
    DEFAULT,
    FIRE_PROJECTILE,
    ROCK,
    BUFF,
    COINS,
    UI_SWITCH,
    BUTTON_CLICK,
    TELEPORT,
    POWER_UP,
    POWER_UP_SUCCESS,
    POWER_UP_FAILURE,
}

public enum Music {
    MAIN_MENU,
    IN_GAME
}

[Serializable]
struct AudioItem<T> {
    public T key;
    public AudioClip audioClip;
}

public class AudioManager : Singleton<AudioManager>
{
    private Dictionary<SFX, AudioClip> _sfxRepository = new Dictionary<SFX, AudioClip>();
    private Dictionary<Music, AudioClip> _musicRepository = new Dictionary<Music, AudioClip>();

    [SerializeField] private AudioMixer _mixer;
    [SerializeField] private float _defaultMusicVolume = -20f;
    [SerializeField] private float _defaultSfxVolume = -10f;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource _sfxAudioSource;
    [SerializeField] private AudioSource _musicAudioSource;

    [Header("Repositories")]
    [SerializeField] private List<AudioItem<SFX>> _sfxList;
    [SerializeField] private List<AudioItem<Music>> _musiclist;

    protected override void Awake()
    {
        if (instance == null) {
            foreach (AudioItem<SFX> SFXItem in _sfxList) {
                if (SFXItem.audioClip == null)
                    continue;
                _sfxRepository.Add(SFXItem.key, SFXItem.audioClip);
            }
            foreach (AudioItem<Music> musicItem in _musiclist) {
                if (musicItem.audioClip == null)
                    continue;
                _musicRepository.Add(musicItem.key, musicItem.audioClip);
            }
        }
        base.Awake();
    }

    public AudioClip GetSfxAudioClip(SFX sfxId)
    {
        AudioClip clip;
        _sfxRepository.TryGetValue(sfxId, out clip);
        return clip;
    }

    public void PlaySoundEffect(SFX SFX_ID)
    {
        if (_sfxRepository.ContainsKey(SFX_ID)) {
            _sfxAudioSource.clip = _sfxRepository[SFX_ID];
            _sfxAudioSource.PlayOneShot(_sfxAudioSource.clip);
        }
    }

    public void PlayMusic(Music musicID)
    {
        if (_musicRepository.ContainsKey(musicID)) {
            if (!(_musicAudioSource.clip != null
                && _musicAudioSource.clip == _musicRepository[musicID]
                && _musicAudioSource.isPlaying)) {
                _musicAudioSource.clip = _musicRepository[musicID];
                _musicAudioSource.Play();
            }
        }
    }

    public void PauseCurrentMusic()
    {
        if (_musicAudioSource.clip != null && _musicAudioSource.isPlaying) {
            _musicAudioSource.Pause();
        }
    }

    public void ResumeCurrentMusic()
    {
        if (_musicAudioSource.clip != null && !_musicAudioSource.isPlaying) {
            _musicAudioSource.Play();
        }
    }

    public void MuteMusic(bool mute)
    {
        if (mute) {
            SetMusicVolume(-80);
        } else {
            SetMusicVolume(_defaultMusicVolume);
        }
    }

    public void MuteSfx(bool mute)
    {
        if (mute) {
            SetSfxVolume(-80);
        } else {
            SetSfxVolume(_defaultSfxVolume);
        }
    }

    // public void MuteMusic()
    // {
    //     _mixer.GetFloat("MusicVolume", out float currentVolume);
    //     if (currentVolume == -80) {
    //         MuteMusic(false);
    //     } else {
    //         MuteMusic(true);
    //     }
    // }

    // public void MuteSfx()
    // {
    //     _mixer.GetFloat("SfxVolume", out float currentVolume);
    //     if (currentVolume == -80) {
    //         MuteSfx(false);
    //     } else {
    //         MuteSfx(true);
    //     }
    // }

    public void SetMusicVolume(float value)
    {
        _mixer.SetFloat("MusicVolume", value);
    }

    public void SetSfxVolume(float value)
    {
        _mixer.SetFloat("SfxVolume", value);
    }

    public void SetMasterVolume(float value)
    {
        _mixer.SetFloat("MasterVolume", value);
    }
}
