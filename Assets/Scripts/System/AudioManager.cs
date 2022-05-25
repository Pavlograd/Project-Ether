using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SFX {
    DEFAULT,
    FIRE_PROJECTILE,
    ROCK,
    BUFF,
    COINS
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

    public void PlaySoundEffect(SFX SFX_ID)
    {
        if (_sfxRepository.ContainsKey(SFX_ID)) {
            _sfxAudioSource.clip = _sfxRepository[SFX_ID];
            _sfxAudioSource.Play();
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
        _musicAudioSource.mute = mute;
    }

    public void MuteSfx(bool mute)
    {
        _sfxAudioSource.mute = mute;
    }

    public void SetMusicVolume(float value)
    {
        _mixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
    }

    public void SetSfxVolume(float value)
    {
        _mixer.SetFloat("SfxVolume", Mathf.Log10(value) * 20);
    }

    public void SetMasterVolume(float value)
    {
        _mixer.SetFloat("MasterVolume", Mathf.Log10(value) * 20);
    }
}
