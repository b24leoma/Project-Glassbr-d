using System.Collections.Generic;
using DG.Tweening;
using Game;
using UnityEngine;
using UnityEngine.Events;

public class TutorialManager : MonoBehaviour
{
    public int moveCounter;
    public int attackCounter;
    public int bushCounter;
    public UnityEvent onMove;
    public UnityEvent onAttack;
    public UnityEvent onFirstAttack;
    public UnityEvent onFirstMove;
    public UnityEvent onFirstBush;
    public UnityEvent onBush;
    public UnityEvent delayedEvent;
   [Range(0, 10),SerializeField] private float delayEventTimer;

    private BattleController battleController;
    private List<Entity> allUnits;
    private GridSystem gridSystem;
    private PlayerAttackSystem playerAttackSystem;

    private TutorialScript tutorialScript;
    [SerializeField] private int sentencenumber;
    [SerializeField] DialogueManager dialogueManager;

    private void Start()
    {
        if (battleController == null)
        {
            battleController = GetComponent<BattleController>();
        }

        if (playerAttackSystem == null)
        {
            playerAttackSystem = GetComponent<PlayerAttackSystem>();
        }

        if (playerAttackSystem != null)
        {
            playerAttackSystem.SetPaused(true);
        }

        sentencenumber = dialogueManager.currentSentence;

    }

    public void TotalStateChecker()
    {
        sentencenumber = dialogueManager.currentSentence;
        var characters = battleController.GetCharacters();

        if (characters == null)
        {
            Debug.Log("NO UNITS ON FIELD");
        }

        if (characters != null)
            foreach (var entity in characters)
            {
                if (entity != null)
                {
                    tutorialScript = entity.GetComponent<TutorialScript>();
                    if (tutorialScript != null)
                    {
                        tutorialScript.CheckingUnits();
                    }
                }
                
            }
    }

    


    public void TryNextSentence(int eventnumber)
    {
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