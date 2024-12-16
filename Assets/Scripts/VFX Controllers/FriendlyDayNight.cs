using System;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using Color = System.Drawing.Color;

public class FriendlyDayNight : MonoBehaviour
{


  [SerializeField] private float duration;
  [SerializeField]   private Gradient gradient;
  
  private Light2D _targetLight;
  private float _currentTime;
  private float _startTime;
  [SerializeField] private float _gradientPercent;
  [SerializeField]   private float _gradientEnd = 1f;
  [SerializeField] private float _gradientStart;
  [SerializeField] private float _endValue;
  [SerializeField] private bool _toEnd;
  

 [SerializeField] private float uno;
  [SerializeField] private float dos;
  

  [SerializeField]  private Ease ease;
  [SerializeField]      private LoopType loopType;

  [SerializeField] private int dygncount;


  private void Awake()
  {
    _targetLight = GetComponent<Light2D>();
    _startTime = Time.time;
    _gradientPercent = 0f;

   

    StartDygn();
  }


  private void StartDygn()
  {

    _endValue = _toEnd ? _gradientEnd : _gradientStart;

    DOTween.To(() => _gradientPercent, x => _gradientPercent = x, 1, duration).SetLoops(-1, loopType).SetEase(ease);


    _toEnd = !_toEnd;


  }


  private void Update()
  {
    _targetLight.color = gradient.Evaluate(_gradientPercent);
      
      
    
  }
}
