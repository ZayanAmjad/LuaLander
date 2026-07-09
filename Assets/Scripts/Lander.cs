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
    public event EventHandler OnFuelPickup;
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
    private const float GRAVITY_NORMAL = 0.7f;
    private const float MAX_SPEED = 15f;
    private bool hasResolvedLanding = false;
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
        landerRigidBody.angularDamping = 2f; 
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
                if(GameInput.Instance.isUpPressed() || GameInput.Instance.isLeftPressed() || GameInput.Instance.isRightPressed())
                {
                    landerRigidBody.gravityScale = GRAVITY_NORMAL;
                    state = State.Normal;
                    SetState(State.Normal);
                }
                break;
            case State.Normal:
                if(GameInput.Instance.isUpPressed() || GameInput.Instance.isLeftPressed() || GameInput.Instance.isRightPressed())
                {
                    consumeFuel();
                }

                if(fuel <= 0f)
                {
                    return;
                }
                //float rotationSpeed = 50f;
                if(GameInput.Instance.isUpPressed()){
                    float force = 1000f;
                    landerRigidBody.AddForce(force * transform.up * Time.deltaTime);
                    OnMiddleForce?.Invoke(this, EventArgs.Empty);
                }

                if(GameInput.Instance.isLeftPressed()){
                    float turnSpeed  = +50f;
                    landerRigidBody.AddTorque(turnSpeed * Time.deltaTime);
                    //landerRigidBody.angularVelocity = rotationSpeed;
                    OnLeftForce?.Invoke(this, EventArgs.Empty);
                }   

                if(GameInput.Instance.isRightPressed()){
                    float turnSpeed  = -50f;
                    landerRigidBody.AddTorque(turnSpeed * Time.deltaTime);
                    //landerRigidBody.angularVelocity = -rotationSpeed;
                    OnRightForce?.Invoke(this, EventArgs.Empty);
                }
                break;
            case State.GameOver:
                break;
        }

        landerRigidBody.linearVelocity = Vector2.ClampMagnitude(landerRigidBody.linearVelocity, MAX_SPEED);

             
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (hasResolvedLanding)
        {
            return;
        }

        if(!collision.gameObject.TryGetComponent(out LandingPad pad))
        {
            hasResolvedLanding = true;
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
            hasResolvedLanding = true;
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
            hasResolvedLanding = true;
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

        hasResolvedLanding = true;
        landerRigidBody.linearVelocity = Vector2.zero;
        landerRigidBody.angularVelocity = 0f;
        landerRigidBody.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.rotation = Quaternion.identity;
        
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
            OnFuelPickup?.Invoke(this, EventArgs.Empty);
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

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (Time.timeScale <= 0f)
        {
            return;
        }

        if(collision.gameObject.TryGetComponent(out Boundary boundary))
        {
            
            HandleOutOfBounds();
        }
    }

    private void HandleOutOfBounds()
    {
        if (hasResolvedLanding)
        {
            return;
        }

        hasResolvedLanding = true;
        //Debug.Log("You went out of bounds!");
        OnLanded?.Invoke(this, new LanderEventArgs 
        {   
            Score = 0,
            landingType = landingType.WrongLadingArea,
            dotVector = Vector2.Dot(Vector2.up, transform.up),
            landingSpeed = landerRigidBody.linearVelocity.magnitude ,
            timeElapsed = GameManager.Instance.GetTimeElapsed()
        });
        SetState(State.GameOver);
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
