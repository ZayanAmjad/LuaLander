using UnityEngine;
using System;

public class MusicManager : MonoBehaviour
{
    private AudioSource musicAudioSource;
    private static float musicTime;

    private static int musicVolume = 6;
    private const int MAXMUSIC = 10;

    public event EventHandler OnMusicVolumeChanged;

    public static MusicManager Instance { get; private set; }

    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        musicAudioSource.volume = GetMusicVolumeNormalized();
    }

    private void Awake()
    {
        musicAudioSource = GetComponent<AudioSource>();
        musicAudioSource.loop = true;
        musicAudioSource.time = musicTime;
        if (!musicAudioSource.isPlaying)
            musicAudioSource.Play();
        DontDestroyOnLoad(gameObject);
    }

    private void OnDestroy()
    {
        if (musicAudioSource != null)
            musicTime = musicAudioSource.time;
    }

    private float GetMusicVolumeNormalized()
    {
        return (float)musicVolume / MAXMUSIC;
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public void ChangeMusicVolume()
    {
        musicVolume = (musicVolume + 1) % (MAXMUSIC);
        musicAudioSource.volume = GetMusicVolumeNormalized();
        OnMusicVolumeChanged?.Invoke(this, EventArgs.Empty);
    }
}
