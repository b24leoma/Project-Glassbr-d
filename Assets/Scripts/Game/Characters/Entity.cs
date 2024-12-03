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
        public Animator animator;
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
    
        public Vector2 Position
        {
            get => transform.position;
            private set => transform.position = value;
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

        public void MoveToTile(Vector2 pos)
        {
            Position = pos + new Vector2(-0.5f, -0.5f);
        }


        public virtual void MoveQueued(bool hasQueued)
        {
            hasQueuedMovement = hasQueued;
        }
        public virtual void AttackQueued(bool hasQueued)
        {
            hasQueuedAttack = hasQueued;
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