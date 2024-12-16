using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;

public class FriendlyDayNight : MonoBehaviour
{


  [SerializeField] private float duration;
  [SerializeField]   private Gradient gradient;
  
  private Light2D _targetLight;
  [SerializeField] private int dygncount;

/*
  private void Awake()
  {
    _targetLight = GetComponent<Light2D>();

    StartDygn();
  }


 private void StartDygn
  {
    DOTween
  }
 */
}
