using FMODUnity;
using UnityEngine;

public class FMODManager : MonoBehaviour
{
    public static FMODManager instance { get; private set; }
    [SerializeField] private FMODRefData [] fmodRefData;
private FMOD.Studio.EventInstance _currentTimelineInstance;
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
    
    
    private EventReference GetEventReference (string eventName)
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
        }

        
        
        Debug.LogWarning("FMOD Manager event not found!");
        return default;
    }


    public void Timeline(string eventName)
    {
        EventReference eventReference = GetEventReference(eventName);

        if (eventReference.IsNull)
        {
            Debug.LogWarning($"FMOD event '{eventName}' is null or not found!");
            return;
        }
        
        StopTimeline();

        _currentTimelineInstance = RuntimeManager.CreateInstance(eventReference);
        _currentTimelineInstance.start();
    }
    
    public void StopTimeline()
    {
        if (_currentTimelineInstance.isValid())
        {
            _currentTimelineInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            _currentTimelineInstance.release();
            _currentTimelineInstance.clearHandle();
        }
    }

    public void PauseTimeline(bool pause)
    {
        if (_currentTimelineInstance.isValid())
        {
            _currentTimelineInstance.setPaused(pause);
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
            RuntimeManager.PlayOneShot(eventReference, position.Value);
        }
        else
        {
            RuntimeManager.PlayOneShot(eventReference);
        }
    }
}
