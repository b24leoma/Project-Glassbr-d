using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using Random = System.Random;

public class FriendlyDayNight : MonoBehaviour
{
    [SerializeField] private float duration;
    [SerializeField] private Gradient gradient;

    private Light2D targetLight;
     [SerializeField] private float gradientPercent;
     [Range(0f,1f),SerializeField] private float endValue = 1;

    [SerializeField] private Ease ease;
    [SerializeField] private LoopType loopType;

    [SerializeField] private int cycleCount;
    [SerializeField] private int dygnCount;
    [SerializeField] private ParticleSystem fog;
    [Range(0, 10), SerializeField] private float chanceForNoFog = 8f;

    [SerializeField] private int slumptal;

    private void Awake()
    {
        targetLight = GetComponent<Light2D>();
        gradientPercent = 0f;
        StartDygn();
    }

    private void StartDygn()
    {
        DOTween.To(() => gradientPercent, x => gradientPercent = x, endValue, duration)
            .SetLoops(-1, loopType)
            .SetEase(ease)
            .OnStepComplete(() => CycleCounter());
    }

    private void Update()
    {
        targetLight.color = gradient.Evaluate(gradientPercent);
    }

    private void CycleCounter()
    {
        cycleCount++;
        dygnCount = cycleCount / 2;
        if (cycleCount % 2 != 0)
        {
            var randomNumber = new Random();
            slumptal = randomNumber.Next(10);

            if (slumptal >= chanceForNoFog)
            {
                Debug.Log("Playing fog, should last for 60 seconds...");
                fog.Play();
            }
        }
    }
}