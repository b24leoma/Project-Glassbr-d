using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFader : MonoBehaviour
{ 
    [SerializeField] Light2D light2D;
    [SerializeField] private Ease ease;
    [Range (0f,10f),SerializeField] private float duration = 0.1f;
    [Range (0f,10f),SerializeField] private float targetMaxIntensity = 1f;
    [Range (0f,10f),SerializeField] private float targetMinIntensity ;
    public Tween CurrentTween;
    private float _valueOnPause;
    private FriendlyDayNight _friendlyDayNight;
    
    
    
    private bool _toMaxIntensity = true;

    private void Start()
    {
        if (light2D == null)
        {
            light2D = GetComponent<Light2D>();
        }

        if (_friendlyDayNight == null)
        {
            _friendlyDayNight = FindObjectOfType<FriendlyDayNight>();
        }
    }


    public void FadeLightToggle()
    {
       
        CurrentTween?.Kill();
        
        var targetIntensity = _toMaxIntensity ? targetMaxIntensity : targetMinIntensity;

       CurrentTween= DOTween.To(() => light2D.intensity, x => light2D.intensity = x, targetIntensity, duration).SetEase(ease).OnComplete(()=>_toMaxIntensity = !_toMaxIntensity);

        


    }


    public void TurnOffLight()
    {
        CurrentTween?.Kill();
        light2D.intensity = 0f;
    }

    public void FadeInLight()
    {
        
        CurrentTween?.Kill();

        CurrentTween =  DOTween.To(() => light2D.intensity, x => light2D.intensity = x, targetMaxIntensity, duration).SetEase(ease);
    }
    
    
    public void FadeOutLight()
    {
        CurrentTween?.Kill();
       CurrentTween =  DOTween.To(() => light2D.intensity, x => light2D.intensity = x, targetMinIntensity, duration).SetEase(ease);
    }



    public void PauseTween()
    {
        _valueOnPause = light2D.intensity;
        light2D.intensity = 0f;
        CurrentTween?.Pause();
    }

    public void UnPauseTween()
    {
        if (_friendlyDayNight.gradientPercent >= _friendlyDayNight.whenNightInGradient)
        {
            light2D.intensity = _valueOnPause;
            CurrentTween?.Play();
        }
       
    }
    
    
    
    



    
}