using UnityEngine;
using System;

public class GameInput : MonoBehaviour
{

    public EventHandler OnMenuPressed;
    public static GameInput Instance { get; private set; }
    private InputActions actions;
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
