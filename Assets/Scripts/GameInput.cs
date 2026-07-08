using UnityEngine;
using System;

public class GameInput : MonoBehaviour
{

    public EventHandler OnMenuPressed;
    public static GameInput Instance { get; private set; }
    private InputActions actions;

    void OnEnable()
    {
        if (actions == null)
        {
            actions = new InputActions();
            actions.Player.Menu.performed += Menu_performed;
        }

        actions.Enable();
    }

    void Awake(){
        Instance = this;
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

    private void OnDisable()
    {
        if (actions != null)
        {
            actions.Disable();
        }
    }

    private void OnDestroy(){
        if (actions != null)
        {
            actions.Player.Menu.performed -= Menu_performed;
            actions.Disable();
            actions.Dispose();
            actions = null;
        }
    }
}
