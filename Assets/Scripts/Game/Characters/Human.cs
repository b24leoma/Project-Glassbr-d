using UnityEngine;

namespace Game
{
    public class Human : Entity
    {
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
        public override void SetAttacking(bool attacking)
        {
            hasAttacked = attacking;
            if (attacking)
            {
                PlayAttack();
                _sprite.color = new Color(0.6f, 0.6f, 0.6f);
            }
            else _sprite.color = Color.white;

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
