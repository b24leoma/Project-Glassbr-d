using UnityEngine;


[CreateAssetMenu(menuName = "Audio/Units/DemonTemplate")]
public class DemonFMODRefTemplate : FMODRefData
{

    [Tooltip("Syntax example: ATK_Demon.Sword")]
    [Header("DEMON")]  
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
