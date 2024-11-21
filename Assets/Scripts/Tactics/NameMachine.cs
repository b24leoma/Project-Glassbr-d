using System.Collections.Generic;
using UnityEngine;

public class NameMachine : MonoBehaviour
{

    private List<string> _humanM;
    private List<string> _humanF;
    private List<string> _demon;

    void Start()
    {
        _humanM = new List<string>
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
        };

        _humanF = new List<string>
        {
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

        _demon = new List<string>
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

    public string TryGenerateName(bool gender, string faction)
    {
        if (string.IsNullOrEmpty(faction))
        {
            Debug.LogWarning("Warning! Invalid faction string!");
            return "Nameless";
        }

        if (faction == "human")
        {
            if (gender)
            {
                return _humanF[Random.Range(0, _humanF.Count)];
            }

            return _humanM[Random.Range(0, _humanM.Count)];
        }

        if (faction == "demon")
        {
            return _demon[Random.Range(1, _demon.Count)];
        }

        return "Not supposed to happen!!!";
    }
}
