using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerController : MonoBehaviour
{
   public UnityEvent onAction1;
   public UnityEvent onAction2;
   public UnityEvent onAction3;


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
