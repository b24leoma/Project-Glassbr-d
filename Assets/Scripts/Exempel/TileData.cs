
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WilhelmsJank
{
  //Baserat på https://www.youtube.com/watch?v=XIqtZnqutGg&t=4s
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
}
