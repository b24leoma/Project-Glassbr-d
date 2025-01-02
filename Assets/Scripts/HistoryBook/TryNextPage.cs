using UnityEngine;
using UnityEngine.Events;

public class TryNextPage : MonoBehaviour
{
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private UnityEvent SkipText;
    [SerializeField] private UnityEvent FlipPage;

    public void TryPageFlip()
    {
        if (dialogueManager == null)
        {
            Debug.LogError("Dialogue manager is null.");
        }


        if (dialogueManager.typingPaused)
        {
            if (dialogueManager.canFlip)
            {
                dialogueManager.canFlip = false;
                dialogueManager.dialogueField.text = "";
                dialogueManager.FlipPage?.Invoke();
            }
        }
        else
        {
            SkipText.Invoke();
        }
        
        
            
        
        
        
    }
    
    
    
}
