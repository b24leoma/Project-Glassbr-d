using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class FULING : MonoBehaviour
{
  [SerializeField] private GameObject target;
  [SerializeField] private GameObject target2;
  [SerializeField] private UnityEvent onAnimationEnd;




   public void Fulinghaha()
   {

       if (target && target2 != null)
       {
           target.SetActive(true);
           target2.SetActive(true);
       }

   }


   private void AnimationEnded()
   {
       onAnimationEnd.Invoke();
   }
}
