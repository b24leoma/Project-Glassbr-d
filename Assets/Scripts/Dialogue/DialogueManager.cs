using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
   // public TMP_Text nameField;
    public TMP_Text dialogueField;
    
    private Queue<string> sentences = new Queue<string>();
    public Animator animator;
    public UnityEvent whenComplete;

    
    
    
    
    // För dialogue kontroll
     [SerializeField] private List<int> stopAfterSentence = new List<int>();
     public bool canStopSentence=true;
     public int currentSentence;
     public bool sentenceIsStopped;
     public bool alwaysStop;
     public bool typingPaused;
     private Coroutine _currentCoroutine;
     [SerializeField] private UnityEvent onDialoguePause;
     [SerializeField] private UnityEvent onDialogueUnpause;
    

     


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
        if (sentenceIsStopped) return;
        if (currentSentence >= 0 && canStopSentence && stopAfterSentence.Contains(currentSentence) || alwaysStop)
        {

            sentenceIsStopped = true;
            alwaysStop = false;
            Debug.Log(
                "Dialogue paused... please start it with UnpauseDialogue in DialogueManagerScript (Works with Unity Events)");
            onDialoguePause.Invoke();
            return;
        }


        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();

        
        currentSentence++;
        canStopSentence = true;
          
        StopAllCoroutines();
       _currentCoroutine = StartCoroutine(TypeSentence(sentence));
       
       if (currentSentence >= 0 && canStopSentence && stopAfterSentence.Contains(currentSentence) || alwaysStop)
       {

           sentenceIsStopped = true;
           alwaysStop = false;
           Debug.Log(
               "Dialogue paused... please start it with UnpauseDialogue in DialogueManagerScript (Works with Unity Events)");
           onDialoguePause.Invoke();
       }
    }

    private IEnumerator TypeSentence(string sentence)
    {
        dialogueField.text = "";
        foreach (char letter in sentence)
        {

            while (typingPaused)
            {
                yield return null;
            }
            
            
            
            dialogueField.text += letter;
            yield return new WaitForSeconds(0.04f);
        }
        
        sentenceIsStopped = false;
    }

    private void EndDialogue()
    {
        StopAllCoroutines();
        dialogueField.text = "";
        whenComplete?.Invoke();
        Debug.Log("Ending conversation");
    }



    
    public void UnpauseDialogue()
    {
        canStopSentence = false;
        sentenceIsStopped = false;
        alwaysStop = false;
        DOVirtual.DelayedCall(1.5f, DelayUnpause);
        onDialogueUnpause.Invoke();
        DisplayNextSentence();
    }

    public void PauseDialogue()
    {
        canStopSentence = true;
        sentenceIsStopped = true;
        alwaysStop = true;
        typingPaused = true;
        
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine); // Stoppa meningen maybe
        }
    }



    private void DelayUnpause()
    {
        
        typingPaused = false;
    }
}
