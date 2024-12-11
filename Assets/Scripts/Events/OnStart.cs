using UnityEngine;
using UnityEngine.Events;

public class OnStart : MonoBehaviour
{
    [SerializeField] private UnityEvent onStart;
    void Start()
    {
      onStart.Invoke();  
    }

   // I am very lazy
}
