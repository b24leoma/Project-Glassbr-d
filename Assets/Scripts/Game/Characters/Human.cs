using UnityEngine;

namespace Game
{
    public class Human : Entity
    {
        [SerializeField] protected GameObject movingIcon;
        [SerializeField] protected GameObject attackingIcon;
        void Awake()
        {
           isHuman = true;
           string[] identity = NameGenerator.GenerateIdentity(true);
           Name = identity[0];
           Age = identity[1];
           Description = identity[2];
        }
        
        public override void MoveDistance(int distance)
        {
            moveDistanceRemaining -= distance;
            movingIcon.SetActive(moveDistanceRemaining == 0);
        }

        public override void SetAttacking(bool attacking)
        {
            hasAttacked = attacking;
            attackingIcon.SetActive(attacking);
            if (attacking) PlayAttack();
                
        }
    }
}
