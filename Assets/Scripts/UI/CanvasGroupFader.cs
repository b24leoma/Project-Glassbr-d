using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class CanvasGroupFader : MonoBehaviour
{
    [SerializeField] CanvasGroup canvasGroup;

    [Range(0f, 10f), SerializeField] private float fadeOutDuration = 1f;

    [Range(0f, 10f), SerializeField] private float fadeInDuration = 1f;

    [SerializeField] UnityEvent afterFadeOut;
    [SerializeField] UnityEvent afterFadeIn;

    private void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void FadeIn()
    {
        if (canvasGroup == null) return;
        canvasGroup.DOFade(1f, fadeInDuration).OnComplete(() => afterFadeIn?.Invoke());
    }

    public void FadeOut()
    {
        if (canvasGroup == null) return;
        canvasGroup.DOFade(0f, fadeOutDuration).OnComplete(() => afterFadeOut?.Invoke());
    }
}