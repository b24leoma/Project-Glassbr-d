using UnityEngine;

public enum Team { Team1, Team2 }
public class UnitInGameData : MonoBehaviour
{
    [SerializeField]  private UnitBaseData unitBaseData;

    [SerializeField] public Team unitTeam;
    [SerializeField] public float currentActionPoints;
    
    [SerializeField] private bool hasAttackedThisTurn;
    [SerializeField] private int isStunned; // 0= false 1=true for one turn  2 = true for two turns etc.
    [SerializeField] private bool isDead;
    [SerializeField] private bool gender; //0 = male   1 = female

    
 
    
    
    public string unitPersonalName;
    private string _factionName;

    void Start()
    {
        _factionName = unitBaseData.unitFaction;
        NameMachine nameMachine = FindObjectOfType<NameMachine>();
        unitPersonalName = nameMachine.TryGenerateName(gender, _factionName);
    }

}
