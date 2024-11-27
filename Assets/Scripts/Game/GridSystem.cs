using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using Vector2 = UnityEngine.Vector2;
using Vector3Int = UnityEngine.Vector3Int;

public class GridSystem : MonoBehaviour
{
    [SerializeField] private List<Spawning> spawnList;
    
    private Tilemap tilemap;
    [SerializeField] private Transform entityParent;
    [SerializeField] private GameObject human;
    [SerializeField] private GameObject demon;
    [SerializeField] private LineRenderer pathLine;
    [SerializeField] private GameObject attackOverlay;
    private Dictionary<Vector2, Tile> tiles;
    private Vector3 size;
    private Entity highlightedEntity;
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
                tilemap.SetTileFlags(new Vector3Int(i - 1, j - 1, 0), TileFlags.None);
            }
        }

        foreach (Spawning spawn in spawnList)
        {
            CreateEntity(spawn.Position, spawn.isHuman);
        }
    }

    void CreateEntity(Vector2 pos, bool isHuman)
    {
        Entity e = Instantiate(isHuman ? human : demon, Vector3.zero, quaternion.identity, entityParent)
            .GetComponent<Entity>();
        tiles[pos].linkedEntity = e;
        e.MoveToTile(pos);
        characters.Add(e);
    }

    public void SelectTile(InputAction.CallbackContext context)
    {
        if (context.started) // Click started
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
        else if (context.canceled && isMoving) // Click finished
        {
            if (hoveredTile == selectPos) return;
            Vector3 pos = new Vector3(hoveredTile.x - 0.5f, hoveredTile.y - 0.5f, -5);

            //Can move to tile
            if (isAttacking || (tiles[hoveredTile].linkedEntity == null &&
                                pos == pathLine.GetPosition(pathLine.positionCount - 1) &&
                                Vector2.Distance(hoveredTile, highlightedEntity.Position - new Vector2(-0.5f, -0.5f)) <
                                highlightedEntity.Range))
            {
                HighlightSquaresInRange(highlightedEntity, Color.white);
                MoveUnit(selectPos,
                    new Vector2Int((int)(pathLine.GetPosition(pathLine.positionCount - 1).x + 0.5f),
                        (int)(pathLine.GetPosition(pathLine.positionCount - 1).y + 0.5f)));
                HighlightSquaresInRange(highlightedEntity, new Color(0.8f, 0.8f, 0.8f));
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

        //Reset highlights
        tilemap.SetColor(new Vector3Int(hoveredTile.x - 1, hoveredTile.y - 1, 0), Color.white);
        if (highlightedEntity != null && !isMoving) HighlightSquaresInRange(highlightedEntity, Color.white);


        if (hit.collider != null)
        {
            Vector2 pos = transform.InverseTransformPoint(hit.point);
            hoveredTile = new Vector2Int((int)Mathf.Round(pos.x + 0.5f), (int)Mathf.Round(pos.y + 0.5f));
            validHoveredTile = true;
        }
        else
        {
            validHoveredTile = false;
        }

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
            else
            {
                attackOverlay.SetActive(false);
                isAttacking = false;
            }

            if (isMoving) //Highlighted squares when moving character
            {
                if (Vector2.Distance(hoveredTile, selectPos) < highlightedEntity.Range)
                {
                    if (pathLine.positionCount > 0 || isAttacking)
                    {
                        Vector3 pos = pathLine.GetPosition(pathLine.positionCount - 1);
                        tilemap.SetColor(new Vector3Int((int)(pos.x - 0.5f), (int)(pos.y - 0.5f), 0),
                            new Color(0.5f, 0.5f, 0.5f));
                    }

                    //Add to path
                    if (validHoveredTile && Vector3.Distance(
                            new Vector3(hoveredTile.x - 0.5f, hoveredTile.y - 0.5f, -5f),
                            pathLine.GetPosition(pathLine.positionCount - 1)) <= 1 &&
                        (tiles[hoveredTile].linkedEntity == null ||
                         tiles[hoveredTile].linkedEntity == highlightedEntity) &&
                        pathLine.GetPosition(pathLine.positionCount - 1) !=
                        new Vector3(hoveredTile.x - 0.5f, hoveredTile.y - 0.5f, -5))
                    {
                        bool valid = true;
                        for (int i = 0; i < pathLine.positionCount; i++)
                        {
                            if (pathLine.GetPosition(i) == new Vector3(hoveredTile.x - 0.5f, hoveredTile.y - 0.5f, -5))
                            {
                                valid = false;
                                pathLine.positionCount = i + 1;
                            }
                        }

                        if (valid)
                            pathLine.positionCount++;
                        pathLine.SetPosition(pathLine.positionCount - 1,
                            new Vector3(hoveredTile.x - 0.5f, hoveredTile.y - 0.5f, -5f));
                    }
                }

                tilemap.SetColor(new Vector3Int(selectPos.x - 1, selectPos.y - 1, 0), new Color(0.5f, 0.5f, 0.5f));
            }
            else
            {
                highlightedEntity = tiles[hoveredTile].linkedEntity;
                if (highlightedEntity != null) HighlightSquaresInRange(highlightedEntity, Color.white);
            }

            if (highlightedEntity != null)
            {
                HighlightSquaresInRange(highlightedEntity, new Color(0.8f, 0.8f, 0.8f));
            }
        }
    }

    void HighlightSquaresInRange(Entity entity, Color color)
    {
        for (int i = -(int)size.x; i <= size.x; i++)
        {
            for (int j = -(int)size.y; j <= size.y; j++)
            {
                if (Vector2.Distance(entity.Position - new Vector2(-0.5f, -0.5f), new Vector2(i, j)) < entity.Range)
                {
                    tilemap.SetColor(new Vector3Int(i - 1, j - 1, 0), color);
                }
            }
        }
    }

    void MoveUnit(Vector2 currentPos, Vector2 newPos)
    {
        if (tiles[currentPos] == null || tiles[currentPos].linkedEntity == null) return;
        Entity e = tiles[currentPos].linkedEntity;
        tiles[currentPos].linkedEntity = null;
        e.MoveToTile(newPos);
        tiles[newPos].linkedEntity = e;
    }
}
[System.Serializable] public class Spawning
{
    public Vector2 Position;
    public bool isHuman;
}

