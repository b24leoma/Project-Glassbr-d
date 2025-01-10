using UnityEngine;
using UnityEngine.Events;

public class FadeOutOnStart : MonoBehaviour
{
    public UnityEvent start;
    void Start()
    {
        start.Invoke();
    }

    
}
