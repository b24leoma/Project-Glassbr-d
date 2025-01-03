using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;

public class ShakeSometimes : MonoBehaviour
{
   [SerializeField] private RectTransform rectTransform;
   [Range(0f, 20f), SerializeField] private float delay; 
   [Range(0f, 20f), SerializeField] private float duration;
   [Range(0f, 20f), SerializeField] private float shakeAmount;
   private IEnumerator shakeLoop;


  

   private void OnEnable()
   {
      shakeLoop = ShakeLoop();
   }

   private void DoShake()
   {
      rectTransform.DOShakeAnchorPos(duration, shakeAmount).OnKill(() =>
      {
         ShakeLoop();
      });
   }

   private IEnumerator ShakeLoop()
   {
      yield return new WaitForSeconds(delay);
      DoShake();
   }


   public void Update()
   {
      if (Mouse.current.leftButton.isPressed)
      {
         DoShake();
      }
   }
}
