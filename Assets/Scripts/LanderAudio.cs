using UnityEngine;

public class LanderAudio : MonoBehaviour
{
   [SerializeField] private AudioSource ThrusterAudioSource;

    private Lander lander;

    private void Awake()
    {
        lander = GetComponent<Lander>();
    }
   private void Start()
   {
       ThrusterAudioSource.Pause();
       ThrusterAudioSource.volume = SoundManager.Instance.getSoundVolumeNormalized();
        lander.OnLeftForce += Lander_OnLeftForce;
        lander.OnRightForce += Lander_OnRightForce;
        lander.OnMiddleForce += Lander_OnMiddleForce;
        lander.OnBeforeForce += Lander_OnBeforeForce;

        SoundManager.Instance.OnSoundVolumeChanged += SoundManager_OnSoundVolumeChanged;
   }

    private void OnDestroy()
    {
        if (lander != null)
        {
            lander.OnLeftForce -= Lander_OnLeftForce;
            lander.OnRightForce -= Lander_OnRightForce;
            lander.OnMiddleForce -= Lander_OnMiddleForce;
            lander.OnBeforeForce -= Lander_OnBeforeForce;
        }

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.OnSoundVolumeChanged -= SoundManager_OnSoundVolumeChanged;
        }
    }

   private void Lander_OnLeftForce(object sender, System.EventArgs e)
   {
       if (!ThrusterAudioSource.isPlaying)
           ThrusterAudioSource.Play();
   }

    private void Lander_OnRightForce(object sender, System.EventArgs e)
    {
         if (!ThrusterAudioSource.isPlaying)
             ThrusterAudioSource.Play();
    }

    private void Lander_OnMiddleForce(object sender, System.EventArgs e)
    {
        if (!ThrusterAudioSource.isPlaying)
            ThrusterAudioSource.Play();
    }

    private void Lander_OnBeforeForce(object sender, System.EventArgs e)
    {
        ThrusterAudioSource.Pause();
    }

    private void SoundManager_OnSoundVolumeChanged(object sender, System.EventArgs e)
    {
        ThrusterAudioSource.volume = SoundManager.Instance.getSoundVolumeNormalized();
    }
}
