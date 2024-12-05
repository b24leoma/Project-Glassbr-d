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
        private Entity hoveredEntity;
        private Vector2Int hoveredTile;
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
            if (context.canceled)
            {
                hoveredEntity = gridSystem.GetTile(hoveredTile).linkedEntity;
                if (isActing) // MOVE AND ATTACK MODE
                {
                    if (hoveredEntity == null && pathLine.positionCount > 0) //MOVES TO EMPTY TILE
                    {
                        if (hoveredTile == GetPathLinePos(pathLine.positionCount - 1))
                        {
                            actingEntity.SetMoving(true);
                            Vector2Int newPos = GetPathLinePos(pathLine.positionCount - 1);
                            battleController.Move(actingEntity.Position, newPos);
                            pathLine.positionCount = 1;
                            SetPathLinePos(0, actingEntity.Position);
                        }
                        else
                        {
                            pathLine.positionCount = 0;
                            isActing = false;
                        }
                    }
                    else if (hoveredEntity != null ) //TILE HAS ENTITY
                    {
                        if (hoveredEntity.isHuman && hoveredEntity != actingEntity) // SELECTS ANOTHER ACTOR
                        {
                            actingEntity = hoveredEntity;
                            pathLine.positionCount = 1;
                            SetPathLinePos(0, actingEntity.Position);
                        }
                        else // ATTACKS ENEMY
                        {
                            if (!actingEntity.hasAttacked && !hoveredEntity.isHuman && gridSystem.GetGridDistance(actingEntity.Position, hoveredEntity.Position) <=
                                actingEntity.AttackRange)
                            {
                                actingEntity.SetAttacking(true);
                                battleController.Attack(actingEntity, hoveredEntity);
                                battleController.UpdateCharacterDisplay(true, hoveredEntity);
                                if (hoveredEntity is Demon demon)
                                {
                                    demon.DisplayAttackingImage(false, Color.white);   
                                }
                                pathLine.positionCount = 1;
                                SetPathLinePos(0, actingEntity.Position);
                                isActing = false;

                            }
                        }
                    }

                }
                else
                {
                    actingEntity = gridSystem.GetTile(hoveredTile).linkedEntity;
                    if (actingEntity != null) // SWAP CHARACTER
                    {
                        if (actingEntity.isHuman)
                        {
                            isActing = true;
                            pathLine.positionCount = 1;
                            actingEntity = hoveredEntity;
                            SetPathLinePos(0, actingEntity.Position);
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
            UpdateBoard();
        }





        void UpdateBoard()
        {
            //Reset board colors
            gridSystem.HighlightSquaresInRange(Vector2.zero, 50, Color.white);
            battleController.UpdateCharacterDisplay(gridSystem.GetTile(hoveredTile).linkedEntity != null,
                gridSystem.GetTile(hoveredTile).linkedEntity);
            if (gridSystem.GetTile(hoveredTile) == null) return;

            if (isActing)
            {
                //MOVE PATH IF WITHIN RANGE
                if (!actingEntity.hasMoved && gridSystem.GetGridDistance(actingEntity.Position, hoveredTile) <=
                    actingEntity.MoveRange)
                {
                    if (gridSystem.GetTile(hoveredTile).walkable &&
                        (gridSystem.GetTile(hoveredTile).linkedEntity == null ||
                         gridSystem.GetTile(hoveredTile).linkedEntity == actingEntity) &&
                        gridSystem.GetGridDistance(hoveredTile, GetPathLinePos(pathLine.positionCount - 1)) <= 1)
                    {
                        bool valid = true;
                        for (int i = 0; i < pathLine.positionCount; i++)
                        {
                            if (GetPathLinePos(i) == hoveredTile)
                            {
                                valid = false;
                                pathLine.positionCount = i + 1;
                            }
                        }

                        if (valid && pathLine.positionCount < actingEntity.MoveRange + 1)
                        {
                            pathLine.positionCount++;
                            SetPathLinePos(pathLine.positionCount - 1, hoveredTile);
                        }
                    }
                }

                if (gridSystem.GetGridDistance(GetPathLinePos(pathLine.positionCount - 1), hoveredTile) == 0)
                {
                    //MOVE HIGHLIGHT
                    if (!actingEntity.hasMoved)
                    {
                        gridSystem.HighlightSquaresInRange(GetPathLinePos(pathLine.positionCount - 1),
                            actingEntity.MoveRange - pathLine.positionCount + 1, new Color(0.8f, 0.8f, 0.8f));
                    }
                }
                else 
                {   // ATTACK HIGHLIGHT
                    if (!actingEntity.hasAttacked)
                    {
                        gridSystem.HighlightSquaresInRange(actingEntity.Position,
                            actingEntity.AttackRange, new Color(0.8f, 0.8f, 0.8f));
                        foreach (Vector2Int pos in gridSystem.demons) // ATTACK ICON
                        {
                            Demon demon = gridSystem.GetTile(pos).linkedEntity as Demon;
                            if (gridSystem.GetGridDistance(actingEntity.Position, pos) <=
                                actingEntity.AttackRange)
                                demon.DisplayAttackingImage(true, hoveredTile == pos ? Color.red : Color.white);
                            else demon.DisplayAttackingImage(false, Color.white);

                        }
                    }
                }
            }
            else
            {
                if (gridSystem.GetTile(hoveredTile).linkedEntity != null)
                {
                    gridSystem.HighlightSquaresInRange(gridSystem.GetTile(hoveredTile).linkedEntity.Position,
                        gridSystem.GetTile(hoveredTile).linkedEntity.MoveRange + gridSystem.GetTile(hoveredTile).linkedEntity.AttackRange, new Color(0.9f, 0.9f, 0.9f));
                    gridSystem.HighlightSquaresInRange(gridSystem.GetTile(hoveredTile).linkedEntity.Position,
                        gridSystem.GetTile(hoveredTile).linkedEntity.MoveRange, new Color(0.8f, 0.8f, 0.8f));
                }
                
                foreach (Vector2Int pos in gridSystem.demons) // ATTACK HIGHLIGHT
                {
                    Demon demon = gridSystem.GetTile(pos).linkedEntity as Demon;
                    demon.DisplayAttackingImage(false, Color.white);
                }
            }
            gridSystem.SetColor(hoveredTile, new Color(0.7f, 0.7f, 0.7f));
        }

        private Vector2Int GetPathLinePos(int pos)
        {
            return new Vector2Int((int)(pathLine.GetPosition(pos).x + 0.5f), (int)(pathLine.GetPosition(pos).y + 0.5f));
        }
        private void SetPathLinePos(int index, Vector2Int pos)
        {
            pathLine.SetPosition(index, new Vector3(pos.x - 0.5f, pos.y - 0.5f, -5));
        }

        public void EndTurn()
        {
            isActing = false;
            isPlayerTurn = false;
            endTurnButton.SetActive(false);
            foreach (Vector2Int p in gridSystem.humans)
            {
                Entity e = gridSystem.GetTile(p).linkedEntity;
                e.SetAttacking(false);
                e.SetMoving(false);
            }
        }

        public void StartTurn()
        {
            isPlayerTurn = true;
            endTurnButton.SetActive(true);
        }
    }
}