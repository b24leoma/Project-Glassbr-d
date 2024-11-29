using UnityEngine;

namespace Game
{
    public class Human : Entity
    {
        [SerializeField] private GameObject movingIcon;
        [SerializeField] private GameObject attackingIcon;
        void Start()
        {
            MoveRange = 3;
            AttackRange = 1;
            MaxHealth = 100;
            CurrentHealth = MaxHealth;
            Damage = 20; 
            GenerateName();
            IsMelee = AttackRange <= 1;
        }
        
        public override void MoveQueued(bool hasQueued)
        {
            hasQueuedMovement = hasQueued;
            movingIcon.SetActive(hasQueued);
        }

        public override void AttackQueued(bool hasQueued)
        {
            hasQueuedAttack = hasQueued;
            attackingIcon.SetActive(hasQueued);
        }
    }
}
