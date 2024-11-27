using UnityEngine;
using TMPro;

public class BattleController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI displayStats;
    [SerializeField] private TextMeshProUGUI displayName;
    
    public void UpdateCharacterDisplay(Entity entity)
    {
        displayStats.text = $"{entity.Damage}\n{entity.CurrentHealth}/{entity.MaxHealth}";
        displayName.text = entity.Name;
    }
}
