using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class DataMap : MonoBehaviour
{
    [SerializeField] private Tilemap dataMap;

    [SerializeField] private float maxDataValue;
    
    [SerializeField] Color maxDataColor, minDataColor, clearDataColor;

    [SerializeField] private TurnBasedController turnBasedController;

    [SerializeField] private float AddDataValue;

    private Dictionary<Vector3Int, float> dataTiles = new Dictionary<Vector3Int, float>();



    public void AddData(Vector2 worldPosition, float dataAmount)
    {
        Vector3Int gridPosition = dataMap.WorldToCell(worldPosition);
        ChangeDataValue(gridPosition, dataAmount);
    }
    
    
    
    
    private void ChangeDataValue(Vector3Int gridPosition, float changeBy)
    {
        if  (!dataTiles.ContainsKey(gridPosition))
            dataTiles.Add(gridPosition, 0f);
        float newValue = dataTiles[gridPosition] + changeBy;

        if (newValue <= 0)
        {
            dataTiles.Remove(gridPosition);
            
            dataMap.SetTileFlags(gridPosition, TileFlags.None);
            dataMap.SetColor(gridPosition, clearDataColor);
            dataMap.SetTileFlags(gridPosition, TileFlags.LockColor);
        }
        else
        {
            dataTiles[gridPosition] = Mathf.Clamp(newValue, 0f, maxDataValue);
        }
    }

    private void VisualizeData()
    {
        foreach (var entry in dataTiles)
        {
            float dataPercent = entry.Value / maxDataValue;
            Color newTileColor = maxDataColor * dataPercent + minDataColor * (1f - dataPercent);

            dataMap.SetTileFlags(entry.Key, TileFlags.None);
            dataMap.SetColor(entry.Key, newTileColor);
            dataMap.SetTileFlags(entry.Key, TileFlags.LockColor);
        }
    }

    public void ClickedTile ()
    {
        Vector2 mouseCameraPosition = turnBasedController.PointerPosition;
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(mouseCameraPosition);
        
        AddData(mousePosition, AddDataValue);
        
        
        VisualizeData();


    }
}
