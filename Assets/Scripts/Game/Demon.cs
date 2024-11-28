public class Demon : Entity
{
    // Start is called before the first frame update
    void Start()
    {
        isHuman = false;
        MoveRange = 2;
        AttackRange = 2;
        MaxHealth = 100;
        CurrentHealth = MaxHealth;
        Damage = 10;
        Name = GenerateName(isHuman);
        IsMelee = AttackRange <= 1;
    }
}
