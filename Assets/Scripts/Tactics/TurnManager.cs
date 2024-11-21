using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class TurnManager : MonoBehaviour
{
  [SerializeField] int turn;
  [SerializeField] UnitMoveManager unitMoveManager;
  [SerializeField] private Team currentTeam;
  [SerializeField] private TextMeshProUGUI turnText;

  void Start()
  {
    turn = 1;
    currentTeam = Team.Team1;
    StartTurn();
    turnText.text = string.Format(" {0}", turn);
  }

  void StartTurn()
  {
    unitMoveManager.hasSelectedUnit = false;
    Debug.Log("Turn: " + turn + " - " + currentTeam);
    
    




  }

  public void EndTurn()
  {
    if (currentTeam == Team.Team2)
    {
      turn++;
    }

    if (turn < 10)
    {
      turnText.text = string.Format(" {0}", turn);
    }
    else
    {
      turnText.text = turn.ToString();
    }

    currentTeam = currentTeam == Team.Team1 ? Team.Team2 : Team.Team1;
    //AllowMove (currentTeam) ;      //Lägg till bool eller liknande i UnitInGameData så man inte kan röra andras units på sin turn.
    
    StartTurn();
  }
  
}
