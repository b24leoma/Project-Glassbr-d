using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class DialogueManager : MonoBehaviour
{
   // public TMP_Text nameField;
    public TMP_Text dialogueField;
    
    private Queue<string> sentences = new Queue<string>();
    public Animator animator;
    public UnityEvent WhenComplete;

    
    
    
    
    // För dialogue kontroll
     [SerializeField] private List<int> stopAfterSentence = new List<int>();
     public bool stopSentence=true;
     [SerializeField] int currentSentence;
     private bool StartedDialogue;
    

     


     public void StartDialogue(Dialogue dialogue)
     {
         StartedDialogue = true;
         currentSentence = 0;
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {

        
        if (currentSentence > 0 && stopSentence && !stopAfterSentence.Contains(currentSentence) )
        {
            
            
                Debug.Log("Dialogue paused... please start it with UnpauseDialogue in DialogueManagerScript (Works with Unity Events)");
                return;
            

            
        }
        
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();

        
        currentSentence++;
        stopSentence = true;
          
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        dialogueField.text = "";
        foreach (char letter in sentence)
        {
            dialogueField.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void EndDialogue()
    {
        StopAllCoroutines();
        
        //        animator.SetBool("isOpen", false);
        WhenComplete?.Invoke();
        Debug.Log("Ending conversation");
    }



    
    public void UnpauseDialogue()
    {
        stopSentence = false;
        DisplayNextSentence();
    }
}
