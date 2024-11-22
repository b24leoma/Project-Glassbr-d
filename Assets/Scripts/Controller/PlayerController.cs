using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{ 
    public InputActionReference pointerPositionReference;
   public UnityEvent onAction1;
   public UnityEvent onAction2;
   public UnityEvent onAction3;

   private Vector2 mousePostion;

   public void MousePosition()
   {
       // se https://docs.unity3d.com/ScriptReference/Camera.ScreenToWorldPoint.html
   }
   
       
   


   public void Action1 ()
   {
       onAction1.Invoke();
   }
   
   public void Action2 ()
   {
       onAction2.Invoke();
   }
   
   public void Action3 ()
   {
       onAction3.Invoke();
   }


}
