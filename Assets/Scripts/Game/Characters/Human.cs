using UnityEngine;

namespace Game
{
    public class Human : Entity
    {
        [SerializeField] protected GameObject movingIcon;
        [SerializeField] protected GameObject attackingIcon;
        private LightFader _lightFader;
        void Awake()
        {
           isHuman = true;
           string[] identity = NameGenerator.GenerateIdentity(true);
           Name = identity[0];
           Age = identity[1];
           Description = identity[2];
           _lightFader = GetComponentInChildren<LightFader>();
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


        public void NightLightToggle(bool toNight)
        {
            switch (toNight)
            {
                case true:
                    _lightFader.FadeInLight();
                    break;
                case false:
                    _lightFader.FadeOutLight();
                    break;
            }
        }
            
    }
}
