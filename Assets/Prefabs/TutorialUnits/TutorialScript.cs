using System.Net;
using Game;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.Events;

public class TutorialScript : MonoBehaviour
{
    private bool hasAttacked;
    private bool hasMoved;
    private Human humanScript;
    public UnityEvent onMove;
    public UnityEvent onAttack;
    public UnityEvent onFirstAttack;
    public UnityEvent onFirstMove;
    private int attackCount;
    private int moveCount;
    [SerializeField] private TutorialManager tutorialManager;
     private GameObject gameManager;

    private void OnEnable()
    {
       
        
        if (tutorialManager == null)
        {
            gameManager = GameObject.Find("GameManager");
            tutorialManager = gameManager.GetComponent<TutorialManager>();
        }
        
        if (tutorialManager)
        {
            var hasHumanScript = gameObject.GetComponent<Human>();
            humanScript = hasHumanScript;
            if (humanScript)
            {
                StateChecker();
            }
        }
        
        
    }

    private void StateChecker()
    {
        
        if (humanScript.hasAttacked)
        {
            Attacked();
        }

        if (humanScript.hasMoved)
        {
            Moved();
        }
    }

    private void Attacked()
    {
        if (0 == attackCount)
        {
            onFirstAttack.Invoke();
            attackCount++;
        }
        
        onAttack.Invoke();
        attackCount++;


    }
    
    private void Moved()
    {
        if (0 == moveCount)
        {
            onFirstMove.Invoke();
            moveCount++;
        }
        
        onMove.Invoke();
        moveCount++;


    }
   
}
