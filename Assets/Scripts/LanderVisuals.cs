using UnityEngine;

public class LanderVisual : MonoBehaviour
{
    [SerializeField] private ParticleSystem LeftThrusterParticle;
    [SerializeField] private ParticleSystem MiddleThrusterParticle;
    [SerializeField] private ParticleSystem RightThrusterParticle;
    [SerializeField] private GameObject ExplosionParticle;
    private Lander lander;
    private void Awake()
    {
        lander = GetComponent<Lander>();
        lander.OnLeftForce += Lander_OnLeftForce;
        lander.OnMiddleForce += Lander_OnMiddleForce;
        lander.OnRightForce += Lander_OnRightForce;
        lander.OnBeforeForce += Lander_OnBeforeForce;


        SetEnableThruster(LeftThrusterParticle, false);
        SetEnableThruster(MiddleThrusterParticle, false);
        SetEnableThruster(RightThrusterParticle, false);
    } 

    private void Lander_OnBeforeForce(object sender, System.EventArgs e)
    {
        SetEnableThruster(LeftThrusterParticle, false);
        SetEnableThruster(MiddleThrusterParticle, false);
        SetEnableThruster(RightThrusterParticle, false);
    }

    private void Lander_OnMiddleForce(object sender, System.EventArgs e)
    {
        SetEnableThruster(MiddleThrusterParticle, true);
        SetEnableThruster(LeftThrusterParticle, true);
        SetEnableThruster(RightThrusterParticle, true);
    }

    private void Lander_OnRightForce(object sender, System.EventArgs e)
    {
        SetEnableThruster(RightThrusterParticle, true);
        SetEnableThruster(LeftThrusterParticle, false);
    }

    private void Lander_OnLeftForce(object sender, System.EventArgs e)
    {
        SetEnableThruster(LeftThrusterParticle, true);
        SetEnableThruster(RightThrusterParticle, false);
    }

    private void SetEnableThruster(ParticleSystem thrusterParticle, bool enable)
    {
        if(enable)
        {
            thrusterParticle.Play();
        }
        else
        {
            thrusterParticle.Stop();
        }
    }

    private void Start()
    {
        lander.OnLanded += Lander_OnLanded;
    }

    private void Lander_OnLanded(object sender, Lander.LanderEventArgs e)
    {
        switch (e.landingType)
        {
            case Lander.landingType.Success:
                break;
            case Lander.landingType.WrongLadingArea:
            case Lander.landingType.SteepAngle:
            case Lander.landingType.TooFast:
                Instantiate(ExplosionParticle, transform.position, Quaternion.identity);
                gameObject.SetActive(false);
                break;
        }
    }

}   