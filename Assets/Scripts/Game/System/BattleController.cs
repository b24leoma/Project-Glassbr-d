using System.Collections;
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
        [SerializeField] public List<SpawnEntity> LevelEntities;
        [Header("Events")]
        [SerializeField] private UnityEvent Player1TurnStart;
        [SerializeField] private UnityEvent Player2TurnStart;
        [Header("Components")]
        [SerializeField] private GridSystem gridSystem;
        [SerializeField] private bool isPlayer1Turn;
        [SerializeField] private Canvas canvas;
        [SerializeField] private UIStates uiStates;
        [Header("Assets")]
        [SerializeField] private GameObject infoDisplay;
        [SerializeField] private TextMeshProUGUI displayStats;
        [SerializeField] private TextMeshProUGUI displayName;
        [SerializeField] private Transform entityParent;
        [SerializeField] private GameObject humanSpearman;
        [SerializeField] private GameObject humanArcher;
        [SerializeField] private GameObject demonSwordsman;
        [SerializeField] private GameObject demonTank;
        [SerializeField] private GameObject damageNumber;
        private List<Entity> characters;
        private int level;
        private string currentScene;
        private string tutorialScene;
        private bool isTutorial;
        private TutorialManager tutorialManager;

        
        void Start()
        {
            currentScene = SceneManager.GetActiveScene().name;
            tutorialScene = "Tutorial";

            if (currentScene == tutorialScene)
            {
               tutorialManager = GetComponent<TutorialManager>();
                
                isTutorial = true;
            }
            
            
            
            NameGenerator.RepopulateList();
            if (gridSystem == null || uiStates == null || canvas == null)
            {
                Debug.LogError("You forgot to assign some components in the inspector :)");
                return;
            }
            characters = new List<Entity>();
            for (int i = characters.Count; i > 0; i++)
            {
                Destroy(characters[i].gameObject);
                characters.RemoveAt(i);
            }
            LoadLevel();
        }

        void LoadLevel()
        {
            characters.Clear();
            foreach (SpawnEntity spawn in LevelEntities)
            {
                CreateEntity(new Vector2Int((int)spawn.Position.x, (int)spawn.Position.y), spawn.Type);
            }
            infoDisplay.SetActive(false);
            Player1TurnStart.Invoke();
        }

        public void NextLevel()
        {
            SceneManager.LoadScene("Game");
        }

        void CreateEntity(Vector2Int pos, Entity.EntityType type)
        {
            GameObject g = null;
            switch (type)
            {
                case Entity.EntityType.HumanSpearman:
                    g = Instantiate(humanSpearman, Vector3.zero, Quaternion.identity, entityParent);
                    break;
                case Entity.EntityType.HumanArcher:
                    g = Instantiate(humanArcher, Vector3.zero, Quaternion.identity, entityParent);
                    break;
                case Entity.EntityType.DemonSwordsman:
                    g = Instantiate(demonSwordsman, Vector3.zero, Quaternion.identity, entityParent);
                    break;
                case Entity.EntityType.DemonTank:
                    g = Instantiate(demonTank, Vector3.zero, Quaternion.identity, entityParent);
                    break;
            }
            Entity e = g.GetComponent<Entity>();
            if (e.isHuman) gridSystem.humans.Add(pos);
            else gridSystem.demons.Add(pos);
            gridSystem.ConnectToTile(pos, e);
            characters.Add(e);
        }

        public IEnumerator Move(Vector2Int[] pos, bool attackAfter, Entity attackTarget = null)
        {
            Entity entity = gridSystem.GetTile(pos[0]).linkedEntity;
            if (pos != null && pos.Length > 1)
            {
                gridSystem.GetTile(pos[0]).linkedEntity.SetMoving(true);
                FMODManager.instance.OneShot("GenericWalk", new Vector3(0, 0, 0));
                if (isTutorial)
                {
                    TutorialTrigger();
                }
                
                gridSystem.MoveUnit(pos[0], pos[^1]);
                if (gridSystem.GetTile(pos[0]).hidingSpot)
                    gridSystem.SetHidingSpotColor(pos[0], Color.white);
                for (int i = 1; i < pos.Length; i++)
                {
                    if (gridSystem.GetTile(pos[i - 1]).hidingSpot)
                        gridSystem.SetHidingSpotColor(pos[i - 1], Color.white);
                    entity.MoveToTile(pos[i]);
                    if (gridSystem.GetTile(pos[i]).hidingSpot)
                        gridSystem.SetHidingSpotColor(pos[i], new Color(1, 1, 1, 0.5f));
                    yield return new WaitForSeconds(0.2f);
                }
            }

            if (attackAfter && gridSystem.GetGridDistance(entity.Position, attackTarget.Position) <= entity.AttackRange)
            {
                Attack(entity, attackTarget);
            }
        }
        
        public void Attack(Entity attacker, Entity target)
        {
            attacker.SetAttacking(true);   
            if (isTutorial)
            {
                TutorialTrigger();
            }
            
            
            float reduction = 1 - (gridSystem.GetTile(new Vector2Int((int)(target.Position.x+0.6f), (int)(target.Position.y+0.6f))) //0.6 due to floating point math suck
                .damageReductionPercent / 100f);
            target.TakeDamage(reduction * attacker.Damage);
            GameObject dmg = Instantiate(damageNumber);
            dmg.transform.position = target.transform.position;
            dmg.GetComponent<DamageNumber>().SetDamage(reduction * attacker.Damage);
            if (target.CurrentHealth <= 0)
            {
                if (target.isHuman)
                {
                    gridSystem.humans.Remove(target.Position);
                    if (gridSystem.humans.Count == 0)
                    {
                        uiStates.TogglePanel(0);
                    }
                }
                else
                {
                    gridSystem.demons.Remove(target.Position);
                    if (gridSystem.demons.Count == 0)
                    {
                        uiStates.TogglePanel(1);
                    }
                }
                target.Kill();
                
            }
            
            
            FMODManager.instance.OneShot("GenericAttack", attacker.transform.position);
            FMODManager.instance.OneShot("GenericHit", target.transform.position);
            
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
            public Entity.EntityType Type;
        }

        [System.Serializable]
        public class Level
        {
            public List<SpawnEntity> spawnList;
        }
        
        public void DebugLose ()
        {
            uiStates.TogglePanel(0);
        }

        public void DebugWin()
        {
            uiStates.TogglePanel(1);
        }

        private void TutorialTrigger ()
        {
            tutorialManager.TotalStateChecker();
        }
    }
    
    
    
}
