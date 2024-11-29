using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Game
{
    public class PlayerAttackSystem : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField]private GridSystem gridSystem;
        [Header("Assets")]
        [SerializeField] private GameObject endTurnButton;
        [SerializeField] private LineRenderer pathLine;
        [SerializeField] private GameObject attackHighlightIcon;
        [SerializeField] private Transform iconParent;
        [SerializeField] private Image moveIcon;
        [SerializeField] private Image attackIcon;
        private Entity hoveredEntity;
        private Vector2Int hoveredTile;
        private bool validHoveredTile;
        private Vector2Int moveStartTile;
        private bool isMoving;
        private bool isAttacking;
        private bool attackMode;
        private bool isPlayerTurn;
        private BattleController battleController;
        void Start()
        {
            battleController = GetComponent<BattleController>();
            moveIcon.color = Color.gray;
            attackIcon.color = Color.gray;
        }

        public void TileClicked(InputAction.CallbackContext context)
        {
            if (!isPlayerTurn) return;
            if (context.started) // Click pressed
            {
                Entity currentEntity = gridSystem.GetTile(hoveredTile).linkedEntity;
                if (currentEntity != null)
                {
                    if (currentEntity.isHuman && !attackMode && currentEntity.hasQueuedMovement)
                    {
                        isMoving = true;
                        pathLine.positionCount = 1;
                        pathLine.SetPosition(0, new Vector3(hoveredTile.x - 0.5f, hoveredTile.y - 0.5f, -5f));
                        moveStartTile = hoveredTile;
                        gridSystem.HighlightSquaresInRange(hoveredEntity.Position, hoveredEntity.AttackRange,
                            new Color(0.8f, 0.8f, 0.8f));
                    }
                }
            }
            else if (context.canceled) // Click released
            {
                if (isMoving)
                {
                    if (isAttacking)
                    {
                        hoveredEntity.AttackQueued(true);
                        battleController.Attack(hoveredEntity, gridSystem.GetTile(hoveredTile).linkedEntity);
                        battleController.UpdateCharacterDisplay(true, gridSystem.GetTile(hoveredTile).linkedEntity);

                    }

                    gridSystem.HighlightSquaresInRange(hoveredEntity.Position,
                        hoveredEntity.MoveRange + hoveredEntity.AttackRange, Color.white);
                    if (hoveredTile != moveStartTile)
                    {
                        gridSystem.GetTile(moveStartTile).linkedEntity.hasQueuedMovement = true;
                        battleController.Move(moveStartTile, new Vector2Int(
                            (int)(pathLine.GetPosition(pathLine.positionCount - 1).x + 0.5f),
                            (int)(pathLine.GetPosition(pathLine.positionCount - 1).y + 0.5f)));
                        if (!isAttacking)
                        {
                            moveStartTile = new Vector2Int(
                                (int)(pathLine.GetPosition(pathLine.positionCount - 1).x + 0.5f),
                                (int)(pathLine.GetPosition(pathLine.positionCount - 1).y + 0.5f));
                            attackMode = true;
                        }
                    }
                    else
                    {
                        attackMode = !attackMode;
                        moveStartTile = hoveredTile;
                        isMoving = false;
                    }

                    isAttacking = false;
                    isMoving = false;
                    pathLine.positionCount = 1;
                    gridSystem.SetColor(new Vector3Int(hoveredTile.x - 1, hoveredTile.y - 1, 0),
                        new Color(0.8f, 0.8f, 0.8f));
                }
                else //if (hoveredTile != moveStartTile)
                {
                    attackMode = false;
                    isAttacking = false;
                    gridSystem.HighlightSquaresInRange(hoveredTile, 99, Color.white);
                }

            }

            UpdateBoard();
        }






        public void MouseMove(InputAction.CallbackContext context)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

            if (hit.collider != null)
            {
                Vector2 pos = transform.InverseTransformPoint(hit.point);
                Vector2Int newPos = new Vector2Int((int)Mathf.Round(pos.x + 0.5f), (int)Mathf.Round(pos.y + 0.5f));
                if (hoveredTile == newPos) return;
                hoveredTile = newPos;
                if (gridSystem.GetTile(hoveredTile).linkedEntity != null && !isMoving)
                {
                    hoveredEntity = gridSystem.GetTile(hoveredTile).linkedEntity;
                }
            }

            validHoveredTile = hit.collider != null;
            UpdateBoard();
        }





        void UpdateBoard()
        {

            //Reset board colors
            gridSystem.HighlightSquaresInRange(Vector2.zero, 50, Color.white);
            foreach (Transform child in iconParent)
            {
                Destroy(child.gameObject);
            }

            battleController.UpdateCharacterDisplay(gridSystem.GetTile(hoveredTile).linkedEntity != null,
                gridSystem.GetTile(hoveredTile).linkedEntity);
            if (gridSystem.GetTile(hoveredTile) == null) return;

            if (isMoving) //Highlighted squares when moving character
            {
                if (hoveredEntity != null && !attackMode)
                {
                    if (gridSystem.GetGridDistance(hoveredTile, moveStartTile) <= hoveredEntity.MoveRange)
                    {
                        //Add to path
                        if (validHoveredTile && gridSystem.GetTile(hoveredTile).walkable && Vector3.Distance(
                                new Vector3(hoveredTile.x - 0.5f, hoveredTile.y - 0.5f, -5f),
                                pathLine.GetPosition(pathLine.positionCount - 1)) <= 1 &&
                            (gridSystem.GetTile(hoveredTile).linkedEntity == null ||
                             gridSystem.GetTile(hoveredTile).linkedEntity == hoveredEntity) &&
                            pathLine.GetPosition(pathLine.positionCount - 1) !=
                            new Vector3(hoveredTile.x - 0.5f, hoveredTile.y - 0.5f, -5))
                        {
                            bool valid = true;
                            for (int i = 0; i < pathLine.positionCount; i++)
                            {
                                if (pathLine.GetPosition(i) ==
                                    new Vector3(hoveredTile.x - 0.5f, hoveredTile.y - 0.5f, -5))
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

            if (!isMoving) hoveredEntity = gridSystem.GetTile(hoveredTile).linkedEntity;
            if (!isAttacking)
                gridSystem.SetColor(new Vector3Int(hoveredTile.x - 1, hoveredTile.y - 1, 0),
                    new Color(0.8f, 0.8f, 0.8f));


            if (hoveredEntity != null && !attackMode)
            {
                gridSystem.HighlightSquaresInRange(hoveredEntity.Position,
                    hoveredEntity.MoveRange + hoveredEntity.AttackRange, new Color(0.8f, 0.8f, 0.8f));
                gridSystem.HighlightSquaresInRange(hoveredEntity.Position, hoveredEntity.MoveRange,
                    new Color(0.7f, 0.7f, 0.7f));
                isAttacking = false;
                if (isMoving)
                {
                    for (int i = 0; i < battleController.GetCharacters().Count; i++)
                    {
                        Entity highlightedEntity = battleController.GetCharacterAt(i);
                        if (highlightedEntity.isHuman != hoveredEntity.isHuman)
                        {
                            GameObject a = Instantiate(attackHighlightIcon, highlightedEntity.Position,
                                Quaternion.identity,
                                iconParent);
                            if (gridSystem.GetGridDistance(hoveredEntity.Position, highlightedEntity.Position) >
                                hoveredEntity.AttackRange + hoveredEntity.MoveRange)
                            {
                                a.GetComponent<SpriteRenderer>().color = Color.gray;
                            }
                            else if (new Vector2Int((int)(highlightedEntity.Position.x + 0.5f),
                                         (int)(highlightedEntity.Position.y + 0.5f)) == hoveredTile && gridSystem.GetGridDistance(pathLine.GetPosition(pathLine.positionCount - 1) + new Vector3(0.5f, 0.5f), hoveredTile) <= 1)
                            {
                                a.GetComponent<SpriteRenderer>().color = Color.red;
                                isAttacking = true;
                            }
                        }
                    }

                }
            }
            else if (attackMode)
            {
                Entity currentEntity = gridSystem.GetTile(moveStartTile).linkedEntity;
                gridSystem.HighlightSquaresInRange(currentEntity.Position, currentEntity.AttackRange,
                    new Color(0.8f, 0.8f, 0.8f));

                for (int i = 0; i < battleController.GetCharacters().Count; i++)
                {
                    Entity highlightedEntity = battleController.GetCharacterAt(i);
                    if (highlightedEntity.isHuman != currentEntity.isHuman)
                    {
                        GameObject a = Instantiate(attackHighlightIcon, highlightedEntity.Position, Quaternion.identity,
                            iconParent);
                        if (gridSystem.GetGridDistance(currentEntity.Position, highlightedEntity.Position) >
                            currentEntity.AttackRange)
                        {
                            a.GetComponent<SpriteRenderer>().color = Color.gray;
                        }
                        else if (new Vector2Int((int)(highlightedEntity.Position.x + 0.5f),
                                     (int)(highlightedEntity.Position.y + 0.5f)) == hoveredTile)
                        {
                            a.GetComponent<SpriteRenderer>().color = Color.red;
                            isAttacking = true;
                        }
                    }
                }
            }


            if (isMoving && pathLine.positionCount > 0)
            {
                Vector3 pos = pathLine.GetPosition(pathLine.positionCount - 1);
                gridSystem.SetColor(new Vector3Int((int)(pos.x - 0.5f), (int)(pos.y - 0.5f), 0),
                    new Color(0.5f, 0.5f, 0.5f));
                gridSystem.SetColor(new Vector3Int(moveStartTile.x - 1, moveStartTile.y - 1, 0),
                    new Color(0.5f, 0.5f, 0.5f));
            }

            moveIcon.color = isMoving && !attackMode ? Color.white : Color.gray;
            attackIcon.color = attackMode || isAttacking ? Color.white : Color.gray;
        }

        public void EndTurn()
        {
            if (!isPlayerTurn) return;
            isPlayerTurn = false;
            endTurnButton.SetActive(false);
            moveIcon.gameObject.SetActive(false);
            attackIcon.gameObject.SetActive(false);
        }

        public void StartTurn()
        {
            isPlayerTurn = true;
            endTurnButton.SetActive(true);
            moveIcon.gameObject.SetActive(true);
            attackIcon.gameObject.SetActive(true);
        }

    }
}
