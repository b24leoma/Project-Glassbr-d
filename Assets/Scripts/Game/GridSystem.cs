using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class GridSystem : MonoBehaviour
{
    private Tilemap tilemap;
    [SerializeField] private Transform EntityParent;
    [SerializeField] private GameObject Human;
    [SerializeField] private GameObject Demon;
    private Vector2Int hoveredTile;
    private Dictionary<Vector2, Tile> tiles;
    private List<Entity> characters;

    private Vector2Int selectPos;
    private bool isMoving;

    private void Start()
    {
        tiles = new Dictionary<Vector2, Tile>();
        characters = new List<Entity>();
        Vector3 size = GetComponent<Tilemap>().size;
        size.x = size.x / 2 - 0.5f;
        size.y = size.y / 2 - 0.5f;
        for (int i = -(int)size.x; i <= size.x; i++)
        {
            for (int j = -(int)size.y; j <= size.y; j++)
            {
                tiles[new Vector2(i, j)] = new Tile();
            }
        }
        Debug.Log(tiles.Count);
        CreateEntity(Vector2.zero, true);
        tilemap = GetComponent<Tilemap>();
    }

    private void Update()
    {
        if (isMoving)
        {
            Debug.DrawLine(new Vector3(selectPos.x, selectPos.y, -5), new Vector3(hoveredTile.x, hoveredTile.y, -5));
        }
    }

    void CreateEntity(Vector2 pos, bool isHuman)
    {
        Entity e = Instantiate(Human, Vector3.zero, quaternion.identity, EntityParent).GetComponent<Entity>();
        tiles[pos].linkedEntity = e;
        e.MoveToTile(pos);
        characters.Add(e);
    }
    
    public void SelectTile(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            selectPos = hoveredTile;
            Debug.Log(selectPos);
            isMoving = true;
            tilemap.SetColor(new Vector3Int(selectPos.x - 1, selectPos.y - 1, 0),
                new Color(0.5f, 0.5f, 0.5f));
        }
        else if (context.canceled)
        {
            MoveUnit(selectPos, hoveredTile);
            Debug.Log(selectPos);
            isMoving = false;
            tilemap.SetColor(new Vector3Int(selectPos.x - 1, selectPos.y - 1, 0), Color.white);
        }
    }

    public void TileDetect(InputAction.CallbackContext context)
    {
        if(!isMoving || selectPos != hoveredTile) tilemap.SetColor(new Vector3Int(hoveredTile.x-1, hoveredTile.y-1, 0), Color.white);;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        if (hit.collider != null)
        {
            Vector2 pos = Vector2.Scale(transform.InverseTransformPoint(hit.point),transform.localScale);
            hoveredTile = new Vector2Int((int)Mathf.Round(pos.x + 0.5f), (int)Mathf.Round(pos.y + 0.5f));
            tilemap.SetTileFlags(new Vector3Int(hoveredTile.x-1, hoveredTile.y-1, 0), TileFlags.None); 
            tilemap.SetColor(new Vector3Int(hoveredTile.x - 1, hoveredTile.y - 1, 0), 
                new Color(0.5f, 0.5f, 0.5f));
        }

    }

    void MoveUnit(Vector2 currentPos, Vector2 newPos)
    {
        if(tiles[currentPos] == null || tiles[currentPos].linkedEntity == null) return;
        Entity e = tiles[currentPos].linkedEntity;
        tiles[currentPos].linkedEntity = null;
        e.MoveToTile(newPos);
        tiles[newPos].linkedEntity = e;
    }
}
