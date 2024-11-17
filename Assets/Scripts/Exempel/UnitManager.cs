using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WilhelmsJank
{


    public class UnitManager : MonoBehaviour
    {
        [SerializeField] private MapManager mapManager;
        [SerializeField] private Tilemap tilemap;
        [SerializeField] GameObject unitPrefab;

        public Dictionary<Vector3Int, GameObject> UnitPositions = new Dictionary<Vector3Int, GameObject>();

        //AI förfining för jag är lat :)

        public void PlaceUnitsOnWalkableTiles()
        {

            if (mapManager.dataFromTiles == null)
            {
                Debug.LogError("dataFromTiles is null.");
                return;
            }

            // Loop through each tile in the dataFromTiles dictionary
            foreach (var tileEntry in mapManager.dataFromTiles)
            {
                TileBase tile = tileEntry.Key;
                TileData tileData = tileEntry.Value;

                // If the tile is walkable, find its position and place a unit
                if (tileData.canWalk)
                {
                    // Loop through all positions within the Tilemap's bounds
                    foreach (var tilePosition in tilemap.cellBounds.allPositionsWithin)
                    {
                        if (tilemap.GetTile(tilePosition) == tile) // Check if the tile at this position is the same
                        {
                            // Place the unit on this tile position
                            if (unitPrefab != null) PlaceUnitOnTile(tilePosition, unitPrefab);
                        }
                    }
                }
            }
        }

        // Example method to place a unit on a tile
        void PlaceUnitOnTile(Vector3Int tilePosition, GameObject unitPrefab)
        {
            // Check if the unitPrefab is assigned
            if (unitPrefab == null)
            {
                Debug.LogError("Unit prefab is not assigned.");
                return;
            }

            Vector3 worldPosition = tilemap.CellToWorld(tilePosition);

            Vector3 tileSize = tilemap.cellSize;

            worldPosition.x += tileSize.x / 2;
            worldPosition.y += tileSize.y / 2;
            // Check if there's already a unit on this tile
            if (UnitPositions.ContainsKey(tilePosition))
            {
                Debug.Log("Tile is already occupied by another unit.");
                return;
            }

            // Instantiate the unit at the tile's position
            GameObject unit = Instantiate(unitPrefab, worldPosition, Quaternion.identity);

            // Add the unit to the dictionary
            UnitPositions[tilePosition] = unit;
        }
    }
}