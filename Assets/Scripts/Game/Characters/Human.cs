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
           Name = NameGenerator.GenerateName(true);
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
