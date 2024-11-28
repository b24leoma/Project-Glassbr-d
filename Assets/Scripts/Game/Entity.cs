using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Entity : MonoBehaviour
    {
        public Animator animator;
        public string Name { get; private set; }
        public int MaxHealth { get; protected set; }
        public int CurrentHealth { get; protected set; }
        public int Damage { get; protected set; } 
        public int MoveRange { get; protected set; } 
        public int AttackRange { get; protected set; }
        public bool IsMelee { get; protected set; }

        public bool isHuman;
    
        public bool hasQueuedMovement;
        public bool hasQueuedAttack;
    
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
    
        protected void GenerateName()
        {
            if (isHuman)
            {
                Name = NameGenerator._human[Random.Range(1, NameGenerator._human.Count)];
                NameGenerator._human.Remove(Name);
            }
            else
            {
                Name = NameGenerator._human[Random.Range(1, NameGenerator._human.Count)];
                NameGenerator._human.Remove(Name);
            }
        }
    }

    public static class NameGenerator
    {
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