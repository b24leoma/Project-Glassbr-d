using System;
using System.Collections.Generic;
using DG.Tweening;
using Game;
using UnityEngine;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour
{
    public int introduceMoveOnSentence;
    public int introduceAttackOnSentence;
    public int introduceEndTurnOnSentence;
    public int introduceBushOnSentence;
    public UnityEvent onMove;
    public UnityEvent onAttack;
    public UnityEvent onBush;
    public UnityEvent onEndTurn;
   
    private GridSystem gridSystem;
    [SerializeField] private int sentencenumber;
    [SerializeField] DialogueManager dialogueManager;

    private void Start()
    {
        sentencenumber = dialogueManager.currentSentence;
        gridSystem = FindObjectOfType<GridSystem>();
    }

    public bool TutorialAttackTime()
    {
        return dialogueManager.currentSentence == introduceAttackOnSentence;
    }
    
    public bool TutorialMoveTime()
    {
        return dialogueManager.currentSentence == introduceMoveOnSentence;
    }

    public bool TutorialBushTime()
    {
        return dialogueManager.currentSentence == introduceBushOnSentence;
    }
    public bool TutorialEndTurnTime()
    {
        return dialogueManager.currentSentence == introduceEndTurnOnSentence;
    }
    
    public void Moving()
    {
        if (!enabled) return;
        TryNextSentence(introduceMoveOnSentence);
        onMove?.Invoke();
    }
    public void Attacking()
    {
        if (!enabled) return;
        TryNextSentence(introduceAttackOnSentence);
        onAttack?.Invoke();
    }
    public void OnEndTurn()
    {
        if (!enabled) return;
        TryNextSentence(introduceEndTurnOnSentence);
        onEndTurn?.Invoke();
    }
    public void Bushing()
    {
        if (!enabled) return;
        TryNextSentence(introduceBushOnSentence);
        onBush?.Invoke();
    }


    private void TryNextSentence(int eventnumber)
    {
        sentencenumber = dialogueManager.currentSentence;
        if (sentencenumber == eventnumber)
        {
            dialogueManager.UnpauseDialogue();
        }
    }

}