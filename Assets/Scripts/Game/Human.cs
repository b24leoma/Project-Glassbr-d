public class Human : Entity
{
    // Start is called before the first frame update
    void Start()
    {
        isHuman = true;
        Range = 2.3f;
        AttackRange = Range + 1;
        MaxHealth = 100;
        CurrentHealth = MaxHealth;
        Damage = 20;
        Name = GenerateName(isHuman);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
