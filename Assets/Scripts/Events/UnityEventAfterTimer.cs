using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

// AI Created Script because I am lazy
public class UnityEventAfterTimer : MonoBehaviour
{
    public UnityEvent onTimerCompleted;
    
    [SerializeField] private float timerLength = 30f;
    private float defaultTime;

    public TextMeshProUGUI timerValueUI;
    private bool displayText = false;

    private void Start()
    {
        defaultTime = timerLength;
        
        if (timerValueUI != null)
        {
            displayText = true;
        }
    }

    public void StartTimer()
    {
        StopAllCoroutines(); // Stoppa eventuell pågående timer innan en ny startas
        StartCoroutine(TimerCoroutine());
    }

    private IEnumerator TimerCoroutine()
    {
        float timeRemaining = timerLength;

        while (timeRemaining > 0)
        {
            if (displayText)
            {
                timerValueUI.SetText($"{(int)timeRemaining}");
            }

            Debug.Log($"Time left: {(int)timeRemaining}");

            yield return new WaitForSeconds(1f); // Vänta en sekund mellan varje uppdatering
            timeRemaining--;
        }

        onTimerCompleted.Invoke();

        // Återställ timer
        timerLength = defaultTime;
    }
}