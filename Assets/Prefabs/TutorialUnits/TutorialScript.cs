using System;
using Game;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    private bool hasAttacked;
    private bool hasMoved;
    private Human humanScript;

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
        if (humanScript)
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
        
        Debug.Log("PERFORMANCE :D");
    }

    private void Attacked()
    {
        if (0 == tutorialManager.attackCounter)
        {
            tutorialManager.onFirstAttack.Invoke();
            tutorialManager.attackCounter++;
        }

        tutorialManager.onAttack.Invoke();
        tutorialManager.attackCounter++;
    }

    private void Moved()
    {
        if (0 == tutorialManager.moveCounter)
        {
            tutorialManager.onFirstMove.Invoke();
            tutorialManager.moveCounter++;
        }

        tutorialManager.onMove.Invoke();
        tutorialManager.moveCounter++;
    }
}
