using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Game
{
    public class BattleController : MonoBehaviour
    {
        [Header("Levels")]
        [SerializeField] public List<Level> LevelEntities;
        [Header("Events")]
        [SerializeField] private UnityEvent Player1TurnStart;
        [SerializeField] private UnityEvent Player2TurnStart;
        [Header("Components")]
        [SerializeField] private GridSystem gridSystem;
        [SerializeField] private bool isPlayer1Turn;  
        [Header("Assets")]
        [SerializeField] private GameObject infoDisplay;
        [SerializeField] private TextMeshProUGUI displayStats;
        [SerializeField] private TextMeshProUGUI displayName;
        [SerializeField] private Transform entityParent;
        [SerializeField] private GameObject human;
        [SerializeField] private GameObject demon;
        private List<Entity> characters;
        private int level;
        void Start()
        {
            characters = new List<Entity>();
            LoadLevel();
        }

        void LoadLevel()
        {
            characters.Clear();
            foreach (SpawnEntity spawn in LevelEntities[level].spawnList)
            {
                CreateEntity(spawn.Position, spawn.isHuman);
            }
            infoDisplay.SetActive(false);
        }

        public void NextLevel()
        {
            level++;
            if (level >= LevelEntities.Count)
            {
                Debug.Log("Win");
            }
            else
            {
                gridSystem.ClearGrid();
                LoadLevel();
            }
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
            isPlayer1Turn = !isPlayer1Turn;
            if(isPlayer1Turn) Player1TurnStart?.Invoke();
            else Player2TurnStart?.Invoke();
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
    
        [System.Serializable] public class SpawnEntity
        {
            public Vector2 Position;
            public bool isHuman;
        }

        [System.Serializable]
        public class Level
        {
            public List<SpawnEntity> spawnList;
        }
    }
}
