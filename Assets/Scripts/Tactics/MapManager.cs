using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [SerializeField] private Tilemap map;
    [SerializeField] private List<TileData> tileData;

    [SerializeField] TurnBasedController turnBasedController;
    [FormerlySerializedAs("unitManager")] [SerializeField] UnitMoveManager unitMoveManager;

    public Dictionary<TileBase, TileData> DataFromTiles { get; private set; }
    public HashSet<Vector3Int> WalkableTilePositions { get; private set; }
    public TileBase CurrentTile { get; private set; }
    public Vector3Int CurrentTilePosition { get; private set; }
    
    
    
    public Dictionary<Vector3Int, TileBase> TopTiles { get; private set; }
    private string _isTopOrBottom;
    private Vector3Int _middlePosition;


    private void Awake()
    {
        DataFromTiles = new Dictionary<TileBase, TileData>();
        WalkableTilePositions = new HashSet<Vector3Int>();
        TopTiles = new Dictionary<Vector3Int, TileBase>();
        Vector3 middlePosition = map.cellBounds.center;
        Vector3 adjustedMiddlePosition = middlePosition + new Vector3(0 , -1 , 0);   // IT JUST WORKS! 

        foreach (var tileDataInfo in tileData)
        {
            foreach (var tile in tileDataInfo.tiles)
            {
                // Add the tile to the DataFromTiles dictionary
                if (!DataFromTiles.TryAdd(tile, tileDataInfo))
                {
                    Debug.LogWarning($"Duplicate: {tile} found. Skipping.");
                }
            }
            
        }
        
        foreach (var pos in map.cellBounds.allPositionsWithin)
        {
            TileBase mapTile = map.GetTile(pos);

            if (mapTile != null && DataFromTiles.TryGetValue(mapTile, out TileData tileInfo))
            {
                
                if (tileInfo.canWalk)
                {
                    WalkableTilePositions.Add(pos);
                }

                if (pos.y > adjustedMiddlePosition.y)
                {
                    TopTiles.Add(pos, mapTile);
                }
            }
        }
    }

    public void GetTile()
    {
        if (Camera.main != null)
        {
            Vector2 mouseCameraPosition = turnBasedController.PointerPosition;

            Vector3 mouseWorldPosition =
                Camera.main.ScreenToWorldPoint(new Vector3(mouseCameraPosition.x, mouseCameraPosition.y, 0));

            Vector3Int gridPosition = map.WorldToCell(mouseWorldPosition);

            CurrentTilePosition = gridPosition;

            TileBase targetTile = map.GetTile(gridPosition);
            if (targetTile is null)
            {
                Debug.Log("No tile found");

                return;
            }

            
            bool isTop = TopTiles.ContainsKey(gridPosition);
            _isTopOrBottom = isTop ? " (TOP) " : " (BOTTOM) ";
            float modifierMovement = DataFromTiles[targetTile].modifierMovement;
            bool canWalk = DataFromTiles[targetTile].canWalk;
            CurrentTile = targetTile;
            Debug.Log("[Tileinfo: " + targetTile + "  " + gridPosition + "      [canWalk: " + canWalk +
                      "] [modifierMovement : " + modifierMovement + "]" + _isTopOrBottom);

        }

    }


    public void SelectionInput()
    {
        GetTile();

        if (unitMoveManager.UnitPositions.ContainsKey(CurrentTilePosition))
        {
            unitMoveManager.SelectUnit(CurrentTilePosition);

        }
        else
        {
            Debug.Log("Tile has no unit.");
            unitMoveManager.hasSelectedUnit = false;
        }
    }

    public void MoveInput()
    {
        GetTile();
        if (unitMoveManager.hasSelectedUnit)
        {
            unitMoveManager.OnTryMoveUnit();
        }
        else
        {
            Debug.Log("No unit selected.");

        }
    }
}