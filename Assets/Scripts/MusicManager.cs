using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private AudioSource musicAudioSource;
    private static float musicTime;

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
}
