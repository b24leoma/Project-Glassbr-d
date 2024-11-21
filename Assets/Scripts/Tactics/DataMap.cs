using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DataMap : MonoBehaviour
{
    [SerializeField] private Tilemap dataMap;

    [SerializeField] private float maxDataValue;

    private Dictionary<Vector3Int, float> dataTiles = new Dictionary<Vector3Int, float>();

    private void ChangeDataValue(Vector3Int gridPosition, float changeBy)
    {
        if  (!dataTiles.ContainsKey(gridPosition))
            dataTiles.Add(gridPosition, 0f);
        float newValue = dataTiles[gridPosition] + changeBy;

        if (newValue <= 0)
        {
            dataTiles.Remove(gridPosition);
        }
        else
        {
            dataTiles[gridPosition] = Mathf.Clamp(newValue, 0f, maxDataValue);
        }
    }

}
