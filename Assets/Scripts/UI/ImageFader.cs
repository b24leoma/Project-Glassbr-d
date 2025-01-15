using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ImageFader : MonoBehaviour
{
    [SerializeField] Image imageObject;

    [Range(0f, 10f), SerializeField] private float fadeOutDuration = 1f;

    [Range(0f, 10f), SerializeField] private float fadeInDuration = 1f;

    [SerializeField] UnityEvent afterFadeOut;
    [SerializeField] UnityEvent afterFadeIn;
    


    public void FadeIn()
    {
        if (imageObject == null) return;
        imageObject.DOFade(1f, fadeInDuration).OnComplete(() => afterFadeIn?.Invoke());
    }

    public void FadeOut()
    {
        if (imageObject == null) return;
        imageObject.DOFade(0f, fadeOutDuration).OnComplete(() => afterFadeOut?.Invoke());
    }
}