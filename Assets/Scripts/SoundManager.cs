using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip coinPickupSound;
    [SerializeField] private AudioClip fuelPickupSound;
    [SerializeField] private AudioClip landingSuccessSound;
    [SerializeField] private AudioClip landingFailSound;
    private void Start()
    {
        Lander.Instance.OnFuelPickup += Lander_OnFuelPickup;
        Lander.Instance.OnCoinPickup += Lander_OnCoinPickup;
        Lander.Instance.OnLanded += Lander_OnLanded;
    }

    private void Lander_OnFuelPickup(object sender, System.EventArgs e)
    {
        AudioSource.PlayClipAtPoint(fuelPickupSound, Camera.main.transform.position);
    }

    private void Lander_OnCoinPickup(object sender, System.EventArgs e)
    {
        AudioSource.PlayClipAtPoint(coinPickupSound, Camera.main.transform.position);
    }

    private void Lander_OnLanded(object sender, Lander.LanderEventArgs e)
    {
        if (e.landingType == Lander.landingType.Success)
        {
            AudioSource.PlayClipAtPoint(landingSuccessSound, Camera.main.transform.position);
        }
        else
        {
            AudioSource.PlayClipAtPoint(landingFailSound, Camera.main.transform.position);
        }
    }
}
