namespace Game
{
    public class DemonSwordsman : Demon
    {
        void Start()
        {
            MoveRange = 3;
            AttackRange = 1;
            MaxHealth = 100;
            CurrentHealth = MaxHealth;
            Damage = 15;
            IsMelee = AttackRange <= 1;
        }
    }
}