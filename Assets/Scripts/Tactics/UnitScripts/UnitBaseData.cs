using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class UnitBaseData : ScriptableObject
{
 public GameObject unitPrefab;
 public string unitType;
 public string unitFaction;
 public string unitName;
 

 public int baseActionPoints;
 
 [Range(0f, 100f)]
 public float baseEvasion ;     // Base change of 100% Accurate attack hitting you
 [Range(0f, 100f)]
 public float baseAccuracy ;    // Base change of attack hitting at 1 range
 [Range(0, 100)]
 public int baseRange;  //base tile range
 [Range(0, 100)]
 public int baseHealth;
 [Range(0f, 100f)]
 public float baseDamage;
 [Range(0f, 100f)]
 public float basePhysicalResistance; //resistance to physical i.e. swords
 [Range(0f, 100f)]
 public float baseMagicalResistance; //resistance to magic i.e. fireballs
 [Range(0, 100)] 
 public int baseDebuffResistance; //resitance to debuffs  1 = 1 turn less of debuffs

 //add more if we add poison, fire e.t.c , gäller all resistance
 // Ändra och clampa values beroende på behov i framtiden


}
