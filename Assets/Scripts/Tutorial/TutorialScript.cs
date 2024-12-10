using System.Collections.Generic;
using DG.Tweening;
using Game;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    private bool hasAttacked;
    private bool hasMoved;
    private Human humanScript;

    [SerializeField] private TutorialManager tutorialManager;
    private GameObject gameManager;
    private GridSystem gridSystem;
    private List<Vector2Int> humanUnitPositions;
    private GameObject TILEMAP;

    private void OnEnable()
    {
        CheckingUnits();
    }

    public void CheckingUnits()
    {
        if (gameManager == null)
        {
            gameManager = GameObject.Find("GameManager");
        }

        if (tutorialManager == null)
        {
            tutorialManager = gameManager.GetComponent<TutorialManager>();
        }

        if (gridSystem == null)
        {
            TILEMAP = GameObject.Find("TILEMAP");
            gridSystem = TILEMAP.GetComponent<GridSystem>();
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
            humanScript = gameObject.GetComponent<Human>();
            
            
            if (humanScript.hasAttacked)
            {
                
                
                    Attacked();
                
            }

            if (humanScript.hasMoved)
            {
                
                
                    Moved();
                
            }
        }
    }

    private void BushChecker()
    {
        Debug.Log("Runing BushChecker");
        humanUnitPositions ??= new List<Vector2Int>();

        humanUnitPositions.Clear();
        humanUnitPositions.AddRange(gridSystem.humans);
        
        Debug.Log("HUMAN COUNT:" + humanUnitPositions.Count);

        foreach (var position in humanUnitPositions)
        {
            var tile = gridSystem.GetTile(position);
            if (tile == null)
            {
                Debug.Log("Tile could not be found");
            }
            if (tile?.hidingSpot != null)
            {
                    Debug.Log("Kom till BushCounter");
                    BushCounter();
                    return;
                
            }
            
            Debug.Log("No units on bush");
        }
    }

    private void BushCounter()
    {
        Debug.Log("BushCounter kördes");
        if (tutorialManager.bushCounter == 0)
        {
            tutorialManager.onFirstBush?.Invoke();
            Debug.Log("First Bush ;^)");
        }
        else
        {
            Debug.Log("Bush.... ;^)");
            tutorialManager.onBush?.Invoke();
        }

        tutorialManager.bushCounter++;
    }

    private void Attacked()
    {
        
        var updateHumanScript = gameObject.GetComponent<Human>();
        humanScript = updateHumanScript;
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
        var updateHumanScript = gameObject.GetComponent<Human>();
        humanScript = updateHumanScript;
        Debug.Log("I moved!");
        

        DOVirtual.DelayedCall(0.3f, BushChecker);

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