using System.Collections.Generic;
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
        private Entity hoveredEntity;
        private Vector2Int hoveredTile;
        private bool validHoveredTile;
        private Vector2Int moveStartTile;
        private bool isMoving;
        private bool isAttacking;
        private bool attackMode;
        private bool isPlayerTurn;
        private Vector3 offsetFix;
        private BattleController battleController;
        private Dictionary<Entity, Entity> attackList;
        void Start()
        {
            battleController = GetComponent<BattleController>();
            attackList = new Dictionary<Entity, Entity>();
            offsetFix = transform.InverseTransformPoint(Vector3.zero);
        }

        public void TileClicked(InputAction.CallbackContext context)
        {
            if (!isPlayerTurn) return;
            Entity currentEntity = gridSystem.GetTile(hoveredTile).linkedEntity;
            if (context.started) // Click pressed
            {
                if (currentEntity != null)
                {
                    if (currentEntity.isHuman && !attackMode && !currentEntity.hasQueuedMovement)
                    {
                        isMoving = true;
                        pathLine.positionCount = 1;
                        pathLine.SetPosition(0, new Vector3(hoveredTile.x - 0.5f, hoveredTile.y - 0.5f, -5f));
                        moveStartTile = hoveredTile;
                    }
                }
            }
            else if (context.canceled) // Click released
            {
                if (isAttacking )// && gridSystem.GetGridDistance(moveStartTile, hoveredTile) <= gridSystem.GetTile(moveStartTile).linkedEntity.AttackRange)
                {
                    Entity startEntity = gridSystem.GetTile(moveStartTile).linkedEntity;
                    if (currentEntity != null && !currentEntity.isHuman)
                    {
                        gridSystem.GetTile(moveStartTile).linkedEntity.AttackQueued(true);
                        attackList[startEntity] = currentEntity;
                        battleController.UpdateCharacterDisplay(true, currentEntity);
                        attackMode = false;
                    }

                    if (currentEntity == startEntity)
                    {
                        attackList.Remove(startEntity);
                        startEntity.AttackQueued(false);
                        attackMode = false;
                    }
                }
                /*else
                {
                    if(hoveredEntity != null) hoveredEntity.AttackQueued(false);
                    else if (gridSystem.GetTile(moveStartTile).linkedEntity != null) gridSystem.GetTile(moveStartTile).linkedEntity.AttackQueued(false);
                }*/
                
                if (isMoving)
                {
                    hoveredEntity = gridSystem.GetTile(moveStartTile).linkedEntity;
                    gridSystem.HighlightSquaresInRange(hoveredEntity.Position,
                        hoveredEntity.MoveRange + hoveredEntity.AttackRange, Color.white);
                    if (hoveredTile != moveStartTile)
                    {
                        gridSystem.GetTile(moveStartTile).linkedEntity.MoveQueued(true);
                        
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
                    gridSystem.SetColor(hoveredTile,
                        new Color(0.8f, 0.8f, 0.8f));
                }
                else
                {
                    if (hoveredEntity != null && hoveredEntity.hasQueuedMovement)
                    {
                        attackMode = true;
                        moveStartTile = hoveredTile;
                    }
                    else
                    {
                        attackMode = false;
                        isAttacking = false;
                    }

                    if (isMoving && hoveredEntity == null)
                    {
                        gridSystem.GetTile(moveStartTile).linkedEntity.MoveQueued(false);
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
                gridSystem.SetColor(hoveredTile,
                    new Color(0.8f, 0.8f, 0.8f));


            if (hoveredEntity != null && !attackMode)
            {
                if (hoveredEntity.hasQueuedMovement)
                {
                    gridSystem.HighlightSquaresInRange(hoveredEntity.Position,
                        hoveredEntity.AttackRange, new Color(0.8f, 0.8f, 0.8f));
                }
                else
                {
                    gridSystem.HighlightSquaresInRange(hoveredEntity.Position,
                        hoveredEntity.MoveRange + hoveredEntity.AttackRange, new Color(0.8f, 0.8f, 0.8f));
                    gridSystem.HighlightSquaresInRange(hoveredEntity.Position, hoveredEntity.MoveRange,
                        new Color(0.7f, 0.7f, 0.7f));
                }
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
                if (currentEntity != null) gridSystem.HighlightSquaresInRange(currentEntity.Position, currentEntity.AttackRange,
                    new Color(0.8f, 0.8f, 0.8f));

                for (int i = 0; i < battleController.GetCharacters().Count; i++)
                {
                    Entity highlightedEntity = battleController.GetCharacterAt(i);
                    if (highlightedEntity != null)
                    {
                        if (highlightedEntity.isHuman != currentEntity.isHuman)
                        {
                            GameObject a = Instantiate(attackHighlightIcon, highlightedEntity.Position,
                                Quaternion.identity,
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
            }


            if (isMoving && pathLine.positionCount > 0)
            {
                Vector3 pos = pathLine.GetPosition(pathLine.positionCount - 1);
                gridSystem.SetColor(pos + new Vector3(0.5f, 0.5f),
                    new Color(0.5f, 0.5f, 0.5f));
                gridSystem.SetColor(moveStartTile,
                    new Color(0.5f, 0.5f, 0.5f));
            }
        }
        

        public void EndTurn()
        {
            foreach (Entity e in battleController.GetCharacters())
            {
                e.AttackQueued(false);
                e.MoveQueued(false);
            }
            battleController.EndTurn(attackList);
        }

        public void StartTurn()
        {
            isPlayerTurn = true;
            endTurnButton.SetActive(true);
        }
    }
}