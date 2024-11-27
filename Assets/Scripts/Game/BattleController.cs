using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class BattleController : MonoBehaviour
{
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

        GetComponent<PlayerAttackSystem>();
        isPlayerTurn = true;
    }

    void CreateEntity(Vector2 pos, bool isHuman)
    {
        Entity e = Instantiate(isHuman ? human : demon, Vector3.zero, Quaternion.identity, entityParent)
            .GetComponent<Entity>();
        gridSystem.ConnectToTile(pos, e);
        characters.Add(e);
    }

    public void Attack(Entity attacker, Entity target)
    {
        target.TakeDamage(attacker.Damage);
        isPlayerTurn = !isPlayerTurn;
        
    }
    
    
    public void UpdateCharacterDisplay(Entity entity)
    {
        displayStats.text = $"{entity.Damage}\n{entity.CurrentHealth}/{entity.MaxHealth}";
        displayName.text = entity.Name;
    }
    
    
    
    public List<Entity> GetCharacters(){return characters;}
    public Entity GetCharacterAt(int i){return characters[i];}
    
    [System.Serializable] public class Spawning
    {
        public Vector2 Position;
        public bool isHuman;
    }
}
