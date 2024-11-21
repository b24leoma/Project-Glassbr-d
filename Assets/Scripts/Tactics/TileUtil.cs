using UnityEngine;
using UnityEngine.Tilemaps;


public class TileUtil : MonoBehaviour
{
    public static Vector3 TileCenter(Tilemap tilemap, Vector3Int tilePosition)
    {
        
        Vector3 worldPosition = tilemap.CellToWorld(tilePosition);

        
        Vector3 tileSize = tilemap.cellSize;
        worldPosition.x += tileSize.x / 2;
        worldPosition.y += tileSize.y / 2;

        return worldPosition;
    }

}
