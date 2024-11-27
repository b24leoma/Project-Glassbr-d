using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demon : Entity
{
    // Start is called before the first frame update
    void Start()
    {
        isHuman = false;
        Range = 2.3f;
        AttackRange = Range + 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
