using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : Entity
{
    // Start is called before the first frame update
    void Start()
    {
        isHuman = false;
        Range = 1.2f;
        AttackRange = 1;
        MaxHealth = 200;
        CurrentHealth = MaxHealth;
        Damage = 10;
        Name = GenerateName(isHuman);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
