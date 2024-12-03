using System;
using System.Collections.Generic;
using UnityEngine;
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
        [HideInInspector] public bool hasQueuedMovement;
        [HideInInspector] public bool hasQueuedAttack;
    
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
            hasQueuedMovement = false;
            hasQueuedMovement = false;
            CurrentHealth = MaxHealth;
            IsMelee = AttackRange <= 1;
        }

        public void MoveToTile(Vector2Int pos)
        {
            Position = pos;
        }


        public virtual void MoveQueued(bool hasQueued)
        {
            hasQueuedMovement = hasQueued;
        }

        public virtual void AttackQueued(bool hasQueued)
        {
            hasQueuedAttack = hasQueued;
            if (hasQueued) PlayAttack();
                
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
                string name = NameGenerator._human[Random.Range(1, NameGenerator._human.Count)];
                NameGenerator._human.Remove(name);
                return name;
                
            }
            else
            {
                string name = NameGenerator._demon[Random.Range(1, NameGenerator._demon.Count)];
                NameGenerator._demon.Remove(name);
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