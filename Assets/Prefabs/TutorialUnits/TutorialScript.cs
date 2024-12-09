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
        CheckingUnits();
    }


    public void CheckingUnits()
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
                if (!hasAttacked)
                {
                    Attacked();
                }
            }

            if (humanScript.hasMoved)
            {
                if (!hasMoved)
                {
                    Moved();
                }

            }
        }
        
    }

    private void Attacked()
    {
        hasAttacked = true;
        Debug.Log("I attacked!");
        if (0 == tutorialManager.attackCounter)
        {
            tutorialManager.onFirstAttack.Invoke();
            
        }
        else
        {
            tutorialManager.onAttack.Invoke();
           
        }
        tutorialManager.attackCounter++;
        
    }

    private void Moved()
    {
        Debug.Log("I moved!");
        hasMoved = true;
        
        if (0 == tutorialManager.moveCounter)
        {
            tutorialManager.onFirstMove.Invoke();
         
        }
        else
        {
            tutorialManager.onMove.Invoke();
            
        }
        tutorialManager.moveCounter++;
        
    }
    
    
}
