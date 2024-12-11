using UnityEngine;
using UnityEngine.Events;

public class TryNextPage : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
    public UnityEvent nextPage;

    public void TryPageFlip()
    {
        if (dialogueManager == null)
        {
            Debug.LogError("Dialogue manager is null.");
        }
        
        
        if (dialogueManager.typingPaused)
        {
            Debug.Log("Dialogue typing is false.");
        }
        else
        {
            nextPage.Invoke();
        }
        
        
            
        
        
        
    }
    
    
    
}
