
using UnityEngine;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour
{
    public int moveCounter;
   public int attackCounter;
   public UnityEvent onMove;
   public UnityEvent onAttack;
   public UnityEvent onFirstAttack;
   public UnityEvent onFirstMove;


   public void totalStateChecker()
   {
       Debug.Log("IT WÖRKS");
   }
}
