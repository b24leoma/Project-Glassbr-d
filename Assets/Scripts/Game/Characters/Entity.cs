using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

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
        protected Animator animator;
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
        
        protected void PlayAttack()
        {
            Debug.Log(Type);
            switch (Type)
            {
                case EntityType.HumanSpearman:
                    animator.SetTrigger("Attack");
                    break;
                case EntityType.HumanArcher:
                    break;
                case EntityType.DemonSwordsman:
                    break;
                case EntityType.DemonTank:
                    break;
            }
        }
    }

    public static class NameGenerator
    {
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
        
        public static readonly List<string> _human = new List<string>
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
        public static readonly List<string> _demon = new List<string>
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
}