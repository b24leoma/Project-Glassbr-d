using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridSystem : MonoBehaviour
{
    private Tilemap tilemap;
    [SerializeField] private TileBase[] solidTiles;
    private Dictionary<Vector2, Tile> tiles;
    private Vector3 size;

    private void Start()
    {
        tilemap = GetComponent<Tilemap>();
        tiles = new Dictionary<Vector2, Tile>();
        size = GetComponent<Tilemap>().size;

        size.x = size.x / 2 - 0.5f;
        size.y = size.y / 2 - 0.5f;
        for (int i = -(int)size.x; i <= size.x; i++)
        {
            for (int j = -(int)size.y; j <= size.y; j++)
            {
                tiles[new Vector2(i, j)] = new Tile();
                if (solidTiles.Contains(tilemap.GetTile(new Vector3Int(i - 1, j - 1, 0))))
                {
                    tiles[new Vector2(i,j)].walkable = false;
                }
               else  tilemap.SetTileFlags(new Vector3Int(i - 1, j - 1, 0), TileFlags.None);
                
                
            }
        }
    }
    public void HighlightSquaresInRange(Entity entity, float range, Color color)
    {
        if(entity == null) return;
        for (int i = -(int)size.x; i <= size.x; i++)
        {
            for (int j = -(int)size.y; j <= size.y; j++)
            {
                if (Vector2.Distance(entity.Position - new Vector2(-0.5f, -0.5f), new Vector2(i, j)) <= range && tiles[new Vector2(i,j)].walkable)
                {
                    tilemap.SetColor(new Vector3Int(i - 1, j - 1, 0), color);
                }
            }
        }
    }

    public void MoveUnit(Vector2 currentPos, Vector2 newPos)
    {
        if (tiles[currentPos] == null || tiles[currentPos].linkedEntity == null) return;
        Entity e = tiles[currentPos].linkedEntity;
        tiles[currentPos].linkedEntity = null;
        ConnectToTile(newPos, e);
        Debug.Log("Moved");
    }

    public void ConnectToTile(Vector2 tile, Entity entity)
    {
        tiles[tile].linkedEntity = entity;
        entity.MoveToTile(tile);
    }

    public Tile GetTile(Vector2Int position)
    {
        return tiles[position];
    }


    public void SetColor(Vector3Int pos, Color color)
    {
        tilemap.SetColor(pos, color);
    }
}

public class Tile
{
    public Entity linkedEntity = null;
    public bool walkable = true;
}


