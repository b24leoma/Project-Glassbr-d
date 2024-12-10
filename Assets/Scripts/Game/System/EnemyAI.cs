using System.Collections;
using System.Linq;
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
                bool hasAttacked = false;


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
                
                Vector2Int[] path = gridSystem.PathFindValidPath(demon.Position, demon.target.Position, demon.MoveRange);
                StartCoroutine(battleController.Move(path, !hasAttacked && gridSystem.GetGridDistance(demon.Position, demon.target.Position) <= 1, demon.target));
                
                yield return new WaitForSeconds(0.5f);
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
