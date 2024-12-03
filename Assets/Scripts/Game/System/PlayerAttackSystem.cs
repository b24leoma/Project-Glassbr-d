using UnityEngine;
using UnityEngine.InputSystem;

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
        private Entity hoveredEntity;
        private Vector2Int hoveredTile;
        private bool validHoveredTile;
        private bool isActing;
        private Entity actingEntity;
        private bool isPlayerTurn;
        private Vector3 offsetFix;
        private BattleController battleController;
        void Start()
        {
            battleController = GetComponent<BattleController>();
            offsetFix = transform.InverseTransformPoint(Vector3.zero);
        }

        public void TileClicked(InputAction.CallbackContext context)
        {
            if (!isPlayerTurn) return;
            Entity currentEntity = gridSystem.GetTile(hoveredTile).linkedEntity;

            if (context.canceled)
            {
                if (isActing) // MOVE AND ATTACK MODE
                {
                    if (currentEntity == null && pathLine.positionCount > 0) //MOVES TO EMPTY TILE
                    {
                        actingEntity.MoveQueued(true);
                        Vector2Int newPos = GetPathlinePos(pathLine.positionCount - 1);
                        battleController.Move(actingEntity.Position, newPos);
                        pathLine.positionCount = 1;
                        isActing = false;
                    }
                    else //TILE HAS ENTITY
                    {
                        if (currentEntity.isHuman) // SELECTS ANOTHER ACTOR
                        {
                            actingEntity = currentEntity; 
                        }
                        else // ATTACKS ENEMY
                        {
                            if (gridSystem.GetGridDistance(actingEntity.Position, currentEntity.Position) <
                                actingEntity.AttackRange)
                            {
                                actingEntity.AttackQueued(true);
                                battleController.Attack(actingEntity, currentEntity);
                                battleController.UpdateCharacterDisplay(true, currentEntity);
                            }
                        }
                    }

                }
                else
                {
                    actingEntity = gridSystem.GetTile(hoveredTile).linkedEntity;
                    if (actingEntity != null)
                    {
                        if (actingEntity.isHuman && !actingEntity.hasQueuedMovement)
                        {
                            isActing = true;
                            pathLine.positionCount = 1;
                            SetPathlinePos(0, actingEntity.Position);
                        }
                    }
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
                Vector2 pos = transform.InverseTransformPoint(hit.point) - offsetFix;
                Vector2Int newPos = new Vector2Int((int)Mathf.Round(pos.x +0.5f), (int)Mathf.Round(pos.y+0.5f));
                if (hoveredTile == newPos) return;
                hoveredTile = newPos;
                if (gridSystem.GetTile(hoveredTile).linkedEntity != null)
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
            
            gridSystem.SetColor(hoveredTile, new Color(0.7f,0.7f,0.7f));

            if (isActing)
            {
                //MOVEPATH IF WITHIN RANGE
                if (gridSystem.GetGridDistance(actingEntity.Position, hoveredTile) <= actingEntity.MoveRange) 
                {
                     if (gridSystem.GetTile(hoveredTile).walkable //&& gridSystem.GetGridDistance(hoveredEntity.Position, GetPathlinePos(pathLine.positionCount - 1))  <= 1
                                                                  && gridSystem.GetTile(hoveredTile).linkedEntity == null && gridSystem.GetGridDistance(hoveredTile, GetPathlinePos(pathLine.positionCount - 1)) <= 1)
                        {
                            bool valid = true;
                            for (int i = 0; i < pathLine.positionCount; i++)
                            {
                                if (GetPathlinePos(i) == hoveredTile)
                                {
                                    valid = false;
                                    pathLine.positionCount = i + 1;
                                }
                            }

                            if (valid && pathLine.positionCount < actingEntity.MoveRange)
                            {
                                pathLine.positionCount++;
                                SetPathlinePos(pathLine.positionCount - 1, hoveredTile);
                            }
                        }
                    gridSystem.HighlightSquaresInRange(GetPathlinePos(pathLine.positionCount - 1), actingEntity.MoveRange - pathLine.positionCount, new Color(0.8f,0.8f,0.8f));
                } // ATTACKHIGHLIGHT
                else if (gridSystem.GetGridDistance(actingEntity.Position, hoveredTile) < actingEntity.MoveRange +
                         actingEntity.AttackRange - (pathLine.positionCount - 1) && hoveredEntity != null) 
                {
                    GameObject a = Instantiate(attackHighlightIcon, new Vector3(hoveredTile.x, hoveredTile.y, 0),
                        Quaternion.identity,
                        iconParent);
                }
            }
        }

        private Vector2Int GetPathlinePos(int pos)
        {
            return new Vector2Int((int)(pathLine.GetPosition(pos).x + 0.5f), (int)(pathLine.GetPosition(pos).y + 0.5f));
        }
        private void SetPathlinePos(int index, Vector2Int pos)
        {
            pathLine.SetPosition(index, new Vector3(pos.x - 0.5f, pos.y - 0.5f, -5));
        }

        public void EndTurn()
        {
            isActing = false;
            foreach (Entity e in battleController.GetCharacters())
            {
                e.AttackQueued(false);
                e.MoveQueued(false);
            }
        }

        public void StartTurn()
        {
            isPlayerTurn = true;
            endTurnButton.SetActive(true);
        }
    }
}