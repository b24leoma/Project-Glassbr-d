using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game
{
    public class GridSystem : MonoBehaviour
    {
        private Tilemap tilemap;
        [SerializeField] private Tile[] CustomTiles;
        private Dictionary<Vector2, Tile> tiles;
        private Vector3 size;
        private void OnEnable()
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
                    tilemap.SetTileFlags(new Vector3Int(i - 1, j - 1, 0), TileFlags.None);
                    
                    //Assign Custom Effects
                    int index = Array.IndexOf(CustomTiles, tilemap.GetTile(new Vector3Int(i - 1, j - 1, 0)));
                    if (index > -1)
                    {
                        tiles[new Vector2(i, j)].walkable = CustomTiles[index].walkable;
                        tiles[new Vector2(i, j)].ArcherRangeIncrease = CustomTiles[index].ArcherRangeIncrease;
                        tiles[new Vector2(i, j)].DamageReductionPercent = CustomTiles[index].DamageReductionPercent;
                    }
                }
            }
        }
        public void HighlightSquaresInRange(Vector2 pos, float range, Color color)
        {
            for (int i = -(int)size.x; i <= size.x; i++)
            {
                for (int j = -(int)size.y; j <= size.y; j++)
                {
                    if (GetGridDistance(pos - new Vector2(-0.5f, -0.5f), new Vector2(i, j)) <= range && tiles[new Vector2(i,j)].walkable)
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
        }

        public void ConnectToTile(Vector2 pos, Entity entity)
        {
            tiles[pos].linkedEntity = entity;
            entity.MoveToTile(pos);
        }

        public Tile GetTile(Vector2Int position)
        {
            return tiles[position];
        }

        public int GetGridDistance(Vector2 from, Vector2 to)
        {
            return (int)Mathf.Abs(from.x - to.x) + (int)Mathf.Abs(from.y - to.y);
        }


        public void SetColor(Vector3Int pos, Color color)
        {
            tilemap.SetColor(pos, color);
        }

        public void ClearGrid()
        {
            for (int i = -(int)size.x; i <= size.x; i++)
            {
                for (int j = -(int)size.y; j <= size.y; j++)
                {
                    Tile t = tiles[new Vector2(i, j)];
                    if (t.linkedEntity != null)
                    {
                        tiles[new Vector2(i, j)].linkedEntity = null;
                        Destroy(t.linkedEntity.gameObject);
                    }
                }
            }
        }
    }

    [Serializable] public class Tile
    {
        [HideInInspector] public Entity linkedEntity;
        public TileBase tile;
        public bool walkable = true;
        
        public int ArcherRangeIncrease = 0;
        public int DamageReductionPercent = 0;
    }
}