using UnityEngine;

namespace Game
{
    public class Human : Entity
    {
        void Awake()
        {
           isHuman = true;
           Name = NameGenerator.GenerateName(true);
        }
    }
}
