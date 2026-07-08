using UnityEngine;
using System;
using UnityEngine.InputSystem.EnhancedTouch;

public class GameInput : MonoBehaviour
{

    public EventHandler OnMenuPressed;
    public static GameInput Instance { get; private set; }
    private InputActions actions;

    void OnEnable()
    {
        // EnhancedTouch requires explicit activation to manage performance overhead
        EnhancedTouchSupport.Enable();
    }

    void OnDisable()
    {
        // Disable when the script is inactive to clean up resources
        EnhancedTouchSupport.Disable();
    }

    void Awake(){
        Instance = this;
        actions = new InputActions();
        actions.Enable();

        actions.Player.Menu.performed += Menu_performed;
    }

    private void Menu_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        OnMenuPressed?.Invoke(this, EventArgs.Empty);
    }
    public bool isUpPressed(){
        return actions.Player.LanderUp.IsPressed();
    } 

    public bool isRightPressed(){
        return actions.Player.LanderRight.IsPressed();
    }

    public bool isLeftPressed(){
        return actions.Player.LanderLeft.IsPressed();
    }

    private void OnDestroy(){
        actions.Disable();
    }
}
