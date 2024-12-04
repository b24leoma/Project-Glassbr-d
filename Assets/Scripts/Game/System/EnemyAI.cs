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
                for (int k = 0; k < gridSystem.GetTile(demons[i]).linkedEntity.MoveRange; k++)
                {
                    if (target.x > demonCurrentPos.x && TileIsFree(demonCurrentPos + Vector2.right))
                    {
                        demonCurrentPos += Vector2Int.right;
                    }
                    else if (target.x < demonCurrentPos.x && TileIsFree(demonCurrentPos + Vector2.left))
                    {
                        demonCurrentPos += Vector2Int.left;
                    }  
                    else if (target.y > demonCurrentPos.y && TileIsFree( demonCurrentPos + Vector2.up))
                    {
                        demonCurrentPos += Vector2Int.up;
                    }
                    else if (target.y < demonCurrentPos.y && TileIsFree(demonCurrentPos + Vector2.down))
                    {
                        demonCurrentPos += Vector2Int.down;
                    }
                }
                battleController.Move(demons[i], demonCurrentPos);
                if (gridSystem.GetGridDistance(target, demonCurrentPos) <=
                    gridSystem.GetTile(demonCurrentPos).linkedEntity.AttackRange)
                {
                    battleController.Attack(gridSystem.GetTile(demonCurrentPos).linkedEntity, gridSystem.GetTile(target).linkedEntity);
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
