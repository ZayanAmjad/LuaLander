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
    
    private void Awake()
    {
        landerRigidBody = GetComponent<Rigidbody2D>();        
    }

    private void FixedUpdate()
    {
        OnBeforeForce?.Invoke(this, EventArgs.Empty);

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
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if(!collision.gameObject.TryGetComponent(out LandingPad pad))
        {
            //Debug.Log("You Crashed!");
            return;
        }

        float softLandingSpeed = 5f;
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
        
        //Debug.Log("Successful Landing! Score: " + score);

    }
}
