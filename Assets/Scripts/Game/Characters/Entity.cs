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
        
        [SerializeField] protected GameObject movingIcon;
        [SerializeField] protected GameObject attackingIcon;
        
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


        public void SetMoving(bool moving)
        {
            hasMoved = moving;
            movingIcon.SetActive(moving);
        }

        public void SetAttacking(bool attacking)
        {
            hasAttacked = attacking;
            attackingIcon.SetActive(attacking);
            if (attacking) PlayAttack();
                
        }

        public void Kill()
        {
            Destroy(gameObject);
        }
        
        private void PlayAttack()
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
                "Luster",
                "Larceny",
                "Execution",
                "Devours",
                "Picker",
                "Haughty",
                "Ire",
                "Rapacity",
                "Envyn",
                "Idlek",
                "Immodera",
                "Solipsi",
                "Treach",
                "Wiolat",
                "Malick",
                "Lia",
                "Torment",
                "Pherver",
                "Cultest",
                "Preshor",
                "Bob",
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