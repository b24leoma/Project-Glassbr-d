using UnityEngine;

public class TeamAssigner : MonoBehaviour
{
    [SerializeField] private UnitMoveManager unitMoveManager;
    [SerializeField] private MapManager mapManager;

    public void TryAssignTeam()
    {
        if (unitMoveManager == null)
        {
            Debug.LogWarning("No UnitMoveManager assigned!");
        }

        if (mapManager == null)
        {
            Debug.LogWarning("No MapManager assigned!");
        }

        AssignTeams();
    }

    void AssignTeams()
    {
        foreach (var (unitPosition, unitObject) in unitMoveManager.UnitPositions)
        {
            var unitData = unitObject.GetComponent<UnitInGameData>();

            if (unitData == null)
            {
                Debug.LogError($"Unit at {unitPosition} is missing the UnitInGameData component.");
                continue;
            }

            unitData.unitTeam = mapManager.TopTiles.ContainsKey(unitPosition) ? Team.Team2 : Team.Team1;
           
        }
    }
}