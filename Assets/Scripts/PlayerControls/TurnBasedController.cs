using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class TurnBasedController : MonoBehaviour
{
    public UnityEvent onAction1;
    public UnityEvent onAction2;
    public UnityEvent onLeftClick;
    public UnityEvent onMouseMove;
    public UnityEvent onRightClick;
    public UnityEvent onAdd;   // Start/Add
    public UnityEvent onBackRemove; // Remove/Back
    public InputActionReference pointerPositionReference;
    
    
    public Vector2 PointerPosition {get; private set;}
    public void Action1(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            onAction1.Invoke();
            Debug.Log("Diddykong");
        }
    }
    public void Action2(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            onAction2.Invoke();
        }
    }

    public void Back_Remove(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            onBackRemove.Invoke();
        }
    }
  
    public void Add(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            onAdd.Invoke();
        }
    }
    

    public void LeftClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PointerPosition = pointerPositionReference.action.ReadValue<Vector2>();
            Debug.Log("Coordinates:" + PointerPosition);
            onLeftClick.Invoke();
        }
    }
    
    
    public void RightClick(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            PointerPosition = pointerPositionReference.action.ReadValue<Vector2>();
            Debug.Log("Coordinates:" + PointerPosition);
            onRightClick.Invoke();
        }
    }
}
