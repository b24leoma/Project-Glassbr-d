using DG.Tweening;
using TMPro;
using UnityEngine;

public class TextGlowAnimator : MonoBehaviour
{
    private static readonly int GlowOuter = Shader.PropertyToID("_GlowOuter");
    [SerializeField] private TMP_Text targetText;
    [Range(0f, 10f), SerializeField] private float durationFadeInLoop  = 2f;
    [Range(0f, 5f), SerializeField] private float durationFadeOut  = 0.2f;
    [Range(0f, 1f), SerializeField] private float targetValue = 1f;
    private float _glowOuter;
    [SerializeField] private LoopType loopType;
    [SerializeField] private Ease easeType;
    private Tween _currentGlowTween;
    

    private bool _glowToggleIsAnimating;
    void Start()
    {
        if (targetText.fontMaterial == null)
        {
            Debug.LogError("Material saknas på targetText!");
        }
        
        
    }

    void OnDisable()
    {
        _currentGlowTween?.Kill();
        targetText.fontMaterial.SetFloat(GlowOuter, 0);
        _glowToggleIsAnimating = false;
    }

    public void AnimateTextGlowToggle(bool toggle)
    {
        if (toggle && !_glowToggleIsAnimating)
        {
            _glowToggleIsAnimating = true;
            _currentGlowTween =  DOTween.To(() => _glowOuter, x =>
                {
                    _glowOuter = x;
                    targetText.fontMaterial.SetFloat(GlowOuter, _glowOuter);
                }, targetValue, durationFadeInLoop)
                .SetLoops(-1, loopType)
                .SetEase(easeType);
        }
        else if (!toggle && _glowToggleIsAnimating)
        {
            _currentGlowTween?.Kill();
            _currentGlowTween = DOTween.To(() => _glowOuter, x =>
                {
                    
                    _glowOuter = x;
                    targetText.fontMaterial.SetFloat(GlowOuter, _glowOuter);
                }, 0, durationFadeOut)
                .SetEase(easeType)
                .OnComplete(() => { _glowToggleIsAnimating = false; });
        }
    }


   //wtf det är inbyggt pog
    
    
}