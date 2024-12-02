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

        public void TakeDamage(int damage)
        {
            CurrentHealth-= damage;
        }
    
    
        void Start()
        {
            animator = GetComponent<Animator>();
            hasQueuedMovement = false;
            hasQueuedMovement = false;
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
                string name = NameGenerator._human[Random.Range(1, NameGenerator._human.Count)];
                NameGenerator._human.Remove(name);
                return name;
            }
        }
        
        public static readonly List<string> _human = new List<string>
        {
            "Aldric",
            "Baldwin",
            "Cedric",
            "Dain",
            "Edric",
            "Fabian",
            "Godwin",
            "Halric",
            "Ivor",
            "Lambert",
            "Magnus",
            "Osric",
            "Percival",
            "Roderic",
            "Sigmund",
            "Theobald",
            "Ulric",
            "Wystan",
            "Yorick",
            "Zephram",
            "Adelina",
            "Beatrix",
            "Cecilia",
            "Diantha",
            "Elspeth",
            "Freya",
            "Giselle",
            "Helene",
            "Isolde",
            "Juliana",
            "Katarina",
            "Livia",
            "Margery",
            "Natalia",
            "Odette",
            "Rosalind",
            "Sabina",
            "Thalia",
            "Ursula",
            "Vivianne",
        };
        public static readonly List<string> _demon = new List<string>
        {
            "Azgorn",
            "Belthar",
            "Calzeth",
            "Daemora",
            "Eryx",
            "Falthor",
            "Gorrak",
            "Helzra",
            "Inferius",
            "Jyxar",
            "Karzog",
            "Lorthan",
            "Malzeth",
            "Nefyra",
            "Ozzrak",
            "Pyrak",
            "Quorath",
            "Razgoul",
            "Skarneth",
            "Thalzor",
            "Umbrak",
            "Vraxxis",
            "Xanthar",
            "Zulkarn",
            "Bob",
        };
    }
}