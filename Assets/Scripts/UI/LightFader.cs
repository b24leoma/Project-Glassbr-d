using DG.Tweening;
using Game;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class LightFader : MonoBehaviour
{
    [SerializeField] private Light2D light2D;
    [SerializeField] private Ease ease;
    [Range (0f,10f),SerializeField] private float duration = 0.1f;
    [Range (0f,10f),SerializeField] private float fadeDuration = 0.2f;
    [Range (0f,10f),SerializeField] private float targetMaxIntensity = 1f;
    [Range (0f,10f),SerializeField] private float targetMinIntensity ;
    public Tween CurrentTween;
    private float _valueOnPause;
    private FriendlyDayNight _friendlyDayNight;
    private string _currentScene;
    
    
    
    private bool _toMaxIntensity = true;
    private bool _isFriendlyDayNightNull;

    private void Start()
    {
       
        if (light2D == null)
        {
            light2D = GetComponent<Light2D>();
        }

        if (_friendlyDayNight == null)
        {
            _currentScene = SceneManager.GetActiveScene().name;
            switch (_currentScene)
            {
                case "MainMenu":
                case "IntroHistoryBook":
                case "MainMenuTryingNewThings":
                    return;
                default:

                    if (_currentScene.Contains("Journal"))
                    {
                        return;
                    }

                    _friendlyDayNight = GameObject.Find("DayNightCycle").GetComponent<FriendlyDayNight>();
                    break;

            }
        }
    }


    public void FadeLightToggle()
    {
       
        CurrentTween?.Kill();
        
        var targetIntensity = _toMaxIntensity ? targetMaxIntensity : targetMinIntensity;

        if (gameObject != null) CurrentTween = DOTween.To(() => light2D.intensity, x => light2D.intensity = x, targetIntensity, duration).SetEase(ease).OnComplete(()=>_toMaxIntensity = !_toMaxIntensity);

        


    }


    public void TurnOffLight()
    {
        CurrentTween?.Kill();
        light2D.intensity = 0f;
    }

    public void FadeInLight()
    {
        if (GetComponentInParent<Entity>() != null && GetComponentInParent<Entity>().hasAttacked) return;
        
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
        CurrentTween?.Kill();
        CurrentTween = DOTween.To(() => light2D.intensity, x => light2D.intensity = x, targetMinIntensity, 0.2f).SetEase(ease);
    }

   

    public void LightSync()
    {
      if (_friendlyDayNight ==null)
      {
          Debug.Log("FriendlyDayNight is null");
          return;
      }
      var gradientSpeed = 1f / _friendlyDayNight.duration;
      var futureGradientPercent =
          Mathf.Repeat(_friendlyDayNight.gradientPercent + gradientSpeed * fadeDuration, 1f);

      var targetIntensity = Mathf.Lerp(targetMinIntensity, targetMaxIntensity, futureGradientPercent);

      CurrentTween?.Kill();
      CurrentTween = DOTween.To(() => light2D.intensity, x => light2D.intensity = x, targetIntensity, fadeDuration)
          .SetEase(ease);
    }
    
    
    
    



    
}