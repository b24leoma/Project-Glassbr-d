using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttackSystem : MonoBehaviour
{
    [SerializeField] private GameObject actionButtonParent;
    [SerializeField] private LineRenderer pathLine;
    [SerializeField] private GameObject attackHighlightIcon;
    [SerializeField] private Transform iconParent;
    private Entity highlightedEntity;
    private Vector2Int hoveredTile;
    private bool validHoveredTile;
    private Vector2Int moveStartTile;
    private bool isMoving;
    private bool isAttacking;
    private BattleController battleController;
    private GridSystem gridSystem;
    private bool isPlayerTurn;

    void Start()
    {
        battleController = GetComponent<BattleController>();
        gridSystem = GetComponent<GridSystem>();
        isPlayerTurn = true;
    }

    public void TileClicked(InputAction.CallbackContext context)
    {
        if (context.started && isPlayerTurn) // Click pressed
        {
            Entity b = gridSystem.GetTile(hoveredTile).linkedEntity;
            if (b != null)
            {
                if (b.isHuman)
                {
                    isMoving = true;
                    pathLine.positionCount = 1;
                    pathLine.SetPosition(0, new Vector3(hoveredTile.x - 0.5f, hoveredTile.y - 0.5f, -5f));
                    moveStartTile = hoveredTile;
                    gridSystem.HighlightSquaresInRange(highlightedEntity, highlightedEntity.AttackRange,
                        new Color(0.8f, 0.8f, 0.8f));
                }
            }
        }
        else if (context.canceled) // Click released
        {
            if (isMoving)
            {
                gridSystem.HighlightSquaresInRange(highlightedEntity,
                    highlightedEntity.Range + highlightedEntity.AttackRange, Color.white);

                if (hoveredTile != moveStartTile)
                {
                    gridSystem.MoveUnit(moveStartTile,
                        new Vector2Int((int)(pathLine.GetPosition(pathLine.positionCount - 1).x + 0.5f),
                            (int)(pathLine.GetPosition(pathLine.positionCount - 1).y + 0.5f)));
                }

                if (isAttacking)
                {
                    battleController.Attack(highlightedEntity, gridSystem.GetTile(hoveredTile).linkedEntity);
                    battleController.UpdateCharacterDisplay(gridSystem.GetTile(hoveredTile).linkedEntity);

                }
                isAttacking = false;
                isMoving = false;
                pathLine.positionCount = 1;
                gridSystem.SetColor(new Vector3Int(hoveredTile.x - 1, hoveredTile.y - 1, 0),
                    new Color(0.8f, 0.8f, 0.8f));
            }
        }
        MouseMove(context);
    }

    public void MouseMove(InputAction.CallbackContext context)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

        //Reset highlights
        if (!isAttacking) gridSystem.SetColor(new Vector3Int(hoveredTile.x - 1, hoveredTile.y - 1, 0), Color.white);
        if (highlightedEntity != null)
            gridSystem.HighlightSquaresInRange(highlightedEntity,
                highlightedEntity.Range + highlightedEntity.AttackRange, Color.white);
        foreach (Transform child in iconParent)
        {
            Destroy(child.gameObject);
        }

        if (hit.collider != null)
        {
            Vector2 pos = transform.InverseTransformPoint(hit.point);
            hoveredTile = new Vector2Int((int)Mathf.Round(pos.x + 0.5f), (int)Mathf.Round(pos.y + 0.5f));
            validHoveredTile = true;
            if (gridSystem.GetTile(hoveredTile).linkedEntity != null && !isMoving)
            {
                highlightedEntity = gridSystem.GetTile(hoveredTile).linkedEntity;
            }
        }
        else
        {
            validHoveredTile = false;
        }

        if (gridSystem.GetTile(hoveredTile) == null) return;
        if (gridSystem.GetTile(hoveredTile).linkedEntity != null)
        {
            battleController.UpdateCharacterDisplay(gridSystem.GetTile(hoveredTile).linkedEntity);
            if (!gridSystem.GetTile(hoveredTile).linkedEntity.isHuman && hoveredTile != moveStartTile &&
                Vector3.Distance(pathLine.GetPosition(pathLine.positionCount - 1),
                    new Vector3(hoveredTile.x - 0.5f, hoveredTile.y - 0.5f, -5)) <= 1)
            {
                isAttacking = true;
            }
        }
        else
        {
            isAttacking = false;
        }

        if (isMoving) //Highlighted squares when moving character
        {
            if (highlightedEntity != null)
            {
                if (Vector2.Distance(hoveredTile, moveStartTile) < highlightedEntity.Range)
                {
                    //Add to path
                    if (validHoveredTile && gridSystem.GetTile(hoveredTile).walkable && Vector3.Distance(
                            new Vector3(hoveredTile.x - 0.5f, hoveredTile.y - 0.5f, -5f),
                            pathLine.GetPosition(pathLine.positionCount - 1)) <= 1 &&
                        (gridSystem.GetTile(hoveredTile).linkedEntity == null ||
                         gridSystem.GetTile(hoveredTile).linkedEntity == highlightedEntity) &&
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
                        {
                            pathLine.positionCount++;
                            pathLine.SetPosition(pathLine.positionCount - 1,
                                new Vector3(hoveredTile.x - 0.5f, hoveredTile.y - 0.5f, -5f));
                        }
                    }
                }
            }
        }

        if (!isMoving) highlightedEntity = gridSystem.GetTile(hoveredTile).linkedEntity;
        if (!isAttacking)
            gridSystem.SetColor(new Vector3Int(hoveredTile.x - 1, hoveredTile.y - 1, 0), new Color(0.8f, 0.8f, 0.8f));


        if (highlightedEntity != null)
        {
            gridSystem.HighlightSquaresInRange(highlightedEntity,
                highlightedEntity.Range + highlightedEntity.AttackRange, new Color(0.9f, 0.9f, 0.9f));
            gridSystem.HighlightSquaresInRange(highlightedEntity, highlightedEntity.Range,
                new Color(0.8f, 0.8f, 0.8f));
            if (isMoving)
            {
                for (int i = 0; i < battleController.GetCharacters().Count; i++)
                {
                    Entity e = battleController.GetCharacterAt(i);
                    if (Vector2.Distance(highlightedEntity.Position, e.Position) <
                        highlightedEntity.Range + highlightedEntity.AttackRange &&
                        e.isHuman != highlightedEntity.isHuman)
                    {
                        GameObject a = Instantiate(attackHighlightIcon, e.Position, Quaternion.identity,
                            iconParent);


                        if (new Vector2Int((int)(e.Position.x + 0.5f), (int)(e.Position.y + 0.5f)) == hoveredTile && Vector3.Distance(pathLine.GetPosition(pathLine.positionCount -1), new Vector3(hoveredTile.x - 0.5f, hoveredTile.y - 0.5f, -5f)) <= 1)
                        {
                            a.GetComponent<SpriteRenderer>().color = Color.red;
                        }
                    }
                }

            }
            Debug.Log($"mov{isMoving} attk:{isAttacking}");
        }


        if (isMoving && pathLine.positionCount > 0)
        {
            Vector3 pos = pathLine.GetPosition(pathLine.positionCount - 1);
            gridSystem.SetColor(new Vector3Int((int)(pos.x - 0.5f), (int)(pos.y - 0.5f), 0),
                new Color(0.5f, 0.5f, 0.5f));
            gridSystem.SetColor(new Vector3Int(moveStartTile.x - 1, moveStartTile.y - 1, 0),
                new Color(0.5f, 0.5f, 0.5f));
        }
    }
    public void StartPlayerTurn()
    {
        isPlayerTurn = true;
        actionButtonParent.SetActive(true);
    }
    
    public void StartMoving()
    {
        if(!isPlayerTurn) return;
        isAttacking = false;
        Debug.Log("Moving");
    }

    public void StartAttacking()
    {
        if (!isPlayerTurn) return;
        Debug.Log("Attacking");
        isMoving = false;
    }

    public void EndTurn()
    {
        if(!isPlayerTurn) return;
        isPlayerTurn = false;
        actionButtonParent.SetActive(false);
    }
    
}
