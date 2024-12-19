using System.Collections.Generic;
using System.IO;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game
{
    public class Entity : MonoBehaviour
    {
        public enum EntityType
        {
            HumanSpearman,
            HumanArcher,
            DemonSwordsman,
            DemonTank
        };

        public EntityType Type;
        private Animator animator;
        protected SpriteRenderer _sprite;
        private ParticleSystem _particle;
        public string Name { get; protected set; }
        public string Age { get; protected set; }
        public string Description { get; protected set; }
        public int MaxHealth;
        public int CurrentHealth { get; protected set; }
        public int Damage;
        public int MoveRange;
        public int AttackRange;
        public bool IsMelee { get; protected set; }

        [SerializeField] private Slider healthBar;
        [HideInInspector] public bool isHuman;
        [HideInInspector] public int moveDistanceRemaining;
        [HideInInspector] public bool hasAttacked;

        public Vector2Int Position
        {
            get => new((int)(transform.position.x + 0.5f), (int)(transform.position.y + 0.5f));
            set => transform.position = new Vector3(value.x - 0.5f, value.y - 0.5f);
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

        public void Kill()
        {
            SFX.DEATH(Type, transform.position);
            transform.DOShakeRotation(0.4f, 1080f);
            Debug.Log(Name + ": WEEEEEEEEEEEEEEEEEEEEEEEE");
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

    public static class NameGenerator
    {
        private static List<string[]> humanInfo;
        private static List<string[]> demonInfo;
        private static StreamReader reader;
        

        public static string[] GenerateIdentity(bool isHuman)
        {
            if (isHuman)
            {
                string[] name = humanInfo[Random.Range(1, humanInfo.Count)];
                humanInfo.Remove(name);
                return name;
            }
            else
            {
                string[] name = demonInfo[Random.Range(1, demonInfo.Count)];
                demonInfo.Remove(name);
                return name;
            }
        }

        public static void ReadFromFile(TextAsset human, TextAsset demons)
        {
            humanInfo = new List<string[]>();
            demonInfo = new List<string[]>();

            string[] info = human.text.Split('\n');
            if (info.Length % 3 != 0) Debug.Log("Human file has incorrect amount of lines");
            for (int i = 0; i < info.Length; i += 3)
            {
                humanInfo.Add(new[] { info[i], info[i + 1], info[i + 2] });
            }
            
            info = demons.text.Split('\n');
            if (info.Length % 2 != 0) Debug.Log("Demon file has incorrect amount of lines");
            for (int i = 0; i < info.Length; i+=2)
            {
                demonInfo.Add(new[] { info[i], info[i + 1]});
            }

        }
    }
}