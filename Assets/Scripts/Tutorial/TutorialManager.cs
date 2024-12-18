using System;
using System.Collections.Generic;
using DG.Tweening;
using Game;
using UnityEngine;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour
{
    public UnityEvent onMove;
    public UnityEvent onAttack;
    public UnityEvent onBush;
    public UnityEvent onEndTurn;
    public UnityEvent delayedEvent;
   [Range(0, 10),SerializeField] private float delayEventTimer;
   
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
        return (dialogueManager.currentSentence == 4);
    }

    public void Attacking()
    {
        onAttack?.Invoke();
    }

    public bool TutorialMoveTime()
    {
        return dialogueManager.currentSentence == 2;
    }

    public bool TutorialBushTime(Vector2Int endPos)
    {
        return dialogueManager.currentSentence == 6;
    }

    public void Bushing()
    {
        onBush?.Invoke();
    }

    public void Moving()
    {
        onMove?.Invoke();
    }

    public void OnEndTurn()
    {
        onEndTurn?.Invoke();
    }


    public void TryNextSentence(int eventnumber)
    {
        sentencenumber = dialogueManager.currentSentence;
        if (sentencenumber == eventnumber)
        {
            dialogueManager.UnpauseDialogue();
        }
    }

    public void EventDelayer()
    {
        //Use this as a middleman to delay an event... probably shitty way to do it, but it works.
        DOVirtual.DelayedCall(delayEventTimer, delayedEvent.Invoke);

    }

}