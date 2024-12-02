using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventAudioPlay : MonoBehaviour
{

    // This is intended to be used with Unity events.
    
    public void PlayUISelect()
    {
        FMODManager.instance.OneShot("UISelect", transform.position);
    }
    
    public void PlayUIClick()
    {
        FMODManager.instance.OneShot("UIClick", transform.position);
    }
}
