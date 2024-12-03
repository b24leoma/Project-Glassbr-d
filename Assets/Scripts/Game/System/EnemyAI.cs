using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private BattleController battleController;

        public void StartTurn()
        {
            EndTurn();
        }

        void EndTurn()
        {
            battleController.EndTurn();
        }
    }
}
