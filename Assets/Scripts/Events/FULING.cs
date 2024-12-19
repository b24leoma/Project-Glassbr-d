using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class FULING : MonoBehaviour
{
  [SerializeField] private GameObject target;
  [SerializeField] private UnityEvent onAnimationEnd;




   public void Fulinghaha()
   {

      
         target.SetActive(true); 
      

   }


   private void AnimationEnded()
   {
       onAnimationEnd.Invoke();
   }
}
