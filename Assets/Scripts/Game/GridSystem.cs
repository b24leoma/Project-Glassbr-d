using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class GridSystem : MonoBehaviour
{
    private Tilemap tilemap;
    [SerializeField] private Transform entityParent;
    [SerializeField] private GameObject human;
    [SerializeField] private GameObject demon;
    private Dictionary<Vector2, Tile> tiles;
    private Vector3 size;
    private Entity highligtedEntity;
    private List<Entity> characters;
    
    private Vector2Int hoveredTile;
    private bool validHoveredTile;
    private Vector2Int selectPos;
    private bool isMoving;

    private void Start()
    {
        tilemap = GetComponent<Tilemap>();
        tiles = new Dictionary<Vector2, Tile>();
        characters = new List<Entity>();
       size = GetComponent<Tilemap>().size;
        size.x = size.x / 2 - 0.5f;
        size.y = size.y / 2 - 0.5f;
        for (int i = -(int)size.x; i <= size.x; i++)
        {
            for (int j = -(int)size.y; j <= size.y; j++)
            {
                tiles[new Vector2(i, j)] = new Tile();
                tilemap.SetTileFlags(new Vector3Int(i-1, j-1, 0), TileFlags.None); 
            }
        }
        Debug.Log(tiles.Count);
        CreateEntity(Vector2.zero, true);
    }

    void CreateEntity(Vector2 pos, bool isHuman)
    {
        Entity e;
        if (isHuman) e = Instantiate(human, Vector3.zero, quaternion.identity, entityParent).GetComponent<Entity>();
        else e = Instantiate(demon, Vector2.zero, quaternion.identity, entityParent).GetComponent<Entity>();
        tiles[pos].linkedEntity = e;
        e.MoveToTile(pos);
        characters.Add(e);
    }
    
    public void SelectTile(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Entity e = tiles[hoveredTile].linkedEntity;
            if (e != null && e.isHuman)
            {
                    selectPos = hoveredTile;
                    isMoving = true;
                        
            }
        }
        else if (context.canceled && isMoving)
        {
            isMoving = false;
            if (hoveredTile == selectPos) return;

            if (validHoveredTile)
            {
                MoveUnit(selectPos, hoveredTile);
                Debug.Log(selectPos + "->" + hoveredTile);
                for (int i = -(int)size.x; i <= size.x; i++)
                {
                    for (int j = -(int)size.y; j <= size.y; j++)
                    {
                        tilemap.SetColor(new Vector3Int(i - 1, j - 1, 0), Color.white);   
                    }
                }
            }
            isMoving = false;
            tilemap.SetColor(new Vector3Int(selectPos.x-1, selectPos.y-1, 0), Color.white);
            tilemap.SetColor(new Vector3Int(hoveredTile.x - 1, hoveredTile.y - 1, 0), 
                new Color(0.8f, 0.8f, 0.8f));
        }
    }

    public void TileDetect(InputAction.CallbackContext context)
    {
        if (isMoving)
        {
            if (isMoving && Vector2.Distance(hoveredTile, selectPos) > 5)
            {
                tilemap.SetColor(new Vector3Int(hoveredTile.x - 1, hoveredTile.y - 1, 0), new Color(0.8f, 0.8f, 0.8f));
            }
            else
            {
                tilemap.SetColor(new Vector3Int(hoveredTile.x - 1, hoveredTile.y - 1, 0), new Color(0.5f, 0.5f, 0.5f));
            }
            tilemap.SetColor(new Vector3Int(selectPos.x - 1, selectPos.y - 1, 0), new Color(0.5f, 0.5f, 0.5f));
               
        }
        tilemap.SetColor(new Vector3Int(hoveredTile.x - 1, hoveredTile.y - 1, 0), Color.white);

        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        if (hit.collider != null)
        {
            Vector2 pos = Vector2.Scale(transform.InverseTransformPoint(hit.point),transform.localScale);
            hoveredTile = new Vector2Int((int)Mathf.Round(pos.x + 0.5f), (int)Mathf.Round(pos.y + 0.5f));
            tilemap.SetColor(new Vector3Int(hoveredTile.x - 1, hoveredTile.y - 1, 0),
                isMoving ? new Color(0.5f, 0.5f, 0.5f) : new Color(0.8f, 0.8f, 0.8f));
            validHoveredTile = true;

            if (!isMoving)
            {
                highligtedEntity = tiles[hoveredTile].linkedEntity;   
            }
            if (highligtedEntity != null)
            {
                for (int i = -(int)size.x; i <= size.x; i++)
                {
                    for (int j = -(int)size.y; j <= size.y; j++)
                    {
                        if (Vector2.Distance(highligtedEntity.Position - new Vector2(-0.5f,-0.5f), new Vector2(i, j)) < highligtedEntity.Range)
                        {
                            tilemap.SetColor(new Vector3Int(i - 1, j - 1, 0), new Color(0.8f, 0.8f, 0.8f));   
                        }
                    }
                }
            }
            else
            {
                for (int i = -(int)size.x; i <= size.x; i++)
                {
                    for (int j = -(int)size.y; j <= size.y; j++)
                    {
                        if (Vector2.Distance(highligtedEntity.Position - new Vector2(-0.5f,-0.5f), new Vector2(i, j)) < highligtedEntity.Range)
                        {
                            tilemap.SetColor(new Vector3Int(i - 1, j - 1, 0), Color.white);   
                        }
                    }
                }
            }
        }
        else
        {
            validHoveredTile = false;
            highligtedEntity = null;
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
