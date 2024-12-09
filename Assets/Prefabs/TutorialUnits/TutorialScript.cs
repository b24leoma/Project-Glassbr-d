using System.Collections.Generic;
using Game;
using Unity.VisualScripting;
using UnityEngine;

public class TutorialScript : MonoBehaviour
{
    private bool hasAttacked;
    private bool hasMoved;
    private Human humanScript;

    [SerializeField] private TutorialManager tutorialManager;
    private GameObject gameManager;
    private GridSystem gridSystem;
    private List<Tile> bushUnits;
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


    private void BushChecker()
    {

        foreach (var thing  in gridSystem.GetAllTiles())
        {
            Debug.Log("POG");
            var tile = thing.Value;
            var isbush = tile.hidingSpot;
             
            //jag tror mina två hjärnceller gav upp i will solve this after some coffee : ) 






        }
           
       
       
            
        
    }

    private void BushAgain()
    {
        if (0 == tutorialManager.bushCounter)
        {
            tutorialManager.onFirstBush.Invoke();
            Debug.Log("Bush virgin");
        }
        else
        {
            Debug.Log("Bush Slut");
            tutorialManager.onBush.Invoke(); 
        }

        tutorialManager.bushCounter++;
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
        

       
        
            BushChecker();
        
        
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
