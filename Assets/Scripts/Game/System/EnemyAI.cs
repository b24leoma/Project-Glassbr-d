using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private BattleController battleController;
        [SerializeField] private GridSystem gridSystem;

        public void StartTurn()
        {
            List<Vector2Int> humans = new List<Vector2Int>();
            List<Vector2Int> demons = new List<Vector2Int>();
            foreach (KeyValuePair<Vector2Int, Tile> tile in gridSystem.GetAllTiles())
            {
                if (tile.Value.linkedEntity != null)
                {
                    if (tile.Value.linkedEntity.isHuman)
                    {
                        humans.Add(tile.Key);
                    }
                    else demons.Add(tile.Key);
                }
            }

            for (int i = 0; i < demons.Count; i++)
            {
                int distance = 999;
                Vector2 target = Vector2.zero;
                for (int j = 0; j < humans.Count; j++)
                {
                    if (gridSystem.GetGridDistance(demons[i], humans[j]) < distance) target = humans[j];
                }

                Vector2Int demonCurrentPos = demons[i];
                for (int k = 0; k < gridSystem.GetTile(new Vector2Int((int)demons[i].x, (int)demons[i].y)).linkedEntity.MoveRange; k++)
                {
                    if (target.y > demonCurrentPos.y && TileIsFree(demons[i] + Vector2.up))
                    {
                        demonCurrentPos += Vector2Int.up;
                    }
                    else if (target.y < demonCurrentPos.y && TileIsFree(demons[i] + Vector2.down))
                    {
                        demonCurrentPos += Vector2Int.down;
                    }   
                    else if (target.y == demonCurrentPos.y)
                    {
                        if (target.x > demonCurrentPos.x && TileIsFree(demons[i] + Vector2.right))
                        {
                            demonCurrentPos += Vector2Int.right;
                        }
                        else if (target.x < demonCurrentPos.x && TileIsFree(demons[i] + Vector2.left))
                        {
                            demonCurrentPos += Vector2Int.left;
                        }  
                    }   
                }
                Debug.Log("DAFSDF");
                gridSystem.MoveUnit(demons[i], demonCurrentPos);
            }
            
            EndTurn();
        }

        void EndTurn()
        {
            battleController.EndTurn();
        }

        bool TileIsFree(Vector2 pos)
        {
            return gridSystem.GetTile(new Vector2Int((int)pos.x, (int)pos.y)).linkedEntity == null ||
                   gridSystem.GetTile(new Vector2Int((int)pos.x, (int)pos.y)).walkable;
        }
    }
}
