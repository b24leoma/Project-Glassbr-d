using UnityEngine;
using UnityEngine.Rendering.Universal;
using DG.Tweening;
using Game;
using Random = System.Random;

public class FriendlyDayNight : MonoBehaviour
{
    [SerializeField] private Gradient gradient;
    [Range(0f, 1f), SerializeField] private float whenNightInGradient;
    [SerializeField] private float duration;
    [SerializeField] private Ease ease;
    [SerializeField] private LoopType loopType;
    [Range(0, 10), SerializeField] private float chanceForNoFog = 8f;
    
    
    [Header ("Assignables")]
    [SerializeField] private BattleController battleController;
    [SerializeField] private ParticleSystem fog;
    private Light2D _targetLight;
    [Header("Debug")] 
    [SerializeField] private bool hasToggled;
    [SerializeField] private bool hasFoggedThisNight;
    [SerializeField] private int cycleCount;
    [SerializeField] private int dygnCount;
    [SerializeField] private int slumptal;
    [SerializeField] private float gradientPercent;
    [Range(0f, 1f), SerializeField] private float endValue = 1;

    private void Awake()
    {
        _targetLight = GetComponent<Light2D>();
        gradientPercent = 0f;
        StartDygn();
    }

    private void StartDygn()
    {
        DOTween.To(() => gradientPercent, x => gradientPercent = x, endValue, duration)
            .SetLoops(-1, loopType)
            .SetEase(ease)
            .OnStepComplete(CycleCounter).OnUpdate(TimeChecker);
    }

    private void CycleCounter()
    {
        cycleCount++;
        dygnCount = cycleCount / 2;


        if (cycleCount % 2 != 0) return;
        var randomNumber = new Random();
        slumptal = randomNumber.Next(10);
        Debug.Log("This cycle RNG: " + slumptal);



    }


    private void TimeChecker()
    {
        
        
        // OMG IM GOOOONNNAA NEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEST
        _targetLight.color = gradient.Evaluate(gradientPercent);
        
        
        
        
        if (cycleCount%2 == 0)
        {
            if (!(gradientPercent >= whenNightInGradient)) return;
            
            if (!hasToggled)
            {
                battleController.ToggleNightLightOnHumans(true);
                TryFog();
                hasToggled = true;
            }


        }

        if (cycleCount % 2 != 0)
        {
            if (gradientPercent >= whenNightInGradient) return;


            if (hasToggled)
            {
                battleController.ToggleNightLightOnHumans(false);
                hasToggled = false;
            }

        }

    }


    private void TryFog()
    {
        if (!(slumptal >= chanceForNoFog)) return;
        Debug.Log("Playing fog, should last for 60 seconds...");
        fog.Play();
    }



    
    
}