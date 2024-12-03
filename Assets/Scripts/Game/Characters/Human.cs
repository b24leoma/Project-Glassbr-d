using UnityEngine;

namespace Game
{
    public class Human : Entity
    {
        [SerializeField] private GameObject movingIcon;
        [SerializeField] private GameObject attackingIcon;
        void Awake()
        {
           isHuman = true;
           Name = NameGenerator.GenerateName(true);
        }
        
        public override void SetMoving(bool Moving)
        {
            hasMoved = Moving;
            movingIcon.SetActive(Moving);
        }

        public override void SetAttacking(bool attacking)
        {
            hasAttacked = attacking;
            attackingIcon.SetActive(attacking);
            if (attacking) PlayAttack();
        }
    }
}
