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
        lander.OnLeftForce += Lander_OnLeftForce;
        lander.OnRightForce += Lander_OnRightForce;
        lander.OnMiddleForce += Lander_OnMiddleForce;
        lander.OnBeforeForce += Lander_OnBeforeForce;
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
}
