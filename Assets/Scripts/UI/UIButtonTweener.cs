using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Playables;

public class UIButtonTweener : MonoBehaviour
{
    [SerializeField] RectTransform targetUIElement;

    [SerializeField] private LoopType loopType;
    [SerializeField] private Ease ease;
    private Vector3 _startLocalValue;
    private Vector3 _startWorldValue;
    private Vector3 _startScale;

    void Start()
    {
        if (targetUIElement == null)
        {
            targetUIElement = gameObject.GetComponent<RectTransform>();
        }

        _startLocalValue = targetUIElement.localPosition;
        _startWorldValue = targetUIElement.position;
        _startScale = gameObject.transform.localScale;
    }


    public void UIShake()
    {
        targetUIElement.DOShakeAnchorPos(0.2f, 10, 1).SetLoops(1, loopType).SetEase(ease);
    }



    public void UISize()
    {

        var endValue = _startScale*1.1f;
        if (targetUIElement.transform.localScale == endValue)
        {
            endValue = _startScale;
        }


        targetUIElement.transform.DOScale(endValue,  0.1f).SetLoops(1, loopType).SetEase(ease);
        
        //not done 

    }
    
}
