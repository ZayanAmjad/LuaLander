using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Lander : MonoBehaviour
{
    private Rigidbody2D landerRigidBody;
    public event EventHandler OnLeftForce;
    public event EventHandler OnRightForce;
    public event EventHandler OnMiddleForce;
    public event EventHandler OnBeforeForce;
    public event EventHandler OnCoinPickup;
    public event EventHandler<LanderEventArgs> OnLanded;
    public class LanderEventArgs : EventArgs
    {
        public int Score;
    }
    public static Lander Instance { get; private set; }
    private float Maxfuel = 20f;
    private float fuel;

    private void consumeFuel()
    {
        float amount = 1f;
        fuel -= (amount * Time.deltaTime);
        fuel = Mathf.Max(0, fuel);
    }
    
    private void Awake()
    {
        landerRigidBody = GetComponent<Rigidbody2D>();  
        Instance = this;  
        fuel = Maxfuel;    
    }

    private void FixedUpdate()
    {
        OnBeforeForce?.Invoke(this, EventArgs.Empty);

        if(Keyboard.current.upArrowKey.isPressed || Keyboard.current.leftArrowKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
        {
            consumeFuel();
        }

        if(fuel <= 0f)
        {
            return;
        }

        if(Keyboard.current.upArrowKey.isPressed){
            float force = 1000f;
            landerRigidBody.AddForce(force * transform.up * Time.deltaTime);
            OnMiddleForce?.Invoke(this, EventArgs.Empty);
        }

        if(Keyboard.current.leftArrowKey.isPressed){
            float turnSpeed  = +150f;
            landerRigidBody.AddTorque(turnSpeed * Time.deltaTime);
            OnLeftForce?.Invoke(this, EventArgs.Empty);
        }   

        if(Keyboard.current.rightArrowKey.isPressed){
            float turnSpeed  = -150f;
            landerRigidBody.AddTorque(turnSpeed * Time.deltaTime);
            OnRightForce?.Invoke(this, EventArgs.Empty);
        }     
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if(!collision.gameObject.TryGetComponent(out LandingPad pad))
        {
            //Debug.Log("You Crashed!");
            return;
        }

        float softLandingSpeed = 10f;
        if(collision.relativeVelocity.magnitude > softLandingSpeed)
        {
            Debug.Log("Landed too hard bro!");
            return;
        }

        float dotVector = Vector2.Dot(Vector2.up, transform.up);
        if(dotVector < 0.9f)
        {
            //Debug.Log("You Crashed! deenga");
            return;
        }
        
        int maxScore = 1000;
        float ScoreMultiplier = 100f;
        int score = maxScore - Mathf.RoundToInt(collision.relativeVelocity.magnitude * ScoreMultiplier);
        Debug.Log("Score multiplier: " + pad.GetScoreMultiplier());
        score += Mathf.RoundToInt(dotVector * ScoreMultiplier * pad.GetScoreMultiplier());
        score = Mathf.Max(0, score);

        OnLanded?.Invoke(this, new LanderEventArgs { Score = score });
        
        //Debug.Log("Successful Landing! Score: " + score);

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out FuelPickup fuelPickup))
        {
            float addedamount = 10f;
            fuel += addedamount;
            if(fuel > Maxfuel)
            {
                fuel = Maxfuel;
            }
            fuelPickup.Destroy();
            return;
        }

        if(collision.gameObject.TryGetComponent(out CoinPickup coinPickup))
        {
            OnCoinPickup?.Invoke(this, EventArgs.Empty);
            coinPickup.Destroy();
            return;
        }
    }

    public float GetSpeedX()
    {
        return landerRigidBody.linearVelocity.x;
    }

    public float GetSpeedY()
    {
        return landerRigidBody.linearVelocity.y;
    }

    public float GetFuel()
    {
        return fuel/Maxfuel;
    }



}
