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
    public int delayBell;
    
    
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


     private void Start()
     {
         delayBell = 0;
     }


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
        sentenceIsStopped = true;
    }

    private IEnumerator TypeSentence(string sentence)
    {
        dialogueField.text = "";
        if (sentence.Length == 0)
        {
            NameSystem ns = FindObjectOfType<NameSystem>();
            if (ns != null)
            {
                if (ns.dead.Count == 0)
                {
                    sentence = "No casualties yet.";
                }
                else
                {
                    if (ns.dead.Count < 3) sentence = "The people I have killed:\n";
                    else if (ns.dead.Count < 5) sentence = "The people we have lost in the war:\n";
                    else sentence = "Sacrifices needed for the war:\n";
                    foreach (string namn in ns.dead)
                    {
                        sentence += $"{namn}\n";


                        if (delayBell == 0)
                        {
                            DOVirtual.DelayedCall(1.5f, () => StartCoroutine(Bells()));
                        }
                        else
                        {
                            StartCoroutine(Bells());
                        }
                        
                        
                        delayBell++;

                    }
                }
            }
            else
            {
                sentence = "Could not load character list :/";
            }
            currentSentenceString = sentence;
        }
        for (int i = 0; i < sentence.Length; i++)
        {

            while (typingPaused)
            {
                yield return null;
            }
            
            
            
            dialogueField.text += sentence[i];
            if (sentence[i] == '<')
            {
                while (sentence[i] != '>')
                {
                    i++;
                    dialogueField.text += sentence[i];
                }
            }
            yield return new WaitForSeconds(0.04f);
        }

        if (stopAfterSentence.Contains(currentSentence))
        {
            onDialoguePause.Invoke();
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
            if (sentenceIsStopped && currentSentence >= 0 && canStopSentence && stopAfterSentence.Contains(currentSentence) || alwaysStop)
            {

                sentenceIsStopped = true;
                alwaysStop = false;
                Debug.Log(
                    "Dialogue paused... please start it with UnpauseDialogue in DialogueManagerScript (Works with Unity Events)");
                onDialoguePause.Invoke();
                //return;
            }
            
            StopCoroutine(_currentCoroutine);
            _currentCoroutine = null;
            dialogueField.text = currentSentenceString;

            typingPaused = true;
        }
    }



    private void DelayUnpause()
    {
        typingPaused = false;
    }


    IEnumerator Bells()
    {
        yield return new WaitForSeconds(delayBell + 1.5f);
        FMODManager.instance.OneShot("Bell");
    }
    
    
    
}
