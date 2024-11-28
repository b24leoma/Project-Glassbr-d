using FMODUnity;
using UnityEngine;

public class FMODManager : MonoBehaviour
{
    public static FMODManager instance { get; private set; }
    [SerializeField] private FMODRefData [] fmodRefData;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("FMOD Manager instance already exists!");
        }
        instance = this;
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
