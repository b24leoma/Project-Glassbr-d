using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
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
        [SerializeField] private UnityEvent GameDone;
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
        [SerializeField] private Image displayPortrait;
        [SerializeField] private Slider displayHealthSlider;
        [SerializeField] private Transform entityParent;
        [SerializeField] private GameObject humanSpearman;
        [SerializeField] private GameObject humanArcher;
        [SerializeField] private GameObject humanTank;
        [SerializeField] private GameObject demonSwordsman;
        [SerializeField] private GameObject demonTank;
        [SerializeField] private GameObject demonArcher;
        [SerializeField] private GameObject damageNumbers;
        [SerializeField] private TextAsset humanNameList;
        [SerializeField] private TextAsset demonNameList;
        [SerializeField] private Transform selectHighlight;
        private List<Entity> characters;
        private int level;
        private string currentScene;
        private string tutorialScene;
        private bool isTutorial;
        private TutorialManager tutorialManager;
        private NameSystem nameSystem;
        private List<string> deadHumans;
        

        
        void Start()
        {
            currentScene = SceneManager.GetActiveScene().name;
            tutorialScene = "Tutorial";
            nameSystem = FindObjectOfType<NameSystem>();
            
            if (currentScene == tutorialScene)
            { 
                nameSystem.RefillNames(humanNameList, demonNameList);
                tutorialManager = GetComponent<TutorialManager>();
                isTutorial = true;
            }
            else if (nameSystem.NoNames()) nameSystem.RefillNames(humanNameList, demonNameList);
            
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
            deadHumans = new List<string>();
        }

        private void OnEnable()
        {
            UIManager.EndTurnEvent += EndTurn;
        }


        private void OnDisable()
        {
            UIManager.EndTurnEvent -= EndTurn;
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
                case Entity.EntityType.DemonArcher:
                    g = Instantiate(demonArcher, Vector3.zero, Quaternion.identity, entityParent);
                    break;
            }
            Entity e = g.GetComponent<Entity>();
            nameSystem.GiveIdentity(e);
            if (e.isHuman) gridSystem.humans.Add(pos);
            else gridSystem.demons.Add(pos);
            gridSystem.ConnectToTile(pos, e);
            characters.Add(e);
            e.Position = e.Position;
        }

        public IEnumerator Move(Vector2Int[] pos, bool tryAttackAfter, Entity attackTarget = null, bool skipAnimation = false)
        {
            Entity entity = gridSystem.GetTile(pos[0]).linkedEntity;
            if (entity == null) yield break;

            if (entity.isHuman)
            {
                
                if (entity.hasAttacked || entity.GetComponent<Human>().isDefending || (isTutorial &&
                        ((tutorialManager.TutorialMoveTime() && pos.Length < 2) ||
                         (tutorialManager.TutorialAttackTime() && !tryAttackAfter) ||
                         (tutorialManager.TutorialBushTime() && !gridSystem.GetTile(pos[^1]).hidingSpot) ||
                         tutorialManager.TutorialEndTurnTime())))
                {
                    yield break;
                }

                
                
                
                entity.GetComponent<Human>().SetMoving(true);
            }
            else
            {
                UpdateCharacterDisplay(false, null, false);
            }
            
            if (pos.Length > 1)
            {
                if (entity.isHuman) UpdateCharacterDisplay(true, entity, true);
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
                    selectHighlight.GetComponent<SpriteRenderer>().color = entity.isHuman ? Color.white : Color.red;
                    SFX.MOVE(entity);
                    entity.transform.DOShakeRotation(0.2f, (1 + pos.Length)).SetLoops(1, LoopType.Yoyo).SetEase(Ease.OutBounce);
                    if (gridSystem.GetTile(pos[i]).hidingSpot)
                        gridSystem.SetHidingSpotColor(pos[i], new Color(1, 1, 1, 0.4f));
                    if (!skipAnimation) yield return new WaitForSeconds(0.2f);
                }
            }

            if (entity.isHuman)
            {
                entity.GetComponent<Human>().SetMoving(false);
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
            MoveDone?.Invoke();
        }

        public void Attack(Entity attacker, Entity target)
        {
            if (isTutorial && attacker.isHuman && !tutorialManager.TutorialAttackTime()) return;
            if (attacker.isHuman && attacker.GetComponent<Human>().isDefending) return;
            if (attacker.isHuman && attacker.IsMelee && attacker.hasAttacked) return;
            if (target == null) return;
            
            switch (attacker.Type)
            {
                case Entity.EntityType.DemonArcher :
                    SetAttackingAndFlip(attacker, target);
                    StartCoroutine( ShootArrowAfterDelay(attacker, target, 0.3f));
                    break;
                case Entity.EntityType.HumanArcher :
                    SetAttackingAndFlip(attacker, target);
                    StartCoroutine( ShootArrowAfterDelay(attacker, target, 0.3f));
                    break;
                
                default:
                    SetAttackingAndFlip(attacker, target); AttackLogic(attacker, target); break;
            }
            
            //MoveDone?.Invoke();
        }

        private void AttackLogic(Entity attacker, Entity target)
        {
            if (!attacker.isHuman)
            {
                DemonHighlightEntity(attacker);
            }
            var tile = DoDamage(attacker, target, out var damage);
            if (target.isHuman && target.GetComponent<Human>().isDefending) damage = Mathf.RoundToInt(damage * 0.95f);
            DamageNumber num;
            if (Random.Range(1, 100) <= attacker.MissChance)
            {
                damage = MissDamageUIUpdate(target);
            }
            else
            {
                Vector3 targetPos = target.transform.position;
                if (Random.Range(1, 100) <= attacker.CritChance)
                {
                    damage = CritDamageUIUpdate(attacker, target, targetPos);
                }

                if (tile.damageReductionPercent > 0)
                {
                    damage = ReducedDamageUIUpdate(tile, damage, targetPos);
                }
                else
                {
                    NormalDamageUIUpdate(targetPos, damage);
                }

                DamageTarget(target, damage);
            }

            UpdateCharacterDisplay(true, target, false);
            
            attacker.MoveDistance(attacker.moveDistanceRemaining);
            if (isTutorial && attacker.isHuman) tutorialManager.Attacking();
            if (target.CurrentHealth <= 0 && damage > 0)
            {
                if (IfTargetDied(target)) return;
            }

            if (attacker.isHuman)
            {
                AllAttacked();
            }
        }

        private static void SetAttackingAndFlip(Entity attacker, Entity target)
        {
            attacker.SetAttacking(true);
            if (target.Position.x < attacker.Position.x && !attacker.Flipped) attacker.Flip();
            if (target.Position.x > attacker.Position.x && attacker.Flipped) attacker.Flip();
        }

        private void DemonHighlightEntity(Entity attacker)
        {
            selectHighlight.position = attacker.transform.position;
            selectHighlight.GetComponent<SpriteRenderer>().color = Color.red;
        }

        private Tile DoDamage(Entity attacker, Entity target, out int damage)
        {
            Tile tile = gridSystem.GetTile(target.Position);
            damage = Random.Range(attacker.MinDamage, attacker.MaxDamage);
            return tile;
        }

        private int MissDamageUIUpdate(Entity target)
        {
            DamageNumber num;
            int damage;
            // ---MISS---
            num = Instantiate(damageNumbers, target.transform.position, quaternion.identity)
                .GetComponent<DamageNumber>();
            num.SetDamage($"MISS");
            damage = 0;
            FMODManager.instance.OneShot("Miss", target.transform.position);
            return damage;
        }

        private int CritDamageUIUpdate(Entity attacker, Entity target, Vector3 targetPos)
        {
            int damage;
            DamageNumber num;
            // ---CRIT---
            damage = attacker.MaxDamage + 10;
            if (target.isHuman && target.GetComponent<Human>().isDefending) damage = Mathf.RoundToInt(damage * 0.95f);
            num = Instantiate(damageNumbers, targetPos + Vector3.up * 1.25f, quaternion.identity)
                .GetComponent<DamageNumber>();
            num.SetDamage($"CRITICAL HIT");
            return damage;
        }

        private int ReducedDamageUIUpdate(Tile tile, int damage, Vector3 targetPos)
        {
            DamageNumber num;
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
            return damage;
        }

        private void NormalDamageUIUpdate(Vector3 targetPos, int damage)
        {
            DamageNumber num;
            // ---NORMAL ATTACK---
            num = Instantiate(damageNumbers, targetPos, quaternion.identity).GetComponent<DamageNumber>();
            num.SetDamage($"-{damage}");
            num.SetSize(5.5f);
        }

        private void DamageTarget(Entity target, int damage)
        {
            target.TakeDamage(damage);
            if (target.isHuman) target.GetComponent<Human>().isDefending = false;
            PortraitAnim(displayPortrait, "DMG");
        }

        private bool IfTargetDied(Entity target)
        {
            target.TakeDamage(target.CurrentHealth);
            if (target.isHuman)
            {
                gridSystem.humans.Remove(target.Position);
                deadHumans.Add(target.Name);
                if (LossCheck()) return true;
            }
            else
            {
                gridSystem.demons.Remove(target.Position);
                if (WinCheck()) return true;
            }
            UpdateUIOnDeath(target);
            target.Kill();
            return false;
        }

        private void UpdateUIOnDeath(Entity target)
        {
            UpdateCharacterDisplay(true, target, false);
            PortraitAnim(displayPortrait, "DEATH");
        }

        private bool LossCheck()
        {
            if (gridSystem.humans.Count == 0)
            {
                GameDone?.Invoke();
                uiStates.TogglePanel(0);
                UpdateCharacterDisplay(false, null, false);
                GetComponent<PlayerAttackSystem>().SetTutorialPaused(true);
                return true;
                        
            }

            return false;
        }

        private bool WinCheck()
        {
            if (gridSystem.demons.Count == 0)
            {
                foreach (string namn in deadHumans)
                {
                    nameSystem.Kill(namn);
                }
                GameDone?.Invoke();
                uiStates.TogglePanel(1);
                UpdateCharacterDisplay(false, null, false);
                GetComponent<PlayerAttackSystem>().SetTutorialPaused(true);
                return true;
            }

            return false;
        }

        private void AllAttacked()
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

        public void EndTurn()
        {
            if (gridSystem.demons.Count <= 0 || gridSystem.humans.Count <= 0 ) return;
            foreach (Vector2Int pos in gridSystem.humans)
            {
                if (gridSystem.GetTile(pos).linkedEntity.GetComponent<Human>().isMoving) return;
            }
            selectHighlight.position = Vector3.down * 100;
            isPlayer1Turn = !isPlayer1Turn;
            if (isPlayer1Turn)
            {
                Player1TurnStart?.Invoke();
            }
            else
            {
                Player2TurnStart?.Invoke();
            }
        }


        public void UpdateCharacterDisplay(bool showDisplay, Entity entity, bool instant)
        {
            infoDisplay.SetActive(showDisplay);
            if (showDisplay)
            {
                displayStats.text =
                    $"{entity.MinDamage}-{entity.MaxDamage}\n{entity.CurrentHealth}/{entity.MaxHealth}\n{(entity.IsMelee ? "MELEE" : "RANGED")}";
                displayName.text = $"{entity.Name}\n{entity.Age}";
                displayDescription.text = entity.Description;

                displayPortrait.sprite = entity.face;
                
                HealthDisplayCalculator(currentHealth: entity.CurrentHealth, maxHealth: entity.MaxHealth, instant: instant);
                
            }

            if (showDisplay)
            {
               UIManager.instance.RunThis("InfoBox.InfoBoxEnable");
            }
            else
            {
                UIManager.instance.RunThis("InfoBox.InfoBoxDisable");
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

        private void HealthDisplayCalculator(float currentHealth, float maxHealth, bool instant)
        {
            if (instant)
            {
                if (currentHealth <= 0)
                {
                    displayHealthSlider.value = 0;
                }

                if (displayHealthSlider.maxValue != maxHealth)
                {
                    displayHealthSlider.maxValue = maxHealth;
                }

                if (displayHealthSlider.value != currentHealth)
                {
                    displayHealthSlider.value = currentHealth;
                }
            }
            else
            {
                if (currentHealth <= 0)
                {
                    displayHealthSlider.DOValue(0, 0.1f);
                }

                if (displayHealthSlider.maxValue != maxHealth)
                {
                    displayHealthSlider.maxValue = maxHealth;
                }

                if (displayHealthSlider.value != currentHealth)
                {
                    displayHealthSlider.DOValue(currentHealth, 0.1f);
                }
            }
            
        }


        private static void PortraitAnim(Image face, string type)
        {
            switch (type)
            {
                case "DMG":
                    face.DOColor(Color.red, 0.2f).OnKill(() => { face.DOColor(Color.white, 0.2f); });
                    face.rectTransform.DOShakeScale(0.2f, 0.3f).SetLoops(1, LoopType.Yoyo);
                    face.rectTransform.DOShakeRotation(0.2f, 0.3f).SetLoops(1, LoopType.Yoyo);
                    face.rectTransform.DOShakePosition(0.2f, 0.1f, 1).SetLoops(1, LoopType.Yoyo);
                    break;
                case "DEATH":
                    face.DOColor(Color.red, 0.5f).OnKill(() => { face.DOColor(Color.white, 0.2f); });
                    break;
                case "MOVE":
                    break;
            }
            
            
        }
        
        
        public void ToggleNightLightOnHumans(bool toNight)
        {
            
            foreach (Vector2Int entity in gridSystem.humans)
            {
                Human h = gridSystem.GetTile(entity).linkedEntity as Human;
                if (h != null && h.CurrentHealth > 0)
                {
                    h.NightLightToggle(toNight);
                }
            }
        }



        private  IEnumerator ShootArrowAfterDelay(Entity attacker, Entity target, float delay)
        {
            yield return new WaitForSeconds(delay); 
            FMODManager.instance.OneShot("BowString", attacker.Position );
            ShootArrow(attacker, target);  
        }

        private void ShootArrow(Entity attacker, Entity target)
        {
            var arrow = Instantiate(attacker.arrowPrefab, attacker.transform.position, Quaternion.identity);
            var arrowSound = arrow.GetComponent<ArrowSound>();

            const float minArch = 0.2f;
            const float maxArch = 1f;
            var attackerPos = attacker.transform.position;
            var targetPos = target.transform.position;
            var distance = Vector3.Distance(attackerPos, targetPos);
            var clampedDistance = Mathf.Clamp(distance, minArch, maxArch);

            var arcHeight = Mathf.Lerp(minArch, maxArch, Mathf.InverseLerp(minArch, maxArch, clampedDistance));


            var middlePos = (attackerPos + targetPos) * 0.5f;


            middlePos.y += arcHeight;


            var point1 = Vector3.Lerp(attackerPos, middlePos, 0.5f);
            var point2 = Vector3.Lerp(middlePos, targetPos, 0.5f);


            Vector3[] arrowPath = { point1, point2, targetPos, };

            Debug.DrawLine(attackerPos, point1, Color.red, 10f);
            Debug.DrawLine(point1, middlePos, Color.yellow, 10f);
            Debug.DrawLine(middlePos, point2, Color.cyan, 10f);
            Debug.DrawLine(point2, targetPos, Color.blue, 10f);


            var duration = Mathf.Clamp(distance * 0.05f, 0.2f, 0.5f);
            arrowSound.ArrowDuration(duration);


            arrow.transform.DOPath(arrowPath, duration, PathType.CatmullRom, PathMode.TopDown2D, gizmoColor: Color.red)
                .SetEase(Ease.InOutSine).OnKill(() =>
                {
                    AttackLogic(attacker, target);
                    Destroy(arrow);
                });
        }

        public void Ability(Entity attacker, int abilityNumber, Tile targetTile)
        {
            Debug.Log(abilityNumber + "penis");
        }

        public void AOEAttack(Entity attacker, Tile targetTile)
        {
            
        }


        
        

    }
    
    
    
    
}
