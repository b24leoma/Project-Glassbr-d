namespace Game
{
    public class Demon : Entity
    {
        // Start is called before the first frame update
        void Start()
        {
            isHuman = false;
            MoveRange = 2;
            AttackRange = 1;
            MaxHealth = 100;
            CurrentHealth = MaxHealth;
            Damage = 10;
            GenerateName();
            IsMelee = AttackRange <= 1;
        }
    }
}
