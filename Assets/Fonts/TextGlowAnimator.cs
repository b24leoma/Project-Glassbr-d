using DG.Tweening;
using TMPro;
using UnityEngine;

public class TextGlowAnimator : MonoBehaviour
{
    private static readonly int GlowOuter = Shader.PropertyToID("_GlowOuter");
    [SerializeField] private TMP_Text targetText;
    [Range(0f, 100f), SerializeField] private float duration  = 50f;
    [Range(0f, 1f), SerializeField] private float targetValue = 1f;
    private float _glowOuter;
    [SerializeField] private LoopType loopType;
    [SerializeField] private EaseType easeType;
    void Start()
    {
        if (targetText.fontMaterial == null)
        {
            Debug.LogError("Material saknas på targetText!");
            return;
        }

        AnimateText();
    }

    private void AnimateText()
    {
        DOTween.To(() => _glowOuter, x =>
            {
                _glowOuter = x;
                targetText.fontMaterial.SetFloat(GlowOuter, _glowOuter);
            }, targetValue, duration)
            .SetLoops(-1, (DG.Tweening.LoopType)loopType)
            .SetEase((Ease)easeType);
    }


    private enum LoopType
    {
        Yoyo,
        Restart,
        Incremental
    }
    
    
    private enum EaseType
    {
        InOutSine,
        InSine,
        OutSine,
        Linear,
        InQuad,
        OutQuad,
        InOutQuad,
        InCubic,
        OutCubic,
        InOutCubic
    }
    
    
    
}