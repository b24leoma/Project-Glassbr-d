using UnityEngine;


[CreateAssetMenu(menuName = "Audio/Units/HumanTemplate")]
public class HumanFMODRefTemplate : FMODRefData
{

    [Tooltip("Syntax example: ATK_Human.Spear")]
    [Header("HUMAN")]  
    public AudioEvent [] miscEvents;
 
 
  
  [Tooltip("Syntax: ATK_UnitNickname")]
  [Header("Attack")]
  public AudioEvent[] attackEvents;
  
  [Tooltip("Syntax: DMG_UnitNickname")]
  [Header("DMG")]
  public AudioEvent[] damageEvents;
  
  
  [Tooltip("Syntax: DEATH_UnitNickname")]
  [Header("Death")]
  public AudioEvent[] deathEvents;
  
  [Tooltip("Syntax: MOVE_UnitNickname")]
  [Header ("Move")]
  public AudioEvent[] moveEvents;
  
  

}
