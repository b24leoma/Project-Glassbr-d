using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VignetteController : MonoBehaviour
{
    [SerializeField] private Volume volume;
    private Vignette _vignette;

    void Start()
    {
        if (volume != null && volume.sharedProfile.TryGet(out Vignette outVignette))
        {
            _vignette = outVignette;
            _vignette.intensity.value = 0.2f;
            _vignette.center.value = new Vector2(0.5f, 0.5f);
        }
    }

   public void VignetteFade(float vignetteDuration)
    {
        if (_vignette==null) return;

        DOTween.To(() => _vignette.intensity.value, x => _vignette.intensity.value = x, 1.0f, vignetteDuration);
        
    }
   
}
