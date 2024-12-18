using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightFader : MonoBehaviour
{ 
    [SerializeField] Light2D light2D;
    [SerializeField] private Ease ease;
    [Range (0f,10f),SerializeField] private float duration = 0.1f;
    [Range (0f,10f),SerializeField] private float targetMaxIntensity = 1f;
    [Range (0f,10f),SerializeField] private float targetMinIntensity;
    private Tween _currentTween;
    
    
    private bool _toMaxIntensity = true;

    private void Start()
    {
        if (light2D == null)
        {
            light2D = GetComponent<Light2D>();
        }
    }


    public void FadeLightToggle()
    {
        var targetIntensity = _toMaxIntensity ? targetMaxIntensity : targetMinIntensity;

        DOTween.To(() => light2D.intensity, x => light2D.intensity = x, targetIntensity, duration).SetEase(ease).OnComplete(()=>_toMaxIntensity = !_toMaxIntensity);

        


    }


    public void TurnOffLight()
    {
        light2D.intensity = 0f;
    }

    public void FadeInLight()
    {
        _currentTween?.Kill();

        _currentTween =  DOTween.To(() => light2D.intensity, x => light2D.intensity = x, 1, duration).SetEase(ease);
    }
    
    
    public void FadeOutLight()
    {
        _currentTween?.Kill();
       _currentTween =  DOTween.To(() => light2D.intensity, x => light2D.intensity = x, 0, duration).SetEase(ease);
    }

}