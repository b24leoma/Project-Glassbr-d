namespace Game
{
    public class DemonTank : Demon
    {
        void Start()
        {
            MoveRange = 2;
            AttackRange = 1;
            MaxHealth = 200;
            CurrentHealth = MaxHealth;
            Damage = 10;
            IsMelee = AttackRange <= 1;
        }
    }
}
