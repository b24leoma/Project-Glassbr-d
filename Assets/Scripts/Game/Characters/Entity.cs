using System.Collections.Generic;
using UnityEngine;

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
        public string Name { get; protected set; }
        public int MaxHealth;
        public int CurrentHealth { get; protected set; }
        public int Damage;
        public int MoveRange;
        public int AttackRange;
        public bool IsMelee { get; protected set; }
        
        [HideInInspector] public bool isHuman;
        [HideInInspector] public bool hasMoved;
        [HideInInspector] public bool hasAttacked;
    
        public Vector2Int Position
        {
            get => new ((int)(transform.position.x+0.5f), (int)(transform.position.y+0.5f));
            set => transform.position = new Vector3(value.x -0.5f, value.y-0.5f);
        }

        public void TakeDamage(float damage)
        {
            CurrentHealth-= (int)damage;
        }
    
    
        void Start()
        {
            animator = GetComponent<Animator>();
            hasMoved = false;
            hasMoved = false;
            CurrentHealth = MaxHealth;
            IsMelee = AttackRange <= 1;
        }

        public void MoveToTile(Vector2Int pos)
        {
            Position = pos;
        }


        public virtual void SetMoving(bool moving)
        {
            hasMoved = moving;
        }

        public virtual void SetAttacking(bool attacking)
        {
            hasAttacked = attacking;
            if (attacking) PlayAttack();
                
        }

        public void Kill()
        {
            Destroy(gameObject);
        }
        
        protected void PlayAttack()
        {
            animator.SetTrigger("Attack");
        }
    }

    public static class NameGenerator
    {
        private static List<string> _human;
        private static List<string> _demon;
        public static void RepopulateList()
        {
            _human = new List<string>
            {
                "Sigrid",
                "Mathilda",
                "Brenna",
                "Col",
                "Nicole",
                "Borko",
                "Oliver",
                "Ivar",
                "Colette",
                "Hildeth",
                "Victoria",
                "Berenice",
                "Tilda",
                "Luthera",
                "Maude",
                "Esme",
                "Wilma",
                "Erika",
                "Annora",
                "Desislava",
                "Agatha",
                "Maxim",
                "Leopold",
                "Frost",
                "Ferdinand",
                "Baldwin",
                "Wilkie",
                "Harold",
                "Emmerich",
                "Dragan",
                "Bridget",
            };
            _demon = new List<string>
            {
                "Luster\nThe sins of lustful people",
                "Larceny\nThe sins of thieves",
                "Execution\nThe sins of executing",
                "Devours\nThe sins of the gluttonous",
                "Picker\nThe sins of multiple pickpocketing",
                "Haughty\nThe sins of the prideful",
                "Ire\nThe sins of acts in fury",
                "Rapacity\nThe sins of greedy individuals",
                "Envyn\nThe sins of envious people",
                "Idlek\nThe sins from inaction",
                "Immodera\nThe sins of owning in over excess",
                "Solipsi\nThe sins of the ego",
                "Treach\nThe sins of betrayals",
                "Wiolat\nThe sins of violations",
                "Malick\nThe sins from every holding malice",
                "Lia\nThe sins of every lie",
                "Torment\nThe sins from tortures",
                "Pherver\nThe sins from the depraved",
                "Cultest\nThe sins from violent worshipping",
                "Preshor\nThe sins of those who opress",
                "Bob\nThe sins of violent thefts",
            };
        }
        public static string GenerateName(bool isHuman)
        {
            if (isHuman)
            {
                string name = _human[Random.Range(1, _human.Count)];
                _human.Remove(name);
                return name;
                
            }
            else
            {
                string name = _demon[Random.Range(1, _demon.Count)];
                _demon.Remove(name);
                return name;
            }
        }
        
    }
}