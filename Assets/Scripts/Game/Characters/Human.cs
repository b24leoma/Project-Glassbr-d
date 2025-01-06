using DG.Tweening;
using UnityEngine;

namespace Game
{
    public class Human : Entity
    {
        private LightFader _lightFader;
        private Color _hasAttackedColor;
        public bool isMoving;
        
        void Awake()
        {
           isHuman = true;
           _lightFader = GetComponentInChildren<LightFader>();
           if (_lightFader == null) Debug.LogWarning("No LightFader found");
            
           _hasAttackedColor= new Color (0.6f, 0.6f, 0.6f);
        }
        public override void SetAttacking(bool attacking)
        {
            hasAttacked = attacking;
            if (attacking)
            {
                PlayAttack();
                _sprite.DOColor(_hasAttackedColor,  0.2f).SetEase(Ease.InSine);      
               
              _lightFader.PauseTween();
                
            }
            
            if (!attacking)
            {
                _sprite.DOColor(Color.white,  0.2f).SetEase(Ease.InSine);

                if (_lightFader)
                {
                    _lightFader.LightSync();
                }


            }

        }

        public void SetMoving(bool moving)
        {
            isMoving = moving;
        }


        public void NightLightToggle(bool toNight)
        {
            switch (toNight )
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
