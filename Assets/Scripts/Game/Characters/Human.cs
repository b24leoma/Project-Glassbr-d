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
        
        public override void SetMoving(bool moving)
        {
            hasMoved = moving;
            movingIcon.SetActive(moving);
        }

        public override void SetAttacking(bool attacking)
        {
            hasAttacked = attacking;
            attackingIcon.SetActive(attacking);
            if (attacking) PlayAttack();
                
        }
    }
}
