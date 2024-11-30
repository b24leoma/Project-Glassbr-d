namespace Game
{
    public class HumanSpearman : Human
    {
        void Start()
        {
            MoveRange = 3;
            AttackRange = 1;
            MaxHealth = 100;
            CurrentHealth = MaxHealth;
            Damage = 20;
            IsMelee = AttackRange <= 1;
        }
    }
}
