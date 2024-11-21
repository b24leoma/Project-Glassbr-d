using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class TileData : ScriptableObject
{
    public TileBase[] tiles;

    public bool canWalk;
    public bool[] hasModifier;

    public float modifierRange;
    public float modifierEvasion;
    public float modifierAccuracy;
    public float modifierMovement;
}

