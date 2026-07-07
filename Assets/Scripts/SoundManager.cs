using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip coinPickupSound;
    [SerializeField] private AudioClip fuelPickupSound;
    [SerializeField] private AudioClip landingSuccessSound;
    [SerializeField] private AudioClip landingFailSound;


    public event EventHandler OnSoundVolumeChanged;

    public static SoundManager Instance { get; private set; }

    private static int SoundVolume = 3;
    private const int MAXSOUND = 10;

    private Lander boundLander;

    private void Awake()
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

    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
        UnbindLander();
    }

    private void Start()
    {
        BindLander();

    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        BindLander();
    }

    private void BindLander()
    {
        UnbindLander();

        boundLander = Lander.Instance;
        if (boundLander == null)
        {
            return;
        }

        boundLander.OnFuelPickup += Lander_OnFuelPickup;
        boundLander.OnCoinPickup += Lander_OnCoinPickup;
        boundLander.OnLanded += Lander_OnLanded;
    }

    private void UnbindLander()
    {
        if (boundLander == null)
        {
            return;
        }

        boundLander.OnFuelPickup -= Lander_OnFuelPickup;
        boundLander.OnCoinPickup -= Lander_OnCoinPickup;
        boundLander.OnLanded -= Lander_OnLanded;
        boundLander = null;
    }

    private void Lander_OnFuelPickup(object sender, System.EventArgs e)
    {
        AudioSource.PlayClipAtPoint(fuelPickupSound, Camera.main.transform.position,getSoundVolumeNormalized());
    }

    private void Lander_OnCoinPickup(object sender, System.EventArgs e)
    {
        AudioSource.PlayClipAtPoint(coinPickupSound, Camera.main.transform.position,getSoundVolumeNormalized());
    }

    private void Lander_OnLanded(object sender, Lander.LanderEventArgs e)
    {
        if (e.landingType == Lander.landingType.Success)
        {
            AudioSource.PlayClipAtPoint(landingSuccessSound, Camera.main.transform.position,getSoundVolumeNormalized());
        }
        else
        {
            AudioSource.PlayClipAtPoint(landingFailSound, Camera.main.transform.position,getSoundVolumeNormalized());
        }
    }

    public void ChangeSound()
    {
        SoundVolume = (SoundVolume + 1) % (MAXSOUND);
        OnSoundVolumeChanged?.Invoke(this, EventArgs.Empty);
    }

    public int getSoundVolume()
    {
        return SoundVolume;
    }

    public float getSoundVolumeNormalized()
    {
        return (float)SoundVolume / MAXSOUND;
    }
}
