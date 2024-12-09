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

        public void HighlightMoveTiles(Vector2Int start, float range, Color color, bool highlightEnemy = false)
        {
            HashSet<Vector2Int> used = new HashSet<Vector2Int>();
            used.Add(start);
            for (int i = 0; i < range; i++)
            {
                Vector2Int[] check = used.ToArray();
                foreach (Vector2Int pos in check)
                {
                        if (TileIsFree(pos + Vector2Int.up) && !used.Contains(pos + Vector2Int.up))
                            used.Add(pos + Vector2Int.up);

                        if (TileIsFree(pos + Vector2Int.right) && !used.Contains(pos + Vector2Int.right))
                            used.Add(pos + Vector2Int.right);

                        if (TileIsFree(pos + Vector2Int.down) && !used.Contains(pos + Vector2Int.down))
                            used.Add(pos + Vector2Int.down);

                        if (TileIsFree(pos + Vector2Int.left) && !used.Contains(pos + Vector2Int.left))
                            used.Add(pos + Vector2Int.left);
                }
            }

            foreach (Vector2Int pos in used)
            {
                if (!highlightEnemy && GetTile(pos).linkedEntity != null && !GetTile(pos).linkedEntity.isHuman) continue;
                SetColor(pos, color);
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

        private bool TileIsInBounds(Vector2Int pos)
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
            if (tiles[pos].walkable) tilemap.SetColor(new Vector3Int(pos.x - 1, pos.y - 1, 0), color);;
        }

        public void SetHidingSpotColor(Vector2Int pos, Color color)
        {
            ObstacleTilemap.SetColor(new Vector3Int(pos.x - 1, pos.y - 1, 0), color);
        }
        
        public Vector2Int[] PathFindValidPath(Vector2Int start, Vector2Int end, int range)
        {
            HashSet<Vector2Int> used = new HashSet<Vector2Int>();
            HashSet<Vector2Int>[] paths = new HashSet<Vector2Int>[range + 2];
            used.Add(start);
            paths[0] = new HashSet<Vector2Int>() { start };
            for (int i = 0; i < range; i++)
            {
                paths[i+1] = new HashSet<Vector2Int>();
                Vector2Int[] validHelper = paths[i].ToArray();
                foreach (Vector2Int pos in validHelper)
                {
                    if (TileIsFree(pos + Vector2Int.up) && !used.Contains(pos + Vector2Int.up))
                    {
                        used.Add(pos + Vector2Int.up);
                        paths[i+1].Add(pos + Vector2Int.up);
                    }

                    if (TileIsFree(pos + Vector2Int.right) && !used.Contains(pos + Vector2Int.right))
                    {
                        used.Add(pos + Vector2Int.right);
                        paths[i+1].Add(pos + Vector2Int.right);
                    }

                    if (TileIsFree(pos + Vector2Int.down) && !used.Contains(pos + Vector2Int.down))
                    {
                        used.Add(pos + Vector2Int.down);
                        paths[i+1].Add(pos + Vector2Int.down);
                    }

                    if (TileIsFree(pos + Vector2Int.left) && !used.Contains(pos + Vector2Int.left))
                    {
                        used.Add(pos + Vector2Int.left);
                        paths[i+1].Add(pos + Vector2Int.left);
                    }
                }
            }
            
            //Picks best tile
            Vector2Int optimalTile = start;
            float selectRange = 999;
            foreach (Vector2Int pos in used)
            {
                float distance = Vector2Int.Distance(pos, end);
                if (distance < selectRange)
                {
                    optimalTile = pos;
                    selectRange = distance;
                }
            }
            
            
            //Creates path
            List<Vector2Int> traced = new List<Vector2Int>(){optimalTile};
            for (int i = 0; i < range + 1; i++)
            {
                if (paths[i].Contains(optimalTile))
                {
                    Vector2Int currentTile = optimalTile;
                    for (int j = i; j > 0; j--)
                    {
                        if (paths[j-1].Contains(currentTile + Vector2Int.up))
                        {
                            currentTile += Vector2Int.up;
                            traced.Add(currentTile);
                        }
                        else if (paths[j-1].Contains(currentTile + Vector2Int.right))
                        {
                            currentTile += Vector2Int.right;
                            traced.Add(currentTile);
                        }
                        else if (paths[j-1].Contains(currentTile + Vector2Int.down))
                        {
                            currentTile += Vector2Int.down;
                            traced.Add(currentTile);
                        }
                        else if (paths[j-1].Contains(currentTile + Vector2Int.left))
                        {
                            currentTile += Vector2Int.left;
                            traced.Add(currentTile);
                        }
                    }
                }
            }
            traced.Reverse();
            return traced.ToArray();
        }
        
        bool TileIsFree(Vector2Int pos)
        {
            if (TileIsInBounds(pos))
            {
                return GetTile(pos).linkedEntity == null &&
                       GetTile(pos).walkable;
            }

            return false;
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