using UnityEngine;

public class Entity : MonoBehaviour
{
    public Animator animator;
    
    public int MaxHealth { get; protected set; }
    public int CurrentHealth { get; protected set; }
    public int Damage { get; protected set; } 
    public float Range { get; protected set; } 

    public bool isHuman;
    public Vector2 Position
    {
        get {return transform.position; }
        set => transform.position = value;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void MoveToTile(Vector2 pos)
    {
        Position = pos + new Vector2(-0.5f, -0.5f);
    }
}
