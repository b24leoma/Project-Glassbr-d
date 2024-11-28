namespace Game
{
    public class Human : Entity
    {
        // Start is called before the first frame update
        void Start()
        {
            isHuman = true;
            MoveRange = 3;
            AttackRange = 1;
            MaxHealth = 100;
            CurrentHealth = MaxHealth;
            Damage = 20; 
            GenerateName();
            IsMelee = AttackRange <= 1;
        }
    }
}
