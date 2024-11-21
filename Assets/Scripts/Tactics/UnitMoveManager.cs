using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class UnitMoveManager : MonoBehaviour
{
    [SerializeField] private MapManager mapManager;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] GameObject unitPrefab;
    [SerializeField] private TeamAssigner teamAssigner;
    [SerializeField] private UnitCombatManager unitCombatManager;


    
 

   
    public Dictionary<Vector3Int, GameObject> UnitPositions { get; } = new Dictionary<Vector3Int, GameObject>();
    public bool hasSelectedUnit;
    private Vector3Int _selectedUnitTilePosition;
    private string _selectedUnitName;
    public Dictionary<Team, HashSet<Vector3Int>> TeamPositions { get; } = new Dictionary<Team, HashSet<Vector3Int>>();
   

   
    
    //Example that populates the whole map
    public void PlaceUnitsOnWalkableTiles()
    {
        foreach (var tilePosition in mapManager.WalkableTilePositions)
        {
            PlaceUnitOnTile(tilePosition, unitPrefab);
        }
        teamAssigner.TryAssignTeam();
        
    }

    public void SpawnUnitHere()
    {
        PlaceUnitOnTile(mapManager.CurrentTilePosition, unitPrefab);
    }
    
    //Main Unit logic, add support for different kinds of unitPrefabs!!!
    void PlaceUnitOnTile(Vector3Int tilePosition, GameObject unitPrefab)
    {
        if (mapManager.WalkableTilePositions.Contains(tilePosition))
        {
            if (unitPrefab == null)
            {
                Debug.LogError("Unit prefab is not assigned.");
                return;
            }

            if (UnitPositions.ContainsKey(tilePosition))
            {
                Debug.Log("Tile is already occupied by another unit.");
                return;
            }

            Vector3 worldPosition = TileUtil.TileCenter(tilemap, tilePosition);
            GameObject unit = Instantiate(unitPrefab, worldPosition, Quaternion.identity);
            
            var unitData = unit.GetComponent<UnitInGameData>();
            if (unitData != null)
            { 
                if (!TeamPositions.ContainsKey(unitData.unitTeam))
                {
                    TeamPositions[unitData.unitTeam] = new HashSet<Vector3Int>();
                }
                
                TeamPositions[unitData.unitTeam].Add(tilePosition);
            }
            else
            {
                Debug.LogWarning("Unit doesn't have data.");
            }
            
            UnitPositions[tilePosition] = unit;
        }
    }


    public void SelectUnit(Vector3Int tilePosition)
    {
        if (UnitPositions.TryGetValue(tilePosition, out var selectedUnit))
        {
            var unitData = selectedUnit.GetComponent<UnitInGameData>(); 
            
            hasSelectedUnit = true;
            Debug.Log("Selected " + unitData.unitPersonalName + "   [Object name: " + selectedUnit + "  at " + tilePosition + " "+ unitData.unitTeam + "]");
            _selectedUnitTilePosition = tilePosition;
            _selectedUnitName = selectedUnit.name;
            
        }
    }
    
   
    
    
    public void RemoveUnitFromTile()
    {

        if (hasSelectedUnit)
        {
            if (UnitPositions.ContainsKey(_selectedUnitTilePosition))
            {
                var selectedUnit = UnitPositions[_selectedUnitTilePosition];
                var unitData = selectedUnit.GetComponent<UnitInGameData>();
                
                if (unitData != null)
                {
                    Team teamKey = unitData.unitTeam;
                    if (TeamPositions.TryGetValue(teamKey, out var position))
                    {
                        position.Remove(_selectedUnitTilePosition);
                    }
                }
                else
                {
                    Debug.LogWarning("Team info not found!");
                }


                Debug.Log("Removed unit " + _selectedUnitName + "at " + _selectedUnitTilePosition);
                
                Destroy(selectedUnit);
                UnitPositions.Remove(_selectedUnitTilePosition);
                
                
                
                hasSelectedUnit = false;
            }
          
        }
        else
        {
            Debug.Log("No unit selected.");
        }
    }


    public void OnTryMoveUnit()
    {
        TryMoveUnit(mapManager.CurrentTilePosition);
    }


    void TryMoveUnit(Vector3Int targetMovePosition)
    {
        
        if (!hasSelectedUnit)
        {
            Debug.Log ("No unit selected to move!");
            return;
        }
        
        var targetTile = tilemap.GetTile(targetMovePosition);
        if (targetTile == null)
        {
            Debug.Log("Tile is outside of the map.");
            return;
        }

        if (!mapManager.WalkableTilePositions.Contains(targetMovePosition))
        {
            Debug.Log("Can't move " + _selectedUnitName + " at " + _selectedUnitTilePosition + mapManager.CurrentTile);
            return;
        }

        if (UnitPositions.TryGetValue(targetMovePosition, out var targetUnit) )
        {
            var targetUnitData = targetUnit.GetComponent<UnitInGameData>();
            var targetTeamKey = targetUnitData.unitTeam;
            
            var selectedUnit = UnitPositions[_selectedUnitTilePosition];
            var selectedUnitData = selectedUnit.GetComponent<UnitInGameData>();
            var selectedTeamKey = selectedUnitData.unitTeam;
            
            if (targetTeamKey == selectedTeamKey)
            {
                Debug.Log("Target tile is already occupied by another unit.");
                return;
            }

            if (targetTeamKey != selectedTeamKey)
            {
                if (unitCombatManager != null)
                {
                    unitCombatManager.AttackUnit(selectedUnit, targetUnit);
                }

                if (unitCombatManager == null)
                {
                    Debug.LogWarning("Unit combat manager is missing, please assign it!");
                }
                return;
            }
        }
        
        //Add check to currently selected unit AP (movement points/action points)
        
        MoveUnit(targetMovePosition);

    }
    void MoveUnit(Vector3Int targetMovePosition)
    {
       var selectedUnit = UnitPositions[_selectedUnitTilePosition];
       
       UnitPositions.Remove(_selectedUnitTilePosition);
       UnitPositions[targetMovePosition] = selectedUnit;
       
       Vector3 centeredTargetMovePosition = TileUtil.TileCenter(tilemap, targetMovePosition);
       
       selectedUnit.transform.position = centeredTargetMovePosition;
       
       hasSelectedUnit = false;
        
        
    }
    
    
}
