using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DialogManager : MonoBehaviour
{
    private static readonly int IsOpen = Animator.StringToHash("isOpen");
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    private Queue<string> sentences;
    public GameObject uiDialogueObject;
    public UnityEvent onDialogEnd;

    public bool isDialogActive;
    public Animator animator;
    public bool IsDialogActive
    {
        get => isDialogActive;
        private set
        {
            isDialogActive = value;
            animator.SetBool(IsOpen, isDialogActive);
            Time.timeScale = isDialogActive ? 0f : 1f;
        }

    }
    

    void Start()
    {
        sentences = new Queue<string>();

        IsDialogActive = false;
        if (uiDialogueObject != null)
        {
            uiDialogueObject.SetActive(true);
        }
    }

    public void StartDialog(DialogStrings dialog, UnityEvent dialogEndEvent)
    {
        
        //Listener skiten är AI genererat för jag blev galen resten är Brackeys + jag :)
        onDialogEnd.RemoveAllListeners();
        onDialogEnd.AddListener(dialogEndEvent.Invoke);
        
        
        
        IsDialogActive = true;



        nameText.text = dialog.name;
        sentences.Clear();

        foreach (string sentence in dialog.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialog();
            return;
        }

        string sentence = sentences.Dequeue();
        StopAllCoroutines();      
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence)
        {
            dialogueText.text += letter;
            yield return new WaitForSecondsRealtime(0.04f);
        }
    }
    
     
    void EndDialog()
    {
        IsDialogActive = false;
        onDialogEnd?.Invoke();
    }
}
