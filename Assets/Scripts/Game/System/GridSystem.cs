using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Game
{
    public class GridSystem : MonoBehaviour
    {
        [SerializeField] private Tilemap ObstacleTilemap;
        private Tilemap tilemap;
        [SerializeField] private Tile[] CustomTiles;
        private Dictionary<Vector2Int, Tile> tiles;
        private Vector3 size;
        [HideInInspector] public List<Vector2Int> demons;
        [HideInInspector] public List<Vector2Int> humans;
        private void OnEnable()
        {
            humans = new List<Vector2Int>();
            demons = new List<Vector2Int>();
            tilemap = GetComponent<Tilemap>();
            tiles = new Dictionary<Vector2Int, Tile>();   // Vector2Int? :thinking:
            size = tilemap.size;

            size.x = size.x / 2 - 0.5f;
            size.y = size.y / 2 - 0.5f;
            for (int i = -(int)size.x; i <= size.x + 1; i++)
            {
                for (int j = -(int)size.y; j <= size.y + 1; j++)
                {
                    tiles[new Vector2Int(i, j)] = new Tile();
                    tilemap.SetTileFlags(new Vector3Int(i-1, j-1, 0), TileFlags.None);
                    ObstacleTilemap.SetTileFlags(new Vector3Int(i-1, j-1, 0), TileFlags.None);
                    //Assign Custom Effects
                    if (ObstacleTilemap.GetTile(new Vector3Int(i-1, j-1, 0)) != null)
                    {
                        TileBase tile = ObstacleTilemap.GetTile(new Vector3Int(i - 1, j - 1, 0));
                        for (int k = 0; k < CustomTiles.Length; k++)
                        {
                            if (CustomTiles[k].tile == tile)
                            {
                                tiles[new Vector2Int(i, j)].walkable = CustomTiles[k].walkable;
                                tiles[new Vector2Int(i, j)].ArcherRangeIncrease = CustomTiles[k].ArcherRangeIncrease;
                                tiles[new Vector2Int(i, j)].DamageReductionPercent =
                                    CustomTiles[k].DamageReductionPercent;
                                tiles[new Vector2Int(i, j)].hidingSpot = CustomTiles[k].hidingSpot;
                            }
                        }
                    }
                }
            }
        }
        public void HighlightSquaresInRange(Vector2 pos, float range, Color color)
        {
            for (int i = -(int)size.x; i < size.x + 1; i++)     //Checkar alla tiles, borde gå att ta den valda tilen och titta enbart ett visst avstånd (range) omkring.
            {
                for (int j = -(int)size.y; j < size.y + 1; j++)
                {
                    if (GetGridDistance(pos, new Vector2(i, j)) <= range && tiles[new Vector2Int(i,j)].walkable)
                    {
                        tilemap.SetColor(new Vector3Int(i - 1, j - 1, 0), color);
                    }
                }
            }
        }

        public void MoveUnit(Vector2Int currentPos, Vector2Int newPos)
        {
            if (tiles[currentPos] == null || tiles[currentPos].linkedEntity == null) return;
            Entity e = tiles[currentPos].linkedEntity;
            tiles[currentPos].linkedEntity = null;
            if (e is Human) humans[humans.IndexOf(currentPos)] = newPos;
            else demons[demons.IndexOf(currentPos)] = newPos;
            ConnectToTile(newPos, e);
        }

        public void ConnectToTile(Vector2Int pos, Entity entity)
        {
            tiles[pos].linkedEntity = entity;
            entity.MoveToTile(pos);
        }

        public Tile GetTile(Vector2Int position)
        {
            return tiles[position];
        }

        public bool TileIsInBounds(Vector2Int pos)
        {
            return (pos.x > -size.x && pos.x < size.x && pos.y > -size.y && pos.y < size.y);
        }

        public int GetGridDistance(Vector2 from, Vector2 to)
        {
            return (int)Mathf.Abs(from.x - to.x) + (int)Mathf.Abs(from.y - to.y);
        }
        
        public Dictionary<Vector2Int, Tile> GetAllTiles()
        {
            return tiles;
        }


        public void SetColor(Vector2Int pos, Color color)
        {
            if (tiles[pos].walkable) tilemap.SetColor(new Vector3Int((int)pos.x - 1, (int)pos.y - 1, 0), color);;
        }

        public void SetHidingSpotColor(Vector2Int pos, Color color)
        {
            ObstacleTilemap.SetColor(new Vector3Int(pos.x-1, pos.y-1, 0), color);
        }

        public void ClearGrid()
        {
            for (int i = -(int)size.x; i <= size.x; i++)
            {
                for (int j = -(int)size.y; j <= size.y; j++)
                {
                    Tile t = tiles[new Vector2Int(i, j)];
                    if (t.linkedEntity != null)
                    {
                        tiles[new Vector2Int(i, j)].linkedEntity = null;
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
        public bool hidingSpot = false;
        
        public int ArcherRangeIncrease = 0;
        public int DamageReductionPercent = 0;
    }
}