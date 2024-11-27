public class Human : Entity
{
    // Start is called before the first frame update
    void Start()
    {
        isHuman = true;
        Range = 2.3f;
        AttackRange = Range + 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
