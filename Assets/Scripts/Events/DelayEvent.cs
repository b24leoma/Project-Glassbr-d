using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class DelayEvent : MonoBehaviour
{
   
   [SerializeField] private UnityEvent delayedEvent;


   public void DelayThisEvent(float delayInSeconds)
   {
       DOVirtual.DelayedCall(delayInSeconds, DelayedEventInvoker);
   }
   
   
   private void DelayedEventInvoker()
   {
      delayedEvent.Invoke();
   }
}
