using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private int maxHealth;
    private int currentHealth;
    private int damage;
    private float range;
    private Vector2 Position
    {
        get {return transform.position; }
        set => transform.position = value;
    }

    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    public void MoveToTile(Vector2 pos)
    {
        Position = pos + new Vector2(-0.5f, -0.5f);
    }
}
