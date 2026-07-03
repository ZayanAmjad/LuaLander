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
        public landingType landingType;
        public float dotVector;
        public float landingSpeed;
        public float timeElapsed;
    }

    public EventHandler<OnStateChangedEventArgs> OnStateChanged;

    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }
    public static Lander Instance { get; private set; }
    public enum landingType
    {
        Success,
        WrongLadingArea,
        SteepAngle,
        TooFast
    }
    private float Maxfuel = 20f;
    private float fuel;
    private const float GRAVITY_NORMAL = 0.5f;
    public enum State
    {
        WaitingToStart,
        Normal,
        GameOver
    }

    private State state;

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
        state = State.WaitingToStart;
    }

    private void FixedUpdate()
    {
        OnBeforeForce?.Invoke(this, EventArgs.Empty);

        switch(state)
        {
            case State.WaitingToStart:
                if(Keyboard.current.upArrowKey.isPressed || Keyboard.current.leftArrowKey.isPressed || Keyboard.current.rightArrowKey.isPressed)
                {
                    landerRigidBody.gravityScale = GRAVITY_NORMAL;
                    state = State.Normal;
                    SetState(State.Normal);
                }
                break;
            case State.Normal:
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
                    float turnSpeed  = +50f;
                    landerRigidBody.AddTorque(turnSpeed * Time.deltaTime);
                    OnLeftForce?.Invoke(this, EventArgs.Empty);
                }   

                if(Keyboard.current.rightArrowKey.isPressed){
                    float turnSpeed  = -50f;
                    landerRigidBody.AddTorque(turnSpeed * Time.deltaTime);
                    OnRightForce?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
        }

             
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if(!collision.gameObject.TryGetComponent(out LandingPad pad))
        {
            //Debug.Log("You Crashed!");
            OnLanded?.Invoke(this, new LanderEventArgs 
            {
                Score = 0,
                landingType = landingType.WrongLadingArea,
                dotVector = Vector2.Dot(Vector2.up, transform.up),
                landingSpeed = collision.relativeVelocity.magnitude ,
                timeElapsed = GameManager.Instance.GetTimeElapsed()
            });

            SetState(State.GameOver);
            return;
        }

        float softLandingSpeed = 10f;
        float dotVector = Vector2.Dot(Vector2.up, transform.up);

        if(collision.relativeVelocity.magnitude > softLandingSpeed)
        {
            //Debug.Log("Landed too hard bro!");
            OnLanded?.Invoke(this, new LanderEventArgs 
            {   
            Score = 0,
            landingType = landingType.TooFast,
            dotVector = dotVector,
            landingSpeed = collision.relativeVelocity.magnitude ,
            timeElapsed = GameManager.Instance.GetTimeElapsed()
            });
            SetState(State.GameOver);
            return;
        }
        
        if(dotVector < 0.9f)
        {
            //Debug.Log("You Crashed! deenga");
            OnLanded?.Invoke(this, new LanderEventArgs 
            {   
            Score = 0,
            landingType = landingType.SteepAngle,
            dotVector = dotVector,
            landingSpeed = collision.relativeVelocity.magnitude ,
            timeElapsed = GameManager.Instance.GetTimeElapsed()
            });
            SetState(State.GameOver);
            return;
        }

        float maxScore = 1000f;
        float speedFactor = Mathf.InverseLerp(softLandingSpeed, 0f, collision.relativeVelocity.magnitude);
        float angleFactor = Mathf.InverseLerp(0.9f, 1f, dotVector);
        int score = Mathf.RoundToInt(maxScore * ((speedFactor + angleFactor) * 0.5f));
        score *= pad.GetScoreMultiplier();
        score = Mathf.Max(0, score);

        OnLanded?.Invoke(this, new LanderEventArgs 
        {   Score = score,
            landingType = landingType.Success,
            dotVector = dotVector,
            landingSpeed = collision.relativeVelocity.magnitude ,
            timeElapsed = GameManager.Instance.GetTimeElapsed()
        });
        SetState(State.GameOver);
        
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

    private void SetState(State newState)
    {
        state = newState;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs { state = state });
    }


}
