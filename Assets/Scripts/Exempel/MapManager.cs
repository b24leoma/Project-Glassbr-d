using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace WilhelmsJank
{
   // Baserat på https://www.youtube.com/watch?v=XIqtZnqutGg&t=4s
    public class MapManager : MonoBehaviour
    {
        [SerializeField] private Tilemap map;
        [SerializeField] private List<TileData> tileData;


        public Dictionary<TileBase, TileData> dataFromTiles;

        private void Awake()
        {
            dataFromTiles = new Dictionary<TileBase, TileData>();

            foreach (var tileDataEntry in tileData)
            {
                foreach (var tile in tileDataEntry.tiles)
                {
                    if (!dataFromTiles.TryAdd(tile, tileDataEntry))
                    {
                        Debug.LogWarning($"Duplicate: {tile} found. Skipping.");
                    }
                }
            }
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (Camera.main != null)
                {
                    Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    Vector3Int gridPosition = map.WorldToCell(mousePosition);

                    TileBase clickedTile = map.GetTile(gridPosition);
                    if (clickedTile is null)
                    {
                        {
                            Debug.Log("No tile found");
                        }
                    }
                    else
                    {
                        float modifierMovement = dataFromTiles[clickedTile].modifierMovement;
                        bool canWalk = dataFromTiles[clickedTile].canWalk;

                        Debug.Log("Rutan är " + clickedTile + " på " + gridPosition + "      [canWalk: " + canWalk +
                                  "] [modifierMovement : " + modifierMovement + "]");
                    }
                }
            }
        }







        //exempel
        public TileData GetTileData(TileBase tile)
        {
            if (dataFromTiles.TryGetValue(tile, out var data))
            {
                return data;
            }

            Debug.Log("No tile found.");

            return null;
        }
    }
}