
using UnityEngine;
using UnityEngine.Events;

public class FlippedPage : MonoBehaviour
{
    public UnityEvent onFlippedPage;
    public UnityEvent onFlippingPage;
   
   public void HasFlippedPage()
   {
       onFlippedPage.Invoke();
   }

   public void FlippingPage()
   {
       onFlippingPage.Invoke();
   }
}
