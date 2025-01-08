using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game
{
    public class Entity : MonoBehaviour
    {
        public enum EntityType
        {
            HumanSpearman,
            HumanArcher,
            HumanTank, 
            DemonSwordsman,
            DemonTank,
            DemonArcher
        };

        public EntityType Type;
        private Animator animator;
        protected SpriteRenderer _sprite;
        private ParticleSystem _particle;
        public GameObject arrowPrefab;
        public string Name { get; protected set; }
        public string Age { get; protected set; }
        public bool IsMale { get; protected set; }
        public string Description { get; protected set; }
        public int MaxHealth;
        public int CurrentHealth { get; protected set; }
        public int MinDamage;
        public int MaxDamage;
        public int MoveRange;
        public int AttackRange;
        public int CritChance;
        public int MissChance;
        public bool IsMelee { get; protected set; }
        public bool Flipped { get; protected set; }
        
        

        [SerializeField] private Slider healthBar;
        [HideInInspector] public bool isHuman;
        [HideInInspector] public int moveDistanceRemaining;
        [HideInInspector] public bool hasAttacked;
        [HideInInspector] public Sprite face;


        public void AssignIdentity(Identity id)
        {
            Name = id.name;
            IsMale = id.isMale;
            Age = id.age;
            Description = id.description;
            face = id.face;
        }

        public Vector2Int Position
        {
            get => new((int)(transform.position.x + 0.5f), (int)(transform.position.y + 0.5f));
            set => transform.position = new Vector3(value.x - 0.5f, value.y - 0.5f, value.y + 5);
        }

        public void TakeDamage(float damage)
        {
            if (!healthBar.IsActive()) healthBar.gameObject.SetActive(true);
            CurrentHealth -= (int)damage;
            if (CurrentHealth < 0) CurrentHealth = 0;
            healthBar.value = CurrentHealth;
            transform.DOShakeScale(0.2f, 0.3f).SetLoops(1, LoopType.Yoyo);
            transform.DOShakeRotation(0.2f, 0.3f).SetLoops(1, LoopType.Yoyo);
            transform.DOShakePosition(0.1f, 0.1f, 1).SetLoops(1, LoopType.Yoyo);
            _sprite.DOColor(Color.red, 0.1f).SetLoops(2, LoopType.Yoyo);
            
            SFX.DMG(Type, transform.position);
            if (_particle != null) _particle.Play();
        }


        void Start()
        {
            animator = GetComponent<Animator>();
            _sprite = GetComponent<SpriteRenderer>();
            _particle=GetComponent<ParticleSystem>();
            CurrentHealth = MaxHealth;
            IsMelee = AttackRange <= 1;
            moveDistanceRemaining = MoveRange;
            healthBar.maxValue = MaxHealth;
            healthBar.value = MaxHealth;
        }

        public void MoveToTile(Vector2Int pos)
        {
            Position = pos;
        }


        public void MoveDistance(int distance)
        {
            moveDistanceRemaining -= distance;
        }

        public virtual void SetAttacking(bool attacking)
        {
            hasAttacked = attacking;
            if (attacking) PlayAttack();
            else _sprite.color = Color.white;
        }
        
        public void Flip()
        {
            Flipped = !Flipped;
            _sprite.flipX = !_sprite.flipX;
        }

        public void Kill()
        {
            SFX.DEATH(Type, transform.position);
            transform.DOShakeRotation(0.4f, 1080f);
            transform.DOMoveY(1.5f, 0.2f).SetLoops(2, LoopType.Yoyo).SetEase(Ease.InOutBounce).OnComplete(() =>
            {
                transform.DOShakePosition(0.3f, new Vector3(0.2f, 0.2f, 0));
            });


            transform.DOShakeScale(0.6f, 0.3f).OnComplete(() =>
            {
               
                _sprite.DOFade(0, 0.3f).OnComplete(() =>
                {
                    DOTween.Kill(transform);
                    DOTween.Kill(_sprite);
                    Destroy(gameObject);
                });
            });
        }

        protected void PlayAttack()
        {
            animator.SetTrigger("Attack");
            SFX.ATK(Type, transform.position);
        }
    }
}