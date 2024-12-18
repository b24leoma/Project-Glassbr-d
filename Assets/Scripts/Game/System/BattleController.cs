using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
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
        [SerializeField] private UnityEvent TutorialOnStart;
        [Header("Components")]
        [SerializeField] private GridSystem gridSystem;
        [SerializeField] private bool isPlayer1Turn;
        [SerializeField] private Canvas canvas;
        [SerializeField] private UIStates uiStates;
        [Header("Assets")]
        [SerializeField] private GameObject infoDisplay;
        [SerializeField] private TextMeshProUGUI displayStats;
        [SerializeField] private TextMeshProUGUI displayName;
        [SerializeField] private TextMeshProUGUI displayDescription;
        [SerializeField] private Transform entityParent;
        [SerializeField] private GameObject humanSpearman;
        [SerializeField] private GameObject humanArcher;
        [SerializeField] private GameObject demonSwordsman;
        [SerializeField] private GameObject demonTank;
        [SerializeField] private GameObject damageNumbers;
        [SerializeField] private TextAsset humanNameList;
        [SerializeField] private TextAsset demonNameList;
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
            
            
            
            NameGenerator.ReadFromFile(humanNameList, demonNameList);
            if (gridSystem == null || uiStates == null || canvas == null)
            {
                Debug.LogError("You forgot to assign some components in the inspector :)");
                return;
            }
            characters = new List<Entity>();
            foreach (SpawnEntity spawn in LevelEntities)
            {
                CreateEntity(new Vector2Int((int)spawn.Position.x, (int)spawn.Position.y), spawn.Type);
            }
            infoDisplay.SetActive(false);
            Player1TurnStart.Invoke();
            if (isTutorial) TutorialOnStart?.Invoke();
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

        public IEnumerator Move(Vector2Int[] pos, bool tryAttackAfter, Entity attackTarget = null)
        {
            Entity entity = gridSystem.GetTile(pos[0]).linkedEntity;
            
            if (isTutorial && entity.isHuman)
            {
                if ((tutorialManager.TutorialMoveTime() && pos.Length < 2) ||
                    (tutorialManager.TutorialAttackTime() && !tryAttackAfter) || (tutorialManager.TutorialBushTime()  && !gridSystem.GetTile(pos[^1]).hidingSpot))
                {
                    yield break;
                }
            }
            
            if (pos.Length > 1)
            {
                UpdateCharacterDisplay(true, entity);
                gridSystem.MoveUnit(pos[0], pos[^1]);
                entity.MoveDistance(pos.Length - 2);
                if (gridSystem.GetTile(pos[0]).hidingSpot)
                    gridSystem.SetHidingSpotColor(pos[0], Color.white);
                for (int i = 1; i < pos.Length; i++)
                {
                    if (gridSystem.GetTile(pos[i - 1]).hidingSpot)
                        gridSystem.SetHidingSpotColor(pos[i - 1], Color.white);
                    entity.MoveToTile(pos[i]);
                    SFX.MOVE(entity.Type, entity.transform.position);
                    entity.transform.DOShakeRotation(0.2f, (1 + pos.Length)).SetLoops(1, LoopType.Yoyo).SetEase(Ease.OutBounce);
                    if (gridSystem.GetTile(pos[i]).hidingSpot)
                        gridSystem.SetHidingSpotColor(pos[i], new Color(1, 1, 1, 0.4f));
                    yield return new WaitForSeconds(0.2f);
                }
            }

            if (isTutorial)
            {
                if (tutorialManager.TutorialMoveTime()) tutorialManager.Moving();
                else if (tutorialManager.TutorialBushTime())
                {
                    isTutorial = false;
                    tutorialManager.Bushing();
                }
            }

            if (tryAttackAfter && gridSystem.GetGridDistance(entity.Position, attackTarget.Position) <=
                entity.AttackRange)
            {
                Attack(entity, attackTarget);
                yield return new WaitForSeconds(0.5f);
            }
        }

        public void Attack(Entity attacker, Entity target)
        {
            if (isTutorial && attacker.isHuman)
            {
                if (!tutorialManager.TutorialAttackTime())
                {
                    return;
                }
            }
            
            attacker.SetAttacking(true);   
            float reduction = 1 - gridSystem.GetTile(target.Position).damageReductionPercent / 100f;
            target.TakeDamage(reduction * attacker.Damage);
            DamageNumber num = Instantiate(damageNumbers, target.transform.position, quaternion.identity).GetComponent<DamageNumber>();
            num.SetDamage($"-{reduction * attacker.Damage}");
            if (reduction < 1)
            {
                num = Instantiate(damageNumbers, target.transform.position + Vector3.down * 0.75f, quaternion.identity).GetComponent<DamageNumber>();
                num.SetDamage($"{gridSystem.GetTile(target.Position).damageReductionPercent}% reduction");
                num.SetSize(4.5f);
            }
            UpdateCharacterDisplay(true, target);
            attacker.MoveDistance(attacker.moveDistanceRemaining);
            if (isTutorial && attacker.isHuman) tutorialManager.Attacking();
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
                displayName.text = $"{entity.Name}\n{entity.Age}";
                displayDescription.text = entity.Description;
            }
        }
        
        
        [System.Serializable] public class SpawnEntity
        {
            public Vector2 Position;
            public Entity.EntityType Type;
        }
        
        public void DebugLose ()
        {
            uiStates.TogglePanel(0);
        }

        public void DebugWin()
        {
            uiStates.TogglePanel(1);
        }
        
        
        public void ToggleNightLightOnHumans(bool toNight)
        {
            
            foreach (var entity in characters)
            {
                if (entity is Human human)
                {
                  human.NightLightToggle(toNight);
                }
            }
        }

    }
    
    
    
    
}
