using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game
{
    public class PlayerAttackSystem : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField]private GridSystem gridSystem;
        [Header("Assets")]
        [SerializeField] private LineRenderer pathLine;
        [SerializeField] private Transform selectHighlight;
        [SerializeField] private RectTransform InfoDisplay;
        private Camera cam;
        private Entity hoveredEntity;
        private Vector2Int hoveredTile;
        private bool isActing;
        private Human actingEntity;
        private bool isPlayerTurn;
        private bool isPaused;
        private bool isTutorialPaused;
        private Vector3 offsetFix;
        private BattleController battleController;
        private Color moveColor;
        private Color attackColor;
        private Color possibleAttackColor;
        void Start()
        {
            cam = Camera.main;
            battleController = GetComponent<BattleController>();
            offsetFix = transform.InverseTransformPoint(Vector3.zero);
            moveColor = new Color(0.6f, 0.6f, 1f);
            attackColor = new Color(1f, 0.6f, 0.6f);
            possibleAttackColor = new Color(0.6f, 0.2f, 1f);
        }

        public void TileClicked(InputAction.CallbackContext context)
        {
            if (!isPlayerTurn || isPaused || isTutorialPaused) return;
            if (context.canceled)
            {
                if (gridSystem.GetTile(hoveredTile) != null) hoveredEntity = gridSystem.GetTile(hoveredTile).linkedEntity;
                if (isActing) // MOVE AND ATTACK MODE
                {
                    if (hoveredEntity == null && pathLine.positionCount > 0) //MOVES TO EMPTY TILE
                    {
                        if (hoveredTile == GetPathLinePos(pathLine.positionCount - 1))
                        {
                            StartCoroutine(battleController.Move(GetFullPathLine(), false));
                            if (actingEntity != null && actingEntity.isMoving)
                            {
                                pathLine.positionCount = 1;
                                SetPathLinePos(0, hoveredTile);
                            }
                        }
                        else // DESELECTS ACTOR
                        {
                            actingEntity = null;
                            isActing = false;
                            selectHighlight.position = Vector3.down * 100;
                            pathLine.positionCount = 0;
                            battleController.UpdateCharacterDisplay(false,null, false);
                        }
                    }
                    else if (hoveredEntity != null ) //TILE HAS ENTITY
                    {
                        if (hoveredEntity.isHuman)
                        {
                            if (hoveredEntity != actingEntity)  // SELECTS ANOTHER ACTOR
                            {
                                actingEntity = hoveredEntity.GetComponent<Human>();
                                pathLine.positionCount = 1;
                                SetPathLinePos(0, hoveredTile);
                                selectHighlight.position = actingEntity.transform.position;
                                selectHighlight.GetComponent<SpriteRenderer>().color = Color.white;
                            }
                            else // DESELECTS ACTOR
                            {
                                isActing = false;
                                actingEntity = null;
                                selectHighlight.position = Vector3.down * 100;
                                pathLine.positionCount = 0;
                            }
                        }
                        else // ATTACKS ENEMY
                        {
                            if (!actingEntity.hasAttacked && !hoveredEntity.isHuman)
                            {
                                if (actingEntity.IsMelee &&
                                    gridSystem.GetGridDistance(GetPathLinePos(pathLine.positionCount - 1),
                                        hoveredEntity.Position) <=
                                    actingEntity.AttackRange)
                                {
                                    if (pathLine.positionCount < 2)
                                    {
                                        battleController.Attack(actingEntity, hoveredEntity);
                                        if (actingEntity.hasAttacked)
                                        {
                                            pathLine.positionCount = 1;
                                            SetPathLinePos(0, hoveredTile);
                                            isActing = false;
                                            selectHighlight.position = Vector3.down * 100;
                                        }
                                    }
                                    else
                                    {
                                        StartCoroutine(battleController.Move(GetFullPathLine(), true, hoveredEntity));
                                        if (actingEntity != null && actingEntity.isMoving)
                                        {
                                            hoveredTile = GetPathLinePos(pathLine.positionCount - 1);
                                            pathLine.positionCount = 1;
                                            SetPathLinePos(0, hoveredTile);
                                        }
                                    }
                                }
                                else if (!actingEntity.IsMelee &&
                                         gridSystem.GetGridDistance(GetPathLinePos(pathLine.positionCount - 1),
                                             hoveredEntity.Position) <=
                                         actingEntity.AttackRange)
                                {
                                    StartCoroutine(battleController.Move(GetFullPathLine(), true, hoveredEntity));
                                    if (actingEntity != null && actingEntity.isMoving)
                                    {
                                        hoveredTile = GetPathLinePos(pathLine.positionCount - 1);
                                        pathLine.positionCount = 1;
                                        SetPathLinePos(0, hoveredTile);
                                    }
                                }
                            }
                        }
                    }
                    else // DESELECTS ACTOR
                    {
                        actingEntity = null;
                        isActing = false;
                        selectHighlight.position = Vector3.down * 100;
                        pathLine.positionCount = 0;
                    }

                }
                else
                {
                    if (gridSystem.GetTile(hoveredTile) != null && gridSystem.GetTile(hoveredTile).linkedEntity != null) actingEntity = gridSystem.GetTile(hoveredTile).linkedEntity.GetComponent<Human>();
                    if (actingEntity != null) // SWAP CHARACTER
                    {
                        if (actingEntity.isHuman)
                        {
                            isActing = true;
                            pathLine.positionCount = 1;
                            if (hoveredEntity != null) actingEntity = hoveredEntity.GetComponent<Human>();
                            selectHighlight.position = actingEntity.transform.position;
                            selectHighlight.GetComponent<SpriteRenderer>().color = Color.white;
                            SetPathLinePos(0, actingEntity.Position);
                        }
                    }
                }
            }
            UpdateBoard();
        }






        public void MouseMove(InputAction.CallbackContext context)
        {
            if (isPaused || isTutorialPaused) return;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);

            if (hit.collider != null)
            {
                Vector2 pos = transform.InverseTransformPoint(hit.point) - offsetFix;
                Vector2Int newPos = new Vector2Int((int)Mathf.Round(pos.x +0.5f), (int)Mathf.Round(pos.y+0.5f));
                if (hoveredTile == newPos) return;
                hoveredTile = newPos;
                if (gridSystem.GetTile(hoveredTile) != null && hoveredTile != null && gridSystem.GetTile(hoveredTile).linkedEntity != null)
                {
                    hoveredEntity = gridSystem.GetTile(hoveredTile).linkedEntity;
                }
            }
            UpdateBoard();
        }





        public void UpdateBoard()
        {
            //Reset board colors
            gridSystem.HighlightSquaresInRange(Vector2.zero, 50, Color.white);

            if (gridSystem.GetTile(hoveredTile) != null && gridSystem.GetTile(hoveredTile).linkedEntity != null)
            {
                battleController.UpdateCharacterDisplay(true, gridSystem.GetTile(hoveredTile).linkedEntity, true);
                InfoDisplay.localPosition = new Vector3(-50, hoveredTile.y < -3 ? 435 : -435, 0);
            }
            else  battleController.UpdateCharacterDisplay(false, null, true) ;
            
            if (gridSystem.GetTile(hoveredTile) == null) return;
            gridSystem.ResetUnusedHidingspotColor();
            foreach (Vector2Int pos in gridSystem.demons) // ATTACK HIGHLIGHT
            {
                gridSystem.GetTile(pos).linkedEntity.GetComponent<Demon>().DisplayAttackingImage(false, Color.white);
            }
            
            if (actingEntity != null)
            {
                if (actingEntity.hasAttacked)
                {
                    hoveredEntity = null;
                    isActing = false;
                    selectHighlight.position = Vector3.down * 100;
                    pathLine.positionCount = 1;
                }
                
                //MOVE PATH IF WITHIN RANGE
                if (actingEntity.moveDistanceRemaining > 0 &&  !actingEntity.isMoving && !actingEntity.isDefending)
                {                    
                    if (gridSystem.GetTile(hoveredTile).walkable &&
                         (gridSystem.GetTile(hoveredTile).linkedEntity == null ||
                          gridSystem.GetTile(hoveredTile).linkedEntity == actingEntity ||
                          (gridSystem.GetTile(hoveredTile).linkedEntity != null &&
                          !gridSystem.GetTile(hoveredTile).linkedEntity.isHuman &&
                         gridSystem.GetGridDistance(GetPathLinePos(pathLine.positionCount - 1), hoveredTile) > 1)) &&
                        gridSystem.GetGridDistance(actingEntity.Position, hoveredTile) <= actingEntity.moveDistanceRemaining)
                    {
                        pathLine.positionCount = 1;
                        Vector2Int[] path = gridSystem.PathFindValidPath(actingEntity.Position, hoveredTile,
                            actingEntity.moveDistanceRemaining);
                        foreach (Vector2Int pos in path)
                        {
                            pathLine.positionCount++;
                            SetPathLinePos(pathLine.positionCount - 1, pos);
                        }

                    }
             
                    //MOVE HIGHLIGHT
                    if (actingEntity.IsMelee)
                    {
                        gridSystem.HighlightMoveTiles(actingEntity.Position,
                            actingEntity.moveDistanceRemaining, moveColor);
                        gridSystem.HighlightMoveTiles(GetPathLinePos(pathLine.positionCount-1), actingEntity.AttackRange, 
                            attackColor);
                    }
                    else
                    {
                        gridSystem.HighlightSquaresInRange(GetPathLinePos(pathLine.positionCount-1), actingEntity.AttackRange,
                            attackColor);
                        gridSystem.HighlightMoveTiles(actingEntity.Position,
                            actingEntity.moveDistanceRemaining, moveColor);
                    }
                }
                else 
                {
                    // ATTACK HIGHLIGHT
                    if (!actingEntity.hasAttacked && !actingEntity.isMoving && !actingEntity.isDefending)
                    {
                        if (actingEntity.IsMelee)
                            gridSystem.HighlightMoveTiles(actingEntity.Position,
                                actingEntity.moveDistanceRemaining + actingEntity.AttackRange, attackColor);
                        else gridSystem.HighlightSquaresInRange(actingEntity.Position, actingEntity.AttackRange,
                                attackColor);
                    }
                }

                if (!actingEntity.hasAttacked && !actingEntity.isDefending)
                {
                    foreach (Vector2Int pos in gridSystem.demons) // ATTACK ICON
                    {
                        if (gridSystem.GetTile(pos).linkedEntity is Demon demon)
                        {
                            if (actingEntity.IsMelee && gridSystem.GetGridDistance(actingEntity.Position,
                                    demon.Position) <= actingEntity.moveDistanceRemaining + actingEntity.AttackRange)
                            {
                                gridSystem.ConnectedMovableTiles(actingEntity.Position,
                                    actingEntity.moveDistanceRemaining + actingEntity.AttackRange,
                                    out HashSet<Vector2Int> positions);
                                if (hoveredTile == pos && gridSystem.GetGridDistance(GetPathLinePos(pathLine.positionCount-1), pos) <= 1) demon.DisplayAttackingImage(true, Color.red);
                                else if (positions.Contains(demon.Position)) demon.DisplayAttackingImage(true, Color.white);
                                else demon.DisplayAttackingImage(false, Color.white);
                            }
                            else if (!actingEntity.IsMelee)
                            {
                                if (hoveredTile == pos)
                                {
                                    pathLine.positionCount = 1;
                                    if (gridSystem.GetGridDistance(GetPathLinePos(pathLine.positionCount - 1),
                                            hoveredTile) <= actingEntity.AttackRange)
                                    {
                                        demon.DisplayAttackingImage(true, Color.red);
                                    }
                                    else
                                    {
                                        Vector2Int[] path = gridSystem.PathFindValidPath(actingEntity.Position, hoveredTile,
                                            actingEntity.moveDistanceRemaining);
                                        foreach (Vector2Int p in path)
                                        {
                                            pathLine.positionCount++;
                                            SetPathLinePos(pathLine.positionCount - 1, p);
                                        }
                                        if (gridSystem.GetGridDistance(GetPathLinePos(pathLine.positionCount - 1),
                                                hoveredTile) <= actingEntity.AttackRange)
                                        {
                                            demon.DisplayAttackingImage(true, Color.red);
                                        }
                                    }
                                    gridSystem.HighlightSquaresInRange(Vector2.zero, 50, Color.white);
                                    gridSystem.HighlightSquaresInRange(GetPathLinePos(pathLine.positionCount-1), actingEntity.AttackRange,
                                        attackColor);
                                    gridSystem.HighlightMoveTiles(actingEntity.Position,
                                        actingEntity.moveDistanceRemaining, moveColor);
                                }
                                else if (gridSystem.GetGridDistance(GetPathLinePos(pathLine.positionCount - 1),
                                             demon.Position) <= actingEntity.AttackRange) demon.DisplayAttackingImage(true, Color.white);
                                else demon.DisplayAttackingImage(false, Color.white);
                            }

                        }
                    }
                }
            }
            else if (gridSystem.GetTile(hoveredTile).linkedEntity != null && hoveredEntity != null)
            {
                if (hoveredEntity.moveDistanceRemaining > 0 && !(hoveredEntity.isHuman && hoveredEntity.GetComponent<Human>().isDefending))
                {
                    //MOVE HIGHLIGHT
                    if (hoveredEntity.IsMelee)
                    {
                        gridSystem.HighlightMoveTiles(hoveredEntity.Position,
                            hoveredEntity.moveDistanceRemaining, moveColor);
                        gridSystem.HighlightMoveTiles(hoveredEntity.Position,
                            hoveredEntity.AttackRange, attackColor);
                    }
                    else
                    {
                        gridSystem.HighlightSquaresInRange(hoveredEntity.Position, hoveredEntity.AttackRange,
                            attackColor);
                        gridSystem.HighlightMoveTiles(hoveredEntity.Position,
                            hoveredEntity.moveDistanceRemaining, moveColor);
                    }
                }
                else 
                {   // ATTACK HIGHLIGHT
                    if (hoveredEntity!= null && !hoveredEntity.hasAttacked  && !hoveredEntity.GetComponent<Human>().isDefending)
                    {
                        if (hoveredEntity.IsMelee)
                            gridSystem.HighlightMoveTiles(hoveredEntity.Position,
                                hoveredEntity.AttackRange, attackColor);
                        else gridSystem.HighlightSquaresInRange(hoveredEntity.Position, hoveredEntity.AttackRange,
                                attackColor);
                    }
                }

                if (!hoveredEntity.hasAttacked && hoveredEntity.isHuman)
                {
                    foreach (Vector2Int pos in gridSystem.demons) // ATTACK ICON
                    {
                        if (gridSystem.GetTile(pos).linkedEntity is Demon demon)
                        {
                            if (hoveredEntity.IsMelee && gridSystem.GetGridDistance(hoveredEntity.Position, pos) <=
                                hoveredEntity.moveDistanceRemaining + hoveredEntity.AttackRange) 
                                demon.DisplayAttackingImage(true, Color.white);
                            else if (!hoveredEntity.IsMelee && gridSystem.GetGridDistance(hoveredEntity.Position, pos) <=
                                     hoveredEntity.AttackRange) 
                                demon.DisplayAttackingImage(true, Color.white);
                            else demon.DisplayAttackingImage(false, Color.white);
                        }
                    }
                }
            }
            else
            {
                if (pathLine.positionCount > 0)
                {
                    hoveredTile = GetPathLinePos(pathLine.positionCount - 1);
                    pathLine.positionCount = 1;
                    SetPathLinePos(0, hoveredTile);
                }
            }
        }

        private Vector2Int[] GetFullPathLine()
        {
            Vector2Int[] path = new Vector2Int[pathLine.positionCount];
            for (int i = 0; i < pathLine.positionCount; i++)
            {
                path[i] = GetPathLinePos(i);
            }

            return path;
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
            UIManager.PlayerEndTurn();
            actingEntity = null;
            isActing = false;
            selectHighlight.position = Vector3.down * 100;
            isPlayerTurn = false;
            gridSystem.HighlightSquaresInRange(Vector2.zero, 50, Color.white);
            
        }

        public void StartTurn()
        {
            isPlayerTurn = true;
            UIManager.PlayerStartTurn();
            foreach (Vector2Int pos in gridSystem.humans)
            {
                gridSystem.GetTile(pos).linkedEntity.GetComponent<Human>().isDefending = false;
            }
        }

        public void SetPaused(bool paused)
        {
            isPaused = paused;
            isActing = false;
            selectHighlight.position = Vector3.down * 100;
            pathLine.positionCount = 1;
            gridSystem.HighlightSquaresInRange(Vector2.zero, 50, Color.white);
        }
        
        public void SetTutorialPaused(bool paused)
        {
            isTutorialPaused = paused;
            isActing = false;
            selectHighlight.position = Vector3.down * 100;
            pathLine.positionCount = 1;
            gridSystem.HighlightSquaresInRange(Vector2.zero, 50, Color.white);
        }

        public void ToDefendingMode()
        {
            if (actingEntity != null)
            {
                actingEntity.isDefending = true;
                selectHighlight.GetChild(0).gameObject.SetActive(false);
            }
        }
    }
}