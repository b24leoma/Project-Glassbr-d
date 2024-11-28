using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Game
{
    public class BattleController : MonoBehaviour
    {
        [SerializeField] private GameObject infoDisplay;
        [SerializeField] private TextMeshProUGUI displayStats;
        [SerializeField] private TextMeshProUGUI displayName;
        [SerializeField] private List<Spawning> spawnList;
        [SerializeField] private Transform entityParent;
        [SerializeField] private GameObject human;
        [SerializeField] private GameObject demon;
        private List<Entity> characters;
        public bool isPlayerTurn;
        private GridSystem gridSystem;
        private PlayerAttackSystem playerAttack;

        void Start()
        {
            characters = new List<Entity>();
            gridSystem = GetComponent<GridSystem>();   
            foreach (Spawning spawn in spawnList)
            {
                CreateEntity(spawn.Position, spawn.isHuman);
            }

            playerAttack = GetComponent<PlayerAttackSystem>();
            isPlayerTurn = true;
            infoDisplay.SetActive(false);
        }

        void CreateEntity(Vector2 pos, bool isHuman)
        {
            Entity e = Instantiate(isHuman ? human : demon, Vector3.zero, Quaternion.identity, entityParent)
                .GetComponent<Entity>();
            gridSystem.ConnectToTile(pos, e);
            characters.Add(e);
        }


        public void Move(Vector2 from, Vector2 to)
        {
            Debug.Log("Move!!!");
            gridSystem.MoveUnit(from, to);
        }
        public void Attack(Entity attacker, Entity target)
        {
            Debug.Log("Attack!!!");
            target.TakeDamage(attacker.Damage);
        }

        public void EndTurn()
        {
            isPlayerTurn = !isPlayerTurn;
            if(isPlayerTurn) playerAttack.StartTurn();
        }


        public void UpdateCharacterDisplay(bool showDisplay, Entity entity)
        {
            infoDisplay.SetActive(showDisplay);
            if (showDisplay)
            {
                displayStats.text = $"{entity.Damage}\n{entity.CurrentHealth}/{entity.MaxHealth}\n{(entity.IsMelee ? "MELEE" : "RANGED")}";
                displayName.text = entity.Name;
            }
        }



        public List<Entity> GetCharacters(){return characters;}
        public Entity GetCharacterAt(int i){return characters[i];}
    
        [System.Serializable] public class Spawning
        {
            public Vector2 Position;
            public bool isHuman;
        }
    }
}
