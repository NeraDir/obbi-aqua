using System;
using UnityEngine;
using YG;

public class Settings : MonoBehaviour
{
    public static Action<AudioClip> playSound;

    private AudioSource _musicSource;
    private AudioSource _soundSource;

    public static Settings instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            AudioSource[] sources = GetComponentsInChildren<AudioSource>();
            _musicSource = sources[0];
            _soundSource = sources[1];
            playSound += OnPlaySound;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void MuteMusic()
    {
        YG2.saves.MusicMute = !YG2.saves.MusicMute;
        if (_musicSource != null) 
            _musicSource.mute = YG2.saves.MusicMute;
        YG2.SaveProgress();
    }

    public bool GetMusicState()
    {
        if (_musicSource != null)
            return _musicSource.mute;
        return false;
    }

    private void OnDestroy()
    {
        playSound -= OnPlaySound;
    }

    private void OnPlaySound(AudioClip clip)
    {
        _soundSource.PlayOneShot(clip);
    }

    private void LateUpdate()
    {
        _musicSource.volume = YG2.saves.MusicVolume;
        _soundSource.volume = YG2.saves.SoundVolume;
    }
}
