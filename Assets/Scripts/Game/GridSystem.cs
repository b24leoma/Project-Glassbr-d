using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using Vector2 = UnityEngine.Vector2;
using Vector3Int = UnityEngine.Vector3Int;

public class GridSystem : MonoBehaviour
{
    private Tilemap tilemap;
    [SerializeField] private Transform entityParent;
    [SerializeField] private GameObject human;
    [SerializeField] private GameObject demon;
    [SerializeField] private LineRenderer pathLine;
    [SerializeField] private GameObject attackOverlay;
    private Dictionary<Vector2, Tile> tiles;
    private Vector3 size;
    private Entity highligtedEntity;
    private List<Entity> characters;
    
    private Vector2Int hoveredTile;
    private bool validHoveredTile;
    private Vector2Int selectPos;
    private bool isMoving;
    private bool isAttacking;

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
        CreateEntity(new Vector2(-2, 1), true);
        CreateEntity(new Vector2(-3, 0), true);
        CreateEntity(new Vector2(3,0), false);
        CreateEntity(new Vector2(2,1), false);
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
            if (e != null)
            {
                if (e.isHuman)
                {
                    isMoving = true;
                    pathLine.positionCount = 1;
                    pathLine.SetPosition(0, new Vector3(hoveredTile.x - 0.5f, hoveredTile.y - 0.5f, -5f));
                }
                    selectPos = hoveredTile;
                    tilemap.SetColor(new Vector3Int(hoveredTile.x - 1, hoveredTile.y - 1, 0),
                        new Color(0.5f, 0.5f, 0.5f));
            }
        }
        else if (context.canceled && isMoving)
        {
            if (hoveredTile == selectPos) return;
            Vector3 pos = new Vector3(hoveredTile.x - 0.5f, hoveredTile.y - 0.5f, -5);
            
            //Can move to tile
            if (isAttacking || (tiles[hoveredTile].linkedEntity == null &&
                                pos == pathLine.GetPosition(pathLine.positionCount - 1) &&
                                Vector2.Distance(hoveredTile, highligtedEntity.Position - new Vector2(-0.5f, -0.5f)) <
                                highligtedEntity.Range))
            {
                HighlightSquaresInRange(highligtedEntity, Color.white);
                MoveUnit(selectPos, new Vector2Int((int)(pathLine.GetPosition(pathLine.positionCount-1).x + 0.5f), (int)(pathLine.GetPosition(pathLine.positionCount-1).y + 0.5f)));
                HighlightSquaresInRange(highligtedEntity, new Color(0.8f, 0.8f, 0.8f));
                if (isAttacking)
                {
                    attackOverlay.SetActive(false);
                    isAttacking = false;
                    Debug.Log("Attacking");
                }
            }

            isMoving = false;
            pathLine.positionCount = 0;
        }
    }

    public void TileDetect(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

        tilemap.SetColor(new Vector3Int(hoveredTile.x - 1, hoveredTile.y - 1, 0), Color.white);
        if (highligtedEntity != null && !isMoving) HighlightSquaresInRange(highligtedEntity, Color.white);
        //if (isMoving && highligtedEntity !=)

        if (hit.collider != null)
        {
            Vector2 pos = Vector2.Scale(transform.InverseTransformPoint(hit.point), transform.localScale);
            hoveredTile = new Vector2Int((int)Mathf.Round(pos.x + 0.5f), (int)Mathf.Round(pos.y + 0.5f));
            validHoveredTile = true;
        }
        else
        {
            validHoveredTile = false;
        }



        if (!isMoving)
        {
            highligtedEntity = tiles[hoveredTile].linkedEntity;
            if (highligtedEntity != null) HighlightSquaresInRange(highligtedEntity, Color.white);
        }

        if (highligtedEntity != null)
        {
            HighlightSquaresInRange(highligtedEntity, new Color(0.8f, 0.8f, 0.8f));
        }

        if (isMoving) //Highlighted squares when moving character
        {
            if (Vector2.Distance(hoveredTile, selectPos) < highligtedEntity.Range)
            {
                if (pathLine.positionCount > 0 || isAttacking)
                {
                    Vector3 pos = pathLine.GetPosition(pathLine.positionCount - 1);
                    tilemap.SetColor(new Vector3Int((int)(pos.x - 0.5f), (int)(pos.y - 0.5f), 0),
                        new Color(0.5f, 0.5f, 0.5f));
                }

                //Add to path
                if (validHoveredTile && Vector3.Distance(new Vector3(hoveredTile.x - 0.5f, hoveredTile.y - 0.5f, -5f),
                        pathLine.GetPosition(pathLine.positionCount - 1)) <= 1 &&
                    tiles[hoveredTile].linkedEntity == null && pathLine.GetPosition(pathLine.positionCount - 1) !=
                    new Vector3(hoveredTile.x - 0.5f, hoveredTile.y - 0.5f, -5))
                {
                    pathLine.positionCount++;
                    pathLine.SetPosition(pathLine.positionCount - 1,
                        new Vector3(hoveredTile.x - 0.5f, hoveredTile.y - 0.5f, -5f));
                }
            }

            tilemap.SetColor(new Vector3Int(selectPos.x - 1, selectPos.y - 1, 0), new Color(0.5f, 0.5f, 0.5f));

            if (tiles[hoveredTile].linkedEntity != null)
            {
                if (!tiles[hoveredTile].linkedEntity.isHuman && hoveredTile != selectPos &&
                    Vector3.Distance(pathLine.GetPosition(pathLine.positionCount - 1),
                        new Vector3(hoveredTile.x - 0.5f, hoveredTile.y - 0.5f, -5)) <= 1)
                {
                    attackOverlay.SetActive(true);
                    isAttacking = true;
                    attackOverlay.transform.position = new Vector3(hoveredTile.x - 0.5f, hoveredTile.y - 0.5f, 0);

                }
            }
            else
            {
                attackOverlay.SetActive(false);
                isAttacking = false;
            }
        }
        else //Highlighted when not moving character
        {
            if (pathLine.positionCount > 0)
            {
                tilemap.SetColor(new Vector3Int(hoveredTile.x - 1 , hoveredTile.y - 1, 0), new Color(0.8f, 0.8f, 0.8f));
            }
        }

    }

    void HighlightSquaresInRange(Entity entity, Color color)
    {
        for (int i = -(int)size.x; i <= size.x; i++)
        {
            for (int j = -(int)size.y; j <= size.y; j++)
            {
                if (Vector2.Distance(entity.Position - new Vector2(-0.5f,-0.5f), new Vector2(i, j)) < entity.Range)
                {
                    tilemap.SetColor(new Vector3Int(i - 1, j - 1, 0), color);   
                }
            }
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
