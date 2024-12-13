using DG.Tweening;
using UnityEngine;

public class UIButtonTweener : MonoBehaviour
{
    [SerializeField] RectTransform targetUIElement;

    [SerializeField] private LoopType loopType;
    [SerializeField] private Ease ease;
    [Range(0f,5f),SerializeField] private float sizeDuration = 0.1f;
    [Range(-5f,5f),SerializeField] private float sizeMultiplier=1.1f;
    
    
    
    private Vector3 _startScale;
    

    void Start()
    {
        if (targetUIElement == null)
        {
            targetUIElement = gameObject.GetComponent<RectTransform>();
        }
        
        _startScale = targetUIElement.localScale;

       
    }


    public void UIShake()
    {
        targetUIElement.DOShakeAnchorPos(0.2f, 10, 1).SetLoops(2, loopType).SetEase(ease);
    }


    public void UISizeIn()
    {
        targetUIElement.transform.DOScale(_startScale * sizeMultiplier, sizeDuration).SetEase(ease);
    }

    public void UISizeOut()
    {
        targetUIElement.transform.DOScale(_startScale,sizeDuration).SetEase(ease).OnComplete(() =>
        {
            if (targetUIElement.localScale != _startScale)
            {
                targetUIElement.transform.DOScale(_startScale, sizeDuration).SetEase(ease);
            }
        });
    }


    public void UISizeInAndOut()
    {
        targetUIElement.transform.DOScale(_startScale*sizeMultiplier,sizeDuration).SetLoops(2,loopType).SetEase(ease).OnComplete(() =>
        {
            if (targetUIElement.localScale != _startScale)
            {
                targetUIElement.transform.DOScale(_startScale, sizeDuration).SetEase(ease);
            }
        });
    }
    
}
