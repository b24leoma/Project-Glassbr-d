using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class SpriteFader : MonoBehaviour
{
    [SerializeField] SpriteRenderer sprite;

    [Range(0f, 10f), SerializeField] private float fadeOutDuration = 1f;

    [Range(0f, 10f), SerializeField] private float fadeInDuration = 1f;

    [SerializeField] UnityEvent afterFadeOut;
    [SerializeField] UnityEvent afterFadeIn;

   

    public void FadeIn()
    {
        if (sprite == null) return;
        sprite.DOFade(1f, fadeInDuration).OnComplete(() => afterFadeIn?.Invoke());
    }

    public void FadeOut()
    {
        if (sprite == null) return;
        sprite.DOFade(0f, fadeOutDuration).OnComplete(() => afterFadeOut?.Invoke());
    }
}