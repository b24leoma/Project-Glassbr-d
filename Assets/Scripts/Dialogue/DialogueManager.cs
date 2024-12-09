using System.Collections;
using System.Collections.Generic;
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
    public UnityEvent whenComplete;

    
    
    
    
    // För dialogue kontroll
     [SerializeField] private List<int> stopAfterSentence = new List<int>();
     public bool stopSentence=true;
     [SerializeField] int currentSentence;
     public bool sentenceIsStopped;
    

     


     public void StartDialogue(Dialogue dialogue)
     {
         sentenceIsStopped=false;
         currentSentence = 0;
        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {

        if (stopAfterSentence.Count != 0)
        {
            if (currentSentence >= 0 && stopSentence && stopAfterSentence.Contains(currentSentence))
            {

                sentenceIsStopped = true;
                Debug.Log(
                    "Dialogue paused... please start it with UnpauseDialogue in DialogueManagerScript (Works with Unity Events)");
                return;



            }
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
        whenComplete?.Invoke();
        Debug.Log("Ending conversation");
    }



    
    public void UnpauseDialogue()
    {
        stopSentence = false;
        sentenceIsStopped = false;
        DisplayNextSentence();
    }
}
