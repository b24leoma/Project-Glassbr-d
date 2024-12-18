using FMODUnity;
using UnityEngine;

public class FMODManager : MonoBehaviour
{
    public static FMODManager instance { get; private set; }
    [SerializeField] private FMODRefData[] fmodRefData;
    private FMOD.Studio.EventInstance _currentTimelineInstance;
    private const string BattleMusic = "Battle";

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.Log("FMOD Manager instance already exists! Destroying...");
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private EventReference GetEventReference(string eventName)
    {
        foreach (var fmodRefDataEntry in fmodRefData)
        {
            foreach (var audioEvent in fmodRefDataEntry.audioEvents)
            {
                if (audioEvent.nameRef == eventName)
                {
                    return audioEvent.eventReference;
                }
            }
             
            // I AM SO SORRY BUT I AM TIRED 
            if (fmodRefDataEntry is HumanFMODRefTemplate humanTemplate)
            {
                foreach (var audioEvent in humanTemplate.attackEvents)
                {
                    if (audioEvent.nameRef == eventName)
                    {
                        return audioEvent.eventReference;
                    }
                }

                foreach (var audioEvent in humanTemplate.damageEvents)
                {
                    if (audioEvent.nameRef == eventName)
                    {
                        return audioEvent.eventReference;
                    }
                }

                foreach (var audioEvent in humanTemplate.deathEvents)
                {
                    if (audioEvent.nameRef == eventName)
                    {
                        return audioEvent.eventReference;
                    }
                }

                foreach (var audioEvent in humanTemplate.moveEvents)
                {
                    if (audioEvent.nameRef == eventName)
                    {
                        return audioEvent.eventReference;
                    }
                }
            }
            else if (fmodRefDataEntry is DemonFMODRefTemplate demonTemplate)
            {
                foreach (var audioEvent in demonTemplate.attackEvents)
                {
                    if (audioEvent.nameRef == eventName)
                    {
                        return audioEvent.eventReference;
                    }
                }

                foreach (var audioEvent in demonTemplate.damageEvents)
                {
                    if (audioEvent.nameRef == eventName)
                    {
                        return audioEvent.eventReference;
                    }
                }

                foreach (var audioEvent in demonTemplate.deathEvents)
                {
                    if (audioEvent.nameRef == eventName)
                    {
                        return audioEvent.eventReference;
                    }
                }

                foreach (var audioEvent in demonTemplate.moveEvents)
                {
                    if (audioEvent.nameRef == eventName)
                    {
                        return audioEvent.eventReference;
                    }
                }
            }
        }

        Debug.LogWarning("FMOD Manager event not found!");
        return default;
    }

    public void NewTimeline(string eventName)
    {
        EventReference eventReference = GetEventReference(eventName);

        if (eventReference.IsNull)
        {
            Debug.LogWarning($"FMOD event '{eventName}' is null or not found!");
            return;
        }

        StopAndRemoveTimeline();

        _currentTimelineInstance = RuntimeManager.CreateInstance(eventReference);
        _currentTimelineInstance.start();
    }

    private void StopAndRemoveTimeline()
    {
        _currentTimelineInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        _currentTimelineInstance.release();
        _currentTimelineInstance.clearHandle();
    }

    public void PauseTimeline(bool pause)
    {
        if (_currentTimelineInstance.isValid())
        {
            _currentTimelineInstance.setPaused(pause);
        }
    }

    public void SetParameter(string parameterName, float value)
    {
        if (_currentTimelineInstance.isValid())
        {
            _currentTimelineInstance.setParameterByName(parameterName, value);
        }
    }

    public void OneShot(string eventName, Vector3? position = null)
    {
        EventReference eventReference = GetEventReference(eventName);

        if (eventReference.IsNull)
        {
            Debug.LogWarning($"FMOD event '{eventName}' is null or not found!");
        }

        if (position.HasValue)
        {
            Vector3 adjustedTopDownPosition = new Vector3(position.Value.x, position.Value.y, 0f);
            RuntimeManager.PlayOneShot(eventReference, adjustedTopDownPosition);
        }
        else
        {
            RuntimeManager.PlayOneShot(eventReference);
        }
    }
}