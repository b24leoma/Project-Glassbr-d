using System.Collections;
using DG.Tweening;
using UnityEngine;

public class NoticeButton : MonoBehaviour
{
    [SerializeField] private RectTransform targetUIElement;

    [Header("Shake Settings")] [SerializeField]
    private bool enableShake = true;

    [Range(0f, 20f), SerializeField] private float shakeDelay = 1f;
    [Range(0f, 5f), SerializeField] private float shakeDuration = 0.5f;
    [Range(0f, 50f), SerializeField] private float shakeStrength = 20f;
    [Range(1, 10), SerializeField] private int shakeVibrato = 5;

    [Header("Size Settings")] [SerializeField]
    private bool enableSizeAnimation = true;

    [SerializeField] private LoopType sizeLoopType = LoopType.Yoyo;
    [SerializeField] private Ease sizeEase = Ease.InOutSine;
    [Range(0f, 5f), SerializeField] private float sizeDuration = 0.5f;
    [Range(0f, 5f), SerializeField] private float sizeMultiplier = 1.1f;

    [Header("Rotation Settings")] [SerializeField]
    private bool enableRotation = true;

    [SerializeField] private LoopType rotationLoopType = LoopType.Yoyo;
    [SerializeField] private Ease rotationEase = Ease.InOutSine;
    [Range(0f, 5f), SerializeField] private float rotationDuration = 1f;
    [Range(-45f, 45f), SerializeField] private float rotationAngle = 10f;

    private Vector3 _startScale;
    private Vector3 _startRotation;
    private float _actualRotationDuration;

    private void Start()
    {
        if (targetUIElement == null)
        {
            targetUIElement = gameObject.GetComponent<RectTransform>();
        }
        _startScale = targetUIElement.localScale;
        _startRotation = targetUIElement.eulerAngles;
        _actualRotationDuration = rotationDuration / 2;
        
    }

    private void OnEnable()
    {
        if (enableShake)
        {
            StartCoroutine(ShakeLoop());
        }

        if (enableSizeAnimation)
        {
            DoScale();
        }

        if (enableRotation)
        {
            DoRotate();
        }
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        targetUIElement.DOKill();
        ResetTransform();
    }

    private void ResetTransform()
    {
        targetUIElement.localScale = _startScale;
        targetUIElement.eulerAngles = _startRotation;
    }

    private void DoShake()
    {
        targetUIElement.DOShakeAnchorPos(shakeDuration, shakeStrength, shakeVibrato, fadeOut: true);
    }

    private IEnumerator ShakeLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(shakeDelay);
            DoShake();
        }
    }

    private void DoScale()
    {
        targetUIElement.DOScale(_startScale * sizeMultiplier, sizeDuration)
            .SetLoops(-1, sizeLoopType)
            .SetEase(sizeEase);
    }

    private void DoRotate()
    {
        targetUIElement.DORotate(new Vector3(0, 0, rotationAngle), _actualRotationDuration)
            .SetEase(rotationEase).OnComplete( DoRotateReverse);
    }
    
    private void DoRotateReverse()
    {
        targetUIElement.DORotate(new Vector3(0, 0, -rotationAngle), _actualRotationDuration)
            .SetEase(rotationEase).OnComplete( DoRotate);
    }
}
