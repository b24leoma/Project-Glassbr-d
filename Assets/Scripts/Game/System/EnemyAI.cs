using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private BattleController battleController;
        private Dictionary<Entity, Entity> attackList;

        private void Start()
        {
            attackList = new Dictionary<Entity, Entity>();
        }

        public void StartTurn()
        {
            EndTurn();
        }

        void EndTurn()
        {
            battleController.EndTurn(attackList);
        }
    }
}
