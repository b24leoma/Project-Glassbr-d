using System.Collections.Generic;
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
            Debug.Log("jaboba");
        }

        if (characters != null)
            foreach (var entity in characters)
            {
                tutorialScript = entity.GetComponent<TutorialScript>();
                tutorialScript.CheckingUnits();
            }
    }

    public void DebugLogger1()
    {
        Debug.Log("WOWIE I DID THE THING");
    }

    public void DebugLogger2()
    {
        Debug.Log("WOWIE I DID THE OTHER THING");
    }



    public void TryNextSentence(int eventnumber)
    {
       if (sentencenumber == eventnumber)
       {
           dialogueManager.UnpauseDialogue();
       }
       
    }

}