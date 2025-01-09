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
     public bool canFlip;
     private Coroutine _currentCoroutine;
     [SerializeField] private UnityEvent onDialoguePause;
     [SerializeField] private UnityEvent onDialogueUnpause;
     public UnityEvent FlipPage;
     private string currentSentenceString;
    

     


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

        currentSentenceString = sentences.Dequeue();

        canFlip = true;        
        currentSentence++;
        canStopSentence = true;

        if (_currentCoroutine != null) StopCoroutine(_currentCoroutine);
        dialogueField.text = "";
        _currentCoroutine = StartCoroutine(TypeSentence(currentSentenceString));
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
        if (sentence.Length == 0)
        {
            NameSystem ns = FindObjectOfType<NameSystem>();
            if (ns != null)
            {
                sentence = $"Deaths: {ns.dead.Count}\n\n";
                if (ns.dead.Count == 0)
                {
                    sentence += "No deaths yet!";
                }
                else
                {
                    foreach (string name in ns.dead)
                    {
                        sentence += $"{name}\n";
                    }
                }
            }
            else
            {
                sentence = "Could not load character list :/";
            }
            currentSentenceString = sentence;
        }
        foreach (char letter in sentence)
        {

            while (typingPaused)
            {
                yield return null;
            }
            
            
            
            dialogueField.text += letter;
            yield return new WaitForSeconds(0.04f);
        }

        typingPaused = true;
        sentenceIsStopped = false;
        _currentCoroutine = null;
    }

    private void EndDialogue()
    {
        if (_currentCoroutine != null) StopCoroutine(_currentCoroutine);
        dialogueField.text = "";
        whenComplete?.Invoke();
        Debug.Log("Ending conversation");
    }




    public void UnpauseDialogue()
    {
        canFlip = false;
        canStopSentence = false;
        sentenceIsStopped = false;
        alwaysStop = false;
        typingPaused = false;
        DOVirtual.DelayedCall(1f, DelayUnpause);
        onDialogueUnpause.Invoke();
        DisplayNextSentence();
    }

    public void PauseDialogue()
    {
        if (_currentCoroutine == null)
        {
            if (canFlip)
            {
                canFlip = false;
                dialogueField.text = "";
                FlipPage?.Invoke();
            }
        }
        else
        {
        
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
            dialogueField.text = currentSentenceString;
            Debug.Log("Skipped text");
            typingPaused = true;
        }
    }



    private void DelayUnpause()
    {
        typingPaused = false;
    }
}
