using UnityEngine;

namespace Game
{
    public class Demon : Entity
    {
        [SerializeField] private GameObject AttackingImage;
        [HideInInspector] public Human target;
        private SpriteRenderer attackImg;
        
        void Awake()
        {
            isHuman = false;
            attackImg = AttackingImage.GetComponent<SpriteRenderer>();
            Flipped = true;
        }

        public void DisplayAttackingImage(bool display, Color color)
        {
            AttackingImage.SetActive(display);
            attackImg.color = color;
        }
    }
}
