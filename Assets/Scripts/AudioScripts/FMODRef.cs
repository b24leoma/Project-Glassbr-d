using UnityEngine;
using FMODUnity;
using UnityEngine.Rendering;


[CreateAssetMenu(menuName = "Audio/FMODSound")]

public class FMODRefData : ScriptableObject
{
    [System.Serializable]
    public class AudioEvent
    { 
        public string nameRef;
        public EventReference eventReference;
    }
    
    public AudioEvent[] audioEvents;
    
}
