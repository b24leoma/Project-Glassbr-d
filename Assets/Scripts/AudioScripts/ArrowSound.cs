using DG.Tweening;
using FMOD.Studio;
using FMODUnity;
using UnityEngine;

public class ArrowSound : MonoBehaviour
{
    private EventInstance _arrowSoundInstance;

    private Vector3 _previousPosition;
    private float _previousTime;

    private void Start()
    {
        _arrowSoundInstance = RuntimeManager.CreateInstance("event:/Arrow");
        RuntimeManager.AttachInstanceToGameObject(_arrowSoundInstance, transform);
        _arrowSoundInstance.start();
    }

    public void ArrowDuration(float duration)
    {
        var halfDuration = duration / 2f;
        DOTween.To(() => 0f, ArrowParameterChange, 1f, halfDuration)
            .SetLoops(2, LoopType.Yoyo)
            .OnComplete(FadeAndRelease);
    }

    private void ArrowParameterChange(float currentValue)
    {
        _arrowSoundInstance.setParameterByName("ArrowVelocity", currentValue);
    }

    private void FadeAndRelease()
    {
        _arrowSoundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        _arrowSoundInstance.release();
    }

    private void OnDestroy()
    {
        DOTween.Sequence()
            .AppendInterval(1f)
            .OnComplete(() =>
            {
                _arrowSoundInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                _arrowSoundInstance.release();
                DOTween.Kill(this);
            });
    }
}
