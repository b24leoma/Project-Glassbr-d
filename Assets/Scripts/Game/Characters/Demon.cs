namespace Game
{
    public class Demon : Entity
    {
        // Start is called before the first frame update
        void Awake()
        {
            isHuman = false;
            Name = NameGenerator.GenerateName(false);
        }
    }
}
