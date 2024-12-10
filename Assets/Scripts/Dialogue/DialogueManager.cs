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
     public bool canStopSentence=true;
     public int currentSentence;
     public bool sentenceIsStopped;
     public bool alwaysStop;
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
        canStopSentence = false;
        sentenceIsStopped = false;
        alwaysStop = false;
        onDialogueUnpause.Invoke();
        DisplayNextSentence();
    }

    public void PauseDialogue()
    {
        canStopSentence = true;
        sentenceIsStopped = true;
        alwaysStop = true;
        
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine); // Stoppa meningen maybe
        }
    }
    
}
