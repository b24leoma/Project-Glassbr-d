using System.Collections.Generic;
using System.Collections;
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
            StartCoroutine(DoTheMagic());
        }
        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator DoTheMagic()
        {
            yield return new WaitForSeconds(1);
            demons = gridSystem.demons;
            for (int i = 0; i < demons.Count; i++)
            {
                int distance = 999;
                Vector2Int demonCurrentPos = demons[i];
                Vector2Int target = gridSystem.humans[Random.Range(0, gridSystem.humans.Count - 1)];
                distance = gridSystem.GetGridDistance(demons[i], target);
                bool hasMoved = false;
                bool hasAttacked = false;
                int range = gridSystem.GetTile(demons[i]).linkedEntity.MoveRange;
                
                
                //Attack before move
                if ( gridSystem.GetGridDistance(target, demonCurrentPos) <=
                     gridSystem.GetTile(demons[i]).linkedEntity.AttackRange)
                {
                    gridSystem.GetTile(demonCurrentPos).linkedEntity.SetAttacking(true);
                    battleController.Attack(gridSystem.GetTile(demonCurrentPos).linkedEntity,
                        gridSystem.GetTile(target).linkedEntity);
                    hasAttacked = true;
                    yield return new WaitForSeconds(1);
                    if (gridSystem.humans.Count == 0) yield break;
                    if (gridSystem.GetTile(target).linkedEntity == null) // FIND NEW TARGET
                    {
                        target = gridSystem.humans[Random.Range(0, gridSystem.humans.Count - 1)];
                        distance = gridSystem.GetGridDistance(demons[i], target);

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
                        gridSystem.GetTile(demons[i]).linkedEntity.SetMoving(true);
                        hasMoved = true;
                        yield return new WaitForSeconds(0.5f);

                        if (!hasAttacked && gridSystem.GetGridDistance(demonCurrentPos, target) <=
                            gridSystem.GetTile(demonCurrentPos).linkedEntity.AttackRange)
                        {
                            battleController.Attack(gridSystem.GetTile(demonCurrentPos).linkedEntity,
                                gridSystem.GetTile(target).linkedEntity);
                            gridSystem.GetTile(demonCurrentPos).linkedEntity.SetAttacking(true);
                            if (gridSystem.humans.Count == 0) yield break;
                            yield return new WaitForSeconds(0.5f);
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
                }
                yield return new WaitForSeconds(0.25f);
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
