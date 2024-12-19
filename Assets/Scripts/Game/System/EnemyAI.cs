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
            yield return new WaitForSeconds(1.5f);
            foreach (Vector2Int p in gridSystem.humans)
            {
                Entity e = gridSystem.GetTile(p).linkedEntity;
                e.SetAttacking(false);
                e.moveDistanceRemaining = e.MoveRange;
                e.MoveDistance(0);
            }
            for (int i = 0; i < gridSystem.demons.Count; i++)
            {
                yield return new WaitForSeconds(1f);
                if (gridSystem.GetTile(gridSystem.demons[i]).linkedEntity is Demon demon)
                {
                    Vector2Int demonCurrentPos = demon.Position;
                    GetClosestTarget(demon);

                    //Attack before move
                    if (demon.target.CurrentHealth > 0 && gridSystem.GetGridDistance(demon.target.Position, demonCurrentPos) <=
                        demon.AttackRange)
                    {
                        gridSystem.GetTile(demonCurrentPos).linkedEntity.SetAttacking(true);
                        battleController.Attack(gridSystem.GetTile(demonCurrentPos).linkedEntity, demon.target);
                        yield return new WaitForSeconds(1);
                        if (gridSystem.humans.Count == 0) yield break;
                        continue;
                    }
                    
                    //Move
                    if (demon.target.CurrentHealth > 0)
                    {
                        Vector2Int[] path =
                            gridSystem.PathFindValidPath(demon.Position, demon.target.Position, demon.MoveRange);
                        StartCoroutine(battleController.Move(path, true, demon.target));
                        yield return new WaitForSeconds(2f);
                    }

                    //Attack After move
                    GetClosestTarget(demon);
                    if (demon.target.CurrentHealth > 0 && gridSystem.GetGridDistance(demon.target.Position, demonCurrentPos) <=
                        demon.AttackRange)
                    {
                        gridSystem.GetTile(demonCurrentPos).linkedEntity.SetAttacking(true);
                        battleController.Attack(gridSystem.GetTile(demonCurrentPos).linkedEntity, demon.target);
                        yield return new WaitForSeconds(1);
                        if (gridSystem.humans.Count == 0) yield break;
                    }
                }
            }

            EndTurn();
        }

        void EndTurn()
        {
            foreach (Vector2Int demonPos in gridSystem.demons)
            {
                if (gridSystem.GetTile(demonPos).linkedEntity is Demon demon)
                {
                    demon.SetAttacking(false);
                    demon.moveDistanceRemaining = demon.MoveRange;
                }
            }
            if (gridSystem.humans.Count != 0) battleController.EndTurn();
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
