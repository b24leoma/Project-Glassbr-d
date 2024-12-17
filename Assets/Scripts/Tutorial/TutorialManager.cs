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

    public void Attacked(Entity e)
    {
        if (e is Human)
        {
            onAttack?.Invoke();
        }
    }

    public void Moved(Entity e, Vector2Int endPos)
    {
        if (e.isHuman)
        {
            onMove?.Invoke();
            if (gridSystem.GetTile(endPos).hidingSpot)
            {
                onBush?.Invoke();
            }
        }
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