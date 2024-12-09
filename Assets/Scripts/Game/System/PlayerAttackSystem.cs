using System.Linq;
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
        private bool isPaused;
        private Vector3 offsetFix;
        private BattleController battleController;
        private Color moveColor;
        private Color attackColor;
        private Color possibleAttackColor;
        void Start()
        {
            battleController = GetComponent<BattleController>();
            offsetFix = transform.InverseTransformPoint(Vector3.zero);
            moveColor = new Color(0.6f, 0.6f, 1f);
            attackColor = new Color(1f, 0.6f, 0.6f);
            possibleAttackColor = new Color(1, 0.8f, 0.8f);
        }

        public void TileClicked(InputAction.CallbackContext context)
        {
            if (!isPlayerTurn || isPaused) return;
            if (context.canceled)
            {
                hoveredEntity = gridSystem.GetTile(hoveredTile).linkedEntity;
                if (isActing) // MOVE AND ATTACK MODE
                {
                    if (hoveredEntity == null && pathLine.positionCount > 0) //MOVES TO EMPTY TILE
                    {
                        if (hoveredTile == GetPathLinePos(pathLine.positionCount - 1))
                        {
                            Vector2Int newPos = GetPathLinePos(pathLine.positionCount - 1);
                            if (gridSystem.GetTile(actingEntity.Position).hidingSpot) gridSystem.SetHidingSpotColor(actingEntity.Position,Color.white);
                            if (gridSystem.GetTile(newPos).hidingSpot) gridSystem.SetHidingSpotColor(newPos, new Color(1,1,1,0.3f));
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
            if (isPaused) return;
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
            if (gridSystem.GetTile(hoveredTile).hidingSpot && gridSystem.GetTile(hoveredTile).linkedEntity == null) gridSystem.SetHidingSpotColor(hoveredEntity.Position,Color.white);
            foreach (Vector2Int pos in gridSystem.demons) // ATTACK HIGHLIGHT
            {
                Demon demon = gridSystem.GetTile(pos).linkedEntity as Demon;
                demon.DisplayAttackingImage(false, Color.white);
            }
            
            if (isActing)
            {
                //MOVE PATH IF WITHIN RANGE
                if (!actingEntity.hasMoved)
                {
                    if ((gridSystem.GetTile(hoveredTile).walkable &&
                         (gridSystem.GetTile(hoveredTile).linkedEntity == null ||
                          gridSystem.GetTile(hoveredTile).linkedEntity == actingEntity) ||
                         gridSystem.GetTile(hoveredTile).linkedEntity != null &&
                         !gridSystem.GetTile(hoveredTile).linkedEntity.isHuman) &&
                        gridSystem.GetGridDistance(actingEntity.Position, hoveredTile) <= actingEntity.MoveRange &&
                        gridSystem.GetGridDistance(actingEntity.Position, hoveredTile) <=
                        actingEntity.MoveRange) 
                    {
                        Debug.Log(Time.time);
                        pathLine.positionCount = 1;
                        Vector2Int[] path = gridSystem.PathFindValidPath(actingEntity.Position, hoveredTile,
                            actingEntity.MoveRange);
                        foreach (Vector2Int pos in path)
                        {
                            pathLine.positionCount++;
                            SetPathLinePos(pathLine.positionCount - 1, pos);
                        }

                    }
                }

                if (!actingEntity.hasMoved)
                {
                    //MOVE HIGHLIGHT
                    if (actingEntity.IsMelee)
                    {
                        gridSystem.HighlightMoveTiles(actingEntity.Position,
                            actingEntity.MoveRange + actingEntity.AttackRange, possibleAttackColor);
                        gridSystem.HighlightMoveTiles(actingEntity.Position,
                            actingEntity.MoveRange, moveColor);
                        gridSystem.HighlightMoveTiles(GetPathLinePos(pathLine.positionCount-1),
                            actingEntity.AttackRange, attackColor);
                    }
                    else
                    {
                        gridSystem.HighlightSquaresInRange(actingEntity.Position, actingEntity.AttackRange,
                            attackColor);
                        gridSystem.HighlightMoveTiles(actingEntity.Position,
                            actingEntity.MoveRange, moveColor);
                    }
                }
                else 
                {   // ATTACK HIGHLIGHT
                    if (!actingEntity.hasAttacked)
                    {
                        if (actingEntity.IsMelee)
                            gridSystem.HighlightMoveTiles(actingEntity.Position,
                                actingEntity.AttackRange, attackColor);
                        else gridSystem.HighlightSquaresInRange(actingEntity.Position, actingEntity.AttackRange,
                                attackColor);
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
                //HOVER HIGHLIGHT
                if (gridSystem.GetTile(hoveredTile).linkedEntity != null)
                {
                    if (hoveredEntity.IsMelee)
                    {
                        gridSystem.HighlightMoveTiles(hoveredEntity.Position,
                            hoveredEntity.MoveRange + hoveredEntity.AttackRange, possibleAttackColor);
                        gridSystem.HighlightMoveTiles(hoveredEntity.Position,
                            hoveredEntity.MoveRange, moveColor);
                        gridSystem.HighlightMoveTiles(hoveredEntity.Position,
                            hoveredEntity.AttackRange, attackColor);
                    }
                    else
                    {
                        gridSystem.HighlightSquaresInRange(hoveredEntity.Position, hoveredEntity.AttackRange,
                            attackColor);
                        gridSystem.HighlightMoveTiles(hoveredEntity.Position,
                            hoveredEntity.MoveRange, moveColor);
                    }
                }
            }
            gridSystem.SetColor(hoveredTile, new Color(0.7f, 0.7f, 0.7f));
            if (gridSystem.GetTile(hoveredTile).hidingSpot) gridSystem.SetHidingSpotColor(hoveredTile, new Color(1,1,1,0.3f));
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

        public void SetPaused(bool paused)
        {
            isPaused = paused;
        }
    }
}