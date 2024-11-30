namespace Game
{
    public class HumanArcher : Human
    {
        void Start()
        {
            MoveRange = 2;
            AttackRange = 4;
            MaxHealth = 60;
            CurrentHealth = MaxHealth;
            Damage = 10;
            IsMelee = AttackRange <= 1;
        }
    }
}
