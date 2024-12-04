using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private BattleController battleController;
        [SerializeField] private GridSystem gridSystem;

        private List<Vector2Int> demons;

        public void StartTurn()
        {
            demons = gridSystem.demons;
            for (int i = 0; i < demons.Count; i++)
            {
                int distance = 999;
                Vector2Int target = Vector2Int.zero;
                for (int j = 0; j < gridSystem.humans.Count; j++)
                {
                    if (gridSystem.GetGridDistance(gridSystem.humans[j], demons[i]) < distance)
                    {
                        distance = gridSystem.GetGridDistance(gridSystem.humans[j], demons[i]);
                        target = gridSystem.humans[j];
                    }
                }
                Vector2Int demonCurrentPos = demons[i];
                bool hasMoved = false;
                bool hasAttacked = false;
                int range = gridSystem.GetTile(demons[i]).linkedEntity.MoveRange;
                
                
                //Attack before move
                if ( gridSystem.GetGridDistance(target, demonCurrentPos) <=
                    range)
                {
                    battleController.Attack(gridSystem.GetTile(demonCurrentPos).linkedEntity,
                        gridSystem.GetTile(target).linkedEntity);
                    hasAttacked = true;
                    if (gridSystem.humans.Count > 0)
                    {
                        for (int j = 0; j < gridSystem.humans.Count; j++)
                        {
                            if (gridSystem.GetGridDistance(gridSystem.humans[j], demons[i]) < distance)
                            {
                                distance = gridSystem.GetGridDistance(gridSystem.humans[j], demons[i]);
                                target = gridSystem.humans[j];
                            }
                        }
                    }
                }
                
                for (int moves = 0; moves < range;)
                {
                    bool couldMove = false;

                    if (moves < range && target.x > demonCurrentPos.x && TileIsFree(demonCurrentPos + Vector2.right))
                    {
                        demonCurrentPos += Vector2Int.right;
                        moves++;
                        couldMove = true;
                    }
                    else if (moves < range && target.x < demonCurrentPos.x && TileIsFree(demonCurrentPos + Vector2.left))
                    {
                        demonCurrentPos += Vector2Int.left;
                        moves++;
                        couldMove = true;
                    }  
                    
                    
                    if (moves < range && target.y > demonCurrentPos.y && TileIsFree( demonCurrentPos + Vector2.up))
                    {
                        demonCurrentPos += Vector2Int.up;
                        moves++;
                        couldMove = true;
                    }
                    else if (moves < range && target.y < demonCurrentPos.y && TileIsFree(demonCurrentPos + Vector2.down))
                    {
                        moves++;
                        demonCurrentPos += Vector2Int.down;
                        couldMove = true;
                    }
                    
                    if(!couldMove) moves = 100;

                    if (moves >= gridSystem.GetTile(demons[i]).linkedEntity.MoveRange && !hasMoved)
                    {
                        battleController.Move(demons[i], demonCurrentPos);
                        hasMoved = true;
                        
                    }
                }
            }
            
            EndTurn();
        }

        void EndTurn()
        {
            battleController.EndTurn();
        }

        bool TileIsFree(Vector2 pos)
        {
            return gridSystem.GetTile(new Vector2Int((int)pos.x, (int)pos.y)).linkedEntity == null &&
                   gridSystem.GetTile(new Vector2Int((int)pos.x, (int)pos.y)).walkable;
        }
    }
}
