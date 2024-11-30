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
