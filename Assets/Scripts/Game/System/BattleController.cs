using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
        [SerializeField] private UnityEvent MoveDone;
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
        [SerializeField] private Slider displayHealthSlider;
        [SerializeField] private Transform entityParent;
        [SerializeField] private GameObject humanSpearman;
        [SerializeField] private GameObject humanArcher;
        [SerializeField] private GameObject humanTank;
        [SerializeField] private GameObject demonSwordsman;
        [SerializeField] private GameObject demonTank;
        [SerializeField] private GameObject damageNumbers;
        [SerializeField] private TextAsset humanNameList;
        [SerializeField] private TextAsset demonNameList;
        [SerializeField] private Transform selectHighlight;
        private int critChance;
        private int missChance;
        private List<Entity> characters;
        private int level;
        private string currentScene;
        private string tutorialScene;
        private bool isTutorial;
        private TutorialManager tutorialManager;
        private NameSystem nameSystem;

        private int _attackvoids;

        
        void Start()
        {
            critChance = 5;
            missChance = 3;
            currentScene = SceneManager.GetActiveScene().name;
            tutorialScene = "Tutorial";
            nameSystem = FindObjectOfType<NameSystem>();

            if (currentScene == tutorialScene)
            {
               tutorialManager = GetComponent<TutorialManager>();
                isTutorial = true;
                nameSystem.RefillNames(humanNameList, demonNameList);
            }
            
            if (gridSystem == null || uiStates == null || canvas == null)
            {
                Debug.LogError("You forgot to assign some components in the inspector :)");
                return;
            }
            characters = new List<Entity>();
            nameSystem.NewLevel();
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
                case Entity.EntityType.HumanTank:
                    g = Instantiate(humanTank, Vector3.zero, Quaternion.identity, entityParent);
                    break;
                case Entity.EntityType.DemonSwordsman:
                    g = Instantiate(demonSwordsman, Vector3.zero, Quaternion.identity, entityParent);
                    break;
                case Entity.EntityType.DemonTank:
                    g = Instantiate(demonTank, Vector3.zero, Quaternion.identity, entityParent);
                    break;
            }
            Entity e = g.GetComponent<Entity>();
            nameSystem.GiveIdentity(e);
            if (e.isHuman) gridSystem.humans.Add(pos);
            else gridSystem.demons.Add(pos);
            gridSystem.ConnectToTile(pos, e);
            characters.Add(e);
        }

        public IEnumerator Move(Vector2Int[] pos, bool tryAttackAfter, Entity attackTarget = null, bool skipAnimation = false)
        {
            Entity entity = gridSystem.GetTile(pos[0]).linkedEntity;
            if (entity == null) yield break;
            
            if (entity.isHuman)
            {
                if (isTutorial && ((tutorialManager.TutorialMoveTime() && pos.Length < 2) ||
                                   (tutorialManager.TutorialAttackTime() && !tryAttackAfter) ||
                                   (tutorialManager.TutorialBushTime() && !gridSystem.GetTile(pos[^1]).hidingSpot)))
                {
                    yield break;
                }
                entity.GetComponent<Human>().SetMoving(true);
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
                    if (i > 1)
                    {
                        if (pos[i].x < entity.Position.x && !entity.Flipped) entity.Flip();
                        if (pos[i].x > entity.Position.x && entity.Flipped) entity.Flip();
                    }
                    entity.MoveToTile(pos[i]);
                    selectHighlight.position = entity.transform.position;
                    SFX.MOVE(entity.Type, entity.transform.position);
                    entity.transform.DOShakeRotation(0.2f, (1 + pos.Length)).SetLoops(1, LoopType.Yoyo).SetEase(Ease.OutBounce);
                    if (gridSystem.GetTile(pos[i]).hidingSpot)
                        gridSystem.SetHidingSpotColor(pos[i], new Color(1, 1, 1, 0.4f));
                    if (!skipAnimation) yield return new WaitForSeconds(0.2f);
                }
            }

            if (entity.isHuman)
            {
                entity.GetComponent<Human>().SetMoving(false);
                MoveDone?.Invoke();
            }

            if (tryAttackAfter && gridSystem.GetGridDistance(entity.Position, attackTarget.Position) <=
                entity.AttackRange)
            {
                Attack(entity, attackTarget);
                if (!skipAnimation) yield return new WaitForSeconds(0.5f);
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
        }

        public void Attack(Entity attacker, Entity target)
        {
            if (isTutorial && attacker.isHuman && !tutorialManager.TutorialAttackTime()) return;

            if (attacker.Type == Entity.EntityType.HumanArcher && _attackvoids == 0 ||
                attacker.Type != Entity.EntityType.HumanArcher)
            {
                attacker.SetAttacking(true);
            }


            if (attacker.Type == Entity.EntityType.HumanArcher && _attackvoids == 0)
            {
               StartCoroutine( ShootArrowAfterDelay(attacker, target, 0.3f));
               StartCoroutine( DelayAttackLogic(attacker, target, 0.55f));
               return;
            }

            if (!attacker.isHuman) selectHighlight.position = attacker.transform.position;
            Tile tile = gridSystem.GetTile(target.Position);
            int  damage = Random.Range(attacker.MinDamage, attacker.MaxDamage);
            bool crit = Random.Range(1, 100) < critChance;
            DamageNumber num;
            if (Random.Range(1, 100) < tile.missChancePercent || Random.Range(1, 100) < critChance)
            {
                // ---MISS---
                num = Instantiate(damageNumbers, target.transform.position, quaternion.identity)
                    .GetComponent<DamageNumber>();
                num.SetDamage($"MISS");
            }
            else
            {
                Vector3 targetPos = target.transform.position;
                if (crit)
                {
                    damage = attacker.MaxDamage + 10;
                    num = Instantiate(damageNumbers, targetPos + Vector3.up * 1.25f, quaternion.identity)
                        .GetComponent<DamageNumber>();
                    num.SetDamage($"CRITICAL HIT");
                }

                if (tile.damageReductionPercent > 0)
                {
                    // ---REDUCTION---
                    float reduction = 1 - tile.damageReductionPercent / 100f;
                    damage = Mathf.RoundToInt(damage * reduction);
                    num = Instantiate(damageNumbers, targetPos, quaternion.identity)
                        .GetComponent<DamageNumber>();
                    num.SetDamage($"-{damage}");
                    num.SetSize(5.5f);

                    num = Instantiate(damageNumbers, targetPos + Vector3.down * 0.65f, quaternion.identity)
                        .GetComponent<DamageNumber>();
                    num.SetDamage($"{tile.damageReductionPercent}% reduction");
                    num.SetSize(4.5f);
                }
                else
                {
                    // ---NORMAL ATTACK---
                    num = Instantiate(damageNumbers, targetPos, quaternion.identity).GetComponent<DamageNumber>();
                    num.SetDamage($"-{damage}");
                    num.SetSize(5.5f);
                }

                target.TakeDamage(damage);
            }

            UpdateCharacterDisplay(true, target);
            attacker.MoveDistance(attacker.moveDistanceRemaining);
            if (isTutorial && attacker.isHuman) tutorialManager.Attacking();
            if (target.CurrentHealth <= 0)
            {
                target.TakeDamage(target.CurrentHealth);
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
                UpdateCharacterDisplay(true, target);
                target.Kill();
            }

            if (attacker.isHuman)
            {
                selectHighlight.position = Vector3.down * 100;
                bool allAttacked = true;
                foreach (Vector2Int humanPos in gridSystem.humans)
                {
                    if (!gridSystem.GetTile(humanPos).linkedEntity.hasAttacked)
                    {
                        allAttacked = false;
                    }
                }

                if (allAttacked)
                {
                    isPlayer1Turn = false;
                    Player2TurnStart?.Invoke();
                }
            }

            _attackvoids = 0;
        }

        public void EndTurn()
        {
            selectHighlight.position = Vector3.down * 100;
            isPlayer1Turn = !isPlayer1Turn;
            if(isPlayer1Turn) Player1TurnStart?.Invoke();
            else Player2TurnStart?.Invoke();
        }


        public void UpdateCharacterDisplay(bool showDisplay, Entity entity)
        {
            infoDisplay.SetActive(showDisplay);
            if (showDisplay)
            {
                displayStats.text = $"{entity.MinDamage}-{entity.MaxDamage}\n{entity.CurrentHealth}/{entity.MaxHealth}\n{(entity.IsMelee ? "MELEE" : "RANGED")}";
                displayName.text = $"{entity.Name}\n{entity.Age}";
                displayDescription.text = entity.Description;
                HealthDisplayCalculator(entity.CurrentHealth, entity.MaxHealth);
                
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

        private void HealthDisplayCalculator(float currentHealth, float maxHealth)
        {
            if (currentHealth <= 0) return;

            if (displayHealthSlider.maxValue != maxHealth)
            {
                displayHealthSlider.maxValue = maxHealth;
            }

            if (displayHealthSlider.value != currentHealth)
            {
                displayHealthSlider.value = currentHealth;
            }
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



        private static IEnumerator ShootArrowAfterDelay(Entity attacker, Entity target, float delay)
        {
            yield return new WaitForSeconds(delay); 
            ShootArrow(attacker, target);  
        }
        private static void ShootArrow(Entity attacker, Entity target)
        {
            var arrow = Instantiate(attacker.arrowPrefab, attacker.transform.position, Quaternion.identity);

            const float minArch = 0.2f;
            const float maxArch = 1f;
            var attackerPos = attacker.transform.position;
            var targetPos = target.transform.position;
            var distance = Vector3.Distance(attackerPos, targetPos);
            var clampedDistance = Mathf.Clamp(distance, minArch, maxArch);

            var arcHeight = Mathf.Lerp(minArch, maxArch, Mathf.InverseLerp(minArch, maxArch, clampedDistance));
            

           var middlePos = (attackerPos + targetPos)*0.5f;
            
           
            
            middlePos.y += arcHeight;
          
          
          var point1 = Vector3.Lerp(attackerPos, middlePos, 0.5f);
          var point2 = Vector3.Lerp(middlePos, targetPos, 0.5f);
          
          
          
         
       
            
           
            

          Vector3[] arrowPath = { point1,point2, targetPos, };
          
          Debug.DrawLine(attackerPos, point1, Color.red, 10f);
          Debug.DrawLine(point1, middlePos, Color.yellow, 10f);
          Debug.DrawLine(middlePos, point2, Color.cyan, 10f);
          Debug.DrawLine(point2, targetPos, Color.blue,10f);

            
       
            var duration = Mathf.Clamp(distance * 0.05f, 0.2f, 0.5f);

            arrow.transform.DOPath(arrowPath, duration, PathType.CatmullRom, PathMode.TopDown2D, gizmoColor: Color.red).SetEase(Ease.InOutSine).OnKill(() => Destroy(arrow));
        }
        
        private IEnumerator DelayAttackLogic(Entity attacker, Entity target, float delay)
        {
            _attackvoids++;
            yield return new WaitForSeconds(delay);
            Attack(attacker, target);
        }


        
        

    }
    
    
    
    
}
