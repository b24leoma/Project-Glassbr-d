using System.Collections.Generic;
using System.Collections;
using UnityEngine;

namespace Game
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private BattleController battleController;
        [SerializeField] private GridSystem gridSystem;
        public void StartTurn()
        {
            StartCoroutine(DoTheMagic());
        }
        // ReSharper disable Unity.PerformanceAnalysis
        private IEnumerator DoTheMagic()
        {
            yield return new WaitForSeconds(1);
            for (int i = 0; i < gridSystem.demons.Count; i++)
            {
                Demon demon = gridSystem.GetTile(gridSystem.demons[i]).linkedEntity as Demon;
                Vector2Int demonCurrentPos = demon.Position;
                GetClosestTarget(demon);
                bool hasMoved = false;
                bool hasAttacked = false;
                int range = demon.MoveRange;
                
                
                //Attack before move
                if (gridSystem.GetGridDistance(demon.target.Position, demonCurrentPos) <=
                     demon.AttackRange)
                {
                    gridSystem.GetTile(demonCurrentPos).linkedEntity.SetAttacking(true);
                    battleController.Attack(gridSystem.GetTile(demonCurrentPos).linkedEntity, demon.target);
                    hasAttacked = true;
                    yield return new WaitForSeconds(1);
                    if (gridSystem.humans.Count == 0) yield break;
                    GetClosestTarget(demon);
                }
                Debug.Log(demon.target.Position);
                for (int moves = 0; moves < range;)
                {
                    bool couldMove = false;

                    if (moves < range && demon.target.Position.x > demonCurrentPos.x && TileIsFree(demonCurrentPos + Vector2Int.right) && gridSystem.GetGridDistance(demon.target.Position, demonCurrentPos) >
                        demon.AttackRange)
                    {
                        demonCurrentPos += Vector2Int.right;
                        moves++;
                        couldMove = true;
                    }
                    else if (moves < range && TileIsFree(demonCurrentPos + Vector2Int.left) && gridSystem.GetGridDistance(demon.target.Position, demonCurrentPos) >
                             demon.AttackRange)
                    {
                        demonCurrentPos += Vector2Int.left;
                        moves++;
                        couldMove = true;
                    }
                    else if (moves < range && TileIsFree(demonCurrentPos + Vector2Int.right) && gridSystem.GetGridDistance(demon.target.Position, demonCurrentPos) >
                             demon.AttackRange)
                    {
                        demonCurrentPos += Vector2Int.right;
                        moves++;
                        couldMove = true;
                    }
                    
                    
                    if (moves < range && demon.target.Position.y > demonCurrentPos.y && TileIsFree( demonCurrentPos + Vector2Int.up) && gridSystem.GetGridDistance(demon.target.Position, demonCurrentPos) >
                        demon.AttackRange)
                    {
                        demonCurrentPos += Vector2Int.up;
                        moves++;
                        couldMove = true;
                    }
                    else if (moves < range && TileIsFree(demonCurrentPos + Vector2Int.down) && gridSystem.GetGridDistance(demon.target.Position, demonCurrentPos) >
                             demon.AttackRange)
                    {
                        moves++;
                        demonCurrentPos += Vector2Int.down;
                        couldMove = true;
                    }
                    else if (moves < range && TileIsFree(demonCurrentPos + Vector2Int.up) && gridSystem.GetGridDistance(demon.target.Position, demonCurrentPos) >
                             demon.AttackRange)
                    {
                        moves++;
                        demonCurrentPos += Vector2Int.up;
                        couldMove = true;
                    }
                    
                    if(!couldMove || gridSystem.GetGridDistance(demon.target.Position, demonCurrentPos) <= demon.AttackRange) moves = 100;

                    if (moves >= demon.MoveRange && !hasMoved)
                    {
                        battleController.Move(demon.Position, demonCurrentPos);
                        demon.SetMoving(true);
                        hasMoved = true;
                        yield return new WaitForSeconds(0.5f);
                        if (!hasAttacked && gridSystem.GetGridDistance(demonCurrentPos, demon.target.Position) <=
                            gridSystem.GetTile(demonCurrentPos).linkedEntity.AttackRange)
                        {
                            battleController.Attack(gridSystem.GetTile(demonCurrentPos).linkedEntity,
                                demon.target);
                            gridSystem.GetTile(demonCurrentPos).linkedEntity.SetAttacking(true);
                            if (gridSystem.humans.Count == 0) yield break;
                            yield return new WaitForSeconds(0.5f);
                        }
                    }
                }
                yield return new WaitForSeconds(0.25f);
            }
            EndTurn();
        }

        void EndTurn()
        {
            foreach (Vector2Int demonPos in gridSystem.demons)
            {
                Demon demon = gridSystem.GetTile(demonPos).linkedEntity as Demon;
                demon.SetAttacking(false);
                demon.SetMoving(false);
            }
            
            battleController.EndTurn();
        }

        bool TileIsFree(Vector2Int pos)
        {
            if (gridSystem.TileIsInBounds(pos))
            {
                return gridSystem.GetTile(pos).linkedEntity == null &&
                       gridSystem.GetTile(pos).walkable;
            }

            return false;
        }

        void GetRandomTarget(Demon demon)
        {
            demon.target = gridSystem.GetTile(gridSystem.humans[Random.Range(0, gridSystem.humans.Count - 1)]).linkedEntity as Human;
        }
        void GetClosestTarget(Demon demon)
        {
            int distance = 999;
            for (int i = 0; i < gridSystem.humans.Count; i++)
            {
                if (gridSystem.GetGridDistance(demon.Position, gridSystem.humans[i]) < distance)
                {
                    distance = gridSystem.GetGridDistance(demon.Position, gridSystem.humans[i]);
                    demon.target = gridSystem.GetTile(gridSystem.humans[i]).linkedEntity as Human;
                }
            }
        }
        void GetFurthestTarget(Demon demon)
        {
            int distance = 0;
            for (int i = 0; i < gridSystem.humans.Count; i++)
            {
                if (gridSystem.GetGridDistance(demon.Position, gridSystem.humans[i]) > distance)
                {
                    distance = gridSystem.GetGridDistance(demon.Position, gridSystem.humans[i]);
                    demon.target = gridSystem.GetTile(gridSystem.humans[i]).linkedEntity as Human;
                }
            }
        }
    }
}
