using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private BattleController battleController;
        [SerializeField] private GridSystem gridSystem;
        private List<Vector2Int> demonList;
        public void StartTurn()
        {
            StartCoroutine(DoTheMagic(false));
        }
        private IEnumerator DoTheMagic(bool skipDelay)
        {
            foreach (Vector2Int p in gridSystem.humans)
            {
                Entity e = gridSystem.GetTile(p).linkedEntity;
                e.SetAttacking(false);
                e.moveDistanceRemaining = e.MoveRange;
                e.MoveDistance(0);
            }

            demonList = gridSystem.demons;
            ShuffleDemonList();
            yield return new WaitForSeconds(1.5f);
            for (int i = 0; i < demonList.Count; i++)
            {
                if (gridSystem.GetTile(demonList[i]).linkedEntity is Demon demon)
                {
                    Vector2Int demonCurrentPos = demon.Position;
                    
                    //first round get random target, then closest
                    if (demon.target == null && gridSystem.humans.Count > 0) demon.target = gridSystem.GetTile(gridSystem.humans[Random.Range(1, gridSystem.humans.Count) - 1]).linkedEntity as Human;
                    else demon.target = GetClosestTarget(demon);

                    //Attack before move
                    if (demon.target != null && gridSystem.GetGridDistance(demon.target.Position, demonCurrentPos) <=
                        demon.AttackRange)
                    {
                        gridSystem.GetTile(demonCurrentPos).linkedEntity.SetAttacking(true);
                        battleController.Attack(gridSystem.GetTile(demonCurrentPos).linkedEntity, demon.target);
                        if(!skipDelay) yield return new WaitForSeconds(1f);
                        if (gridSystem.humans.Count == 0) yield break;
                        continue;
                    }
                    
                    //Move
                    if (demon.target != null && demon.target.CurrentHealth > 0)
                    {
                        
                        Vector2Int[] path =
                            gridSystem.PathFindValidPath(demon.Position, demon.target.Position, demon.MoveRange);
                        if(path.Length > 1 && !skipDelay) yield return new WaitForSeconds(1.5f);
                        yield return StartCoroutine(battleController.Move(path, true, demon.target));
                        if (!skipDelay) yield return new WaitForSeconds(0.5f);
                    }
                    
                    //Attack After move
                    if (demon.target != null && demon.target.CurrentHealth > 0 && gridSystem.GetGridDistance(demon.target.Position, demonCurrentPos) <=
                        demon.AttackRange)
                    {
                        gridSystem.GetTile(demonCurrentPos).linkedEntity.SetAttacking(true);
                        battleController.Attack(gridSystem.GetTile(demonCurrentPos).linkedEntity, demon.target);
                        if (gridSystem.humans.Count == 0) yield break;
                        GetClosestTarget(demon);
                        if(!skipDelay) yield return new WaitForSeconds(0.5f);
                    }

                    if (!demon.hasAttacked)
                    {
                        Human closest = GetClosestTarget(demon);
                        if (gridSystem.GetGridDistance(demon.Position, closest.Position) <=
                            demon.AttackRange)
                        {
                            demon.target = closest;
                           demon.SetAttacking(true);
                            battleController.Attack(demon, demon.target);
                            if (gridSystem.humans.Count == 0) yield break;
                            GetClosestTarget(demon);
                            if(!skipDelay) yield return new WaitForSeconds(0.5f);
                        }
                    }
                }
            }
            yield return new WaitForSeconds(0.5f);
            EndTurn();
        }

        void EndTurn()
        {
            foreach (Vector2Int demonPos in demonList)
            {
                if (gridSystem.GetTile(demonPos).linkedEntity is Demon demon)
                {
                    demon.SetAttacking(false);
                    demon.moveDistanceRemaining = demon.MoveRange;
                }
            }
            if (gridSystem.humans.Count != 0) battleController.EndTurn();
        }
        
        Human GetClosestTarget(Demon demon)
        {
            Human target = null;
            int distance = 999;
            for (int i = 0; i < gridSystem.humans.Count; i++)
            {
                if (gridSystem.GetGridDistance(demon.Position, gridSystem.humans[i]) < distance)
                {
                    distance = gridSystem.GetGridDistance(demon.Position, gridSystem.humans[i]);
                    target = gridSystem.GetTile(gridSystem.humans[i]).linkedEntity as Human;
                }
            }

            return target;
        }
        
        private void ShuffleDemonList()
        {
            System.Random rnd = new System.Random();
            int n = demonList.Count;  
            while (n > 1) {  
                n--;  
                int k = rnd.Next(n + 1);  
                Vector2Int value = demonList[k];  
                demonList[k] = demonList[n];  
                demonList[n] = value;
            }  
        }

        public void StopTurn()
        {
            StopAllCoroutines();
        }
    }
}
