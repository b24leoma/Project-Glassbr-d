using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour


{
    public Animator animator;

    public string Name;
    public int MaxHealth { get; protected set; }
    public int CurrentHealth { get; protected set; }
    public int Damage { get; protected set; } 
    public float Range { get; protected set; } 
    public float AttackRange { get; protected set; }

    public bool isHuman;
    public Vector2 Position
    {
        get {return transform.position; }
        set => transform.position = value;
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth-= damage;
    }

    private static List<string> _human = new List<string>
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
    private static List<string> _demon = new List<string>
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
    
    
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void MoveToTile(Vector2 pos)
    {
        Position = pos + new Vector2(-0.5f, -0.5f);
    }

    public string GenerateName(bool isHuman)
    {

        if (isHuman)
        {
            return _human[Random.Range(1, _demon.Count)]; 
        }
        return _demon[Random.Range(1, _demon.Count)];
    }
}
