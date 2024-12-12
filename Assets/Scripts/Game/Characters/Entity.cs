using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Entity : MonoBehaviour
    {
        public enum EntityType
        {
            HumanSpearman,
            HumanArcher,
            DemonSwordsman,
            DemonTank
        };

        public EntityType Type;
        private Animator animator;
        public string Name { get; protected set; }
        public int MaxHealth;
        public int CurrentHealth { get; protected set; }
        public int Damage;
        public int MoveRange;
        public int AttackRange;
        public bool IsMelee { get; protected set; }
        
        [HideInInspector] public bool isHuman;
        [HideInInspector] public bool hasMoved;
        [HideInInspector] public bool hasAttacked;
    
        public Vector2Int Position
        {
            get => new ((int)(transform.position.x+0.5f), (int)(transform.position.y+0.5f));
            set => transform.position = new Vector3(value.x -0.5f, value.y-0.5f);
        }

        public void TakeDamage(float damage)
        {
            CurrentHealth-= (int)damage;
        }
    
    
        void Start()
        {
            animator = GetComponent<Animator>();
            hasMoved = false;
            hasMoved = false;
            CurrentHealth = MaxHealth;
            IsMelee = AttackRange <= 1;
        }

        public void MoveToTile(Vector2Int pos)
        {
            Position = pos;
        }


        public virtual void SetMoving(bool moving)
        {
            hasMoved = moving;
        }

        public virtual void SetAttacking(bool attacking)
        {
            hasAttacked = attacking;
            if (attacking) PlayAttack();
                
        }

        public void Kill()
        {
            Destroy(gameObject);
        }
        
        protected void PlayAttack()
        {
            animator.SetTrigger("Attack");
        }
    }

    public static class NameGenerator
    {
        private static List<string> _human;
        private static List<string> _demon;
        public static void RepopulateList()
        {
            _human = new List<string>
            {
                "Sigrid",
                "Mathilda",
                "Brenna",
                "Col Baker\nAge:36\nHe has always followed in his mothers footsteps. Therefore he followed suit into battle. Always ready to protect his mother.",
                "Nicole",
                "Borko Caldwell\nAge:32\nHe is thick like a demons hide, but he has a heart of a true knight.",
                "Oliver Gestour\nAge:35\nCan somehow still joke and fun during downtimes. His cling to hope is fascinating.",
                "Ivar Godwin\nAge:17\nHe was a willing recruit as a soldier. He is constantly romanticizing the job and is one of the few with great hope left.",
                "Colette Baker\nAge:30\nTough and hard headed, she fights brutally and gruff. But she never misses the chance to gorge on sweet things.",
                "Hildeth Pendragon\nAge:21\nShe is a very scared person, but she wants to prove herselves fiercely. Therefore she makes dumb mistakes.",
                "Victoria Fitzgerald\nAge:25\nHer mom previously had this role before her head was ripped off. Now Victoria seeks revenge.",
                "Berenice Ironside\nAge:45\nOne of the great blacksmith in the capital, when soldiers started to run short she was recruited.",
                "Tilda Quenell\nAge:22\nHer parents raised a strong resilience in her. So she sees a distant future of hope, but there still a glance over her eyes from the people she has lost.",
                "Luthera Guinevere\nAge:28\nA noblewoman who has now been dragged into the war. She hates the uncleanness of the war.",
                "Maude Lumley\nAge:31\nShe has a strong swing she got by cleaving meat in her butcher shop.",
                "Esme Blackwater\nAge:18\nShe joined the army as quick as she could so she could die with some purpose.",
                "Wilma Oakshield\nAge:39\nEvery calm moment she looks down a bottle. seeing the faces of all she failed to protect in the alcohol.",
                "Erika Blackwood\nAge:32\nHer village often had demon attacks even before the war. So she grew up in an environment of horrid violence. But she got used to it, she even started to love it.",
                "Annora Falkenrath\nAge:25\nA peasant, drafted early int the war with her family. Now it is only her and her younger sister left.",
                "Desislava Morgansson\nAge:33\nThe daughter of a fisherman, her father was killed by a demon that snuck up on the boat. Her only family, now gone.",
                "Agatha Carver\nAge:43\nShe's tired. She holds resentment against all who can be blamed with Her ice cold fury.",
                "Maxim Grimm\nAge:24\nAsk one of the few still alive who knew him 8 years ago and you will hear about his giant beautiful smile. Because you will never see it anymore.",
                "Leopold Xavier\nAge:33\nA nomad who travelled through the kingdom. He visited almost every town or city and therefore knows a lot about the landscape and is saddened at every destroyed town.",
                "Frost Norbury\nAge:14\nDespite his young age he is very intelligent and despite his name he doesn't like winters.",
                "Ferdinand Guinevere\nAge:28\nHe thinks of himself as the best combaten. New in this war he will soon learn otherwise or meet a demons jaw.",
                "Baldwin Marsden\nAge:40\nLiked by few. He has a rough skin and a sharp tongue. He wishes to return to his cabin in the woods.",
                "Wilkie Inglewood\nAge:19\nWilkie the wimp he was called. But as a wimp he hid while his village was mauled. And now his blood scalds.",
                "Harold Godfrey\nAge:46\nA cleric who ler people take cover in his cathedral. His pacifism was slowly worn down by seeing how much pain swept through it each day.",
                "Emmerich\n",
                "Dragan Lovelace\nAge:24\nHe hopes to find his true love somehow. Therefore he fights for a future to settle down.",
                "Bridget Axton\nAge:30\nShe's grateful that she is now a soldier, because of her past as an executioner.",
            };
            _demon = new List<string>
            {
                "Luster\nThe sins of lustful people",
                "Larceny\nThe sins of thieves",
                "Execution\nThe sins of executing",
                "Devours\nThe sins of the gluttonous",
                "Picker\nThe sins of multiple pickpocketing",
                "Haughty\nThe sins of the prideful",
                "Ire\nThe sins of acts in fury",
                "Rapacity\nThe sins of greedy individuals",
                "Envyn\nThe sins of envious people",
                "Idlek\nThe sins from inaction",
                "Immodera\nThe sins of owning in over excess",
                "Solipsi\nThe sins of the ego",
                "Treach\nThe sins of betrayals",
                "Wiolat\nThe sins of violations",
                "Malick\nThe sins from every holding malice",
                "Lia\nThe sins of every lie",
                "Torment\nThe sins from tortures",
                "Pherver\nThe sins from the depraved",
                "Cultest\nThe sins from violent worshipping",
                "Preshor\nThe sins of those who opress",
                "Bob\nThe sins of violent thefts",
            };
        }
        public static string GenerateName(bool isHuman)
        {
            if (isHuman)
            {
                string name = _human[Random.Range(1, _human.Count)];
                _human.Remove(name);
                return name;
                
            }
            else
            {
                string name = _demon[Random.Range(1, _demon.Count)];
                _demon.Remove(name);
                return name;
            }
        }
        
    }
}