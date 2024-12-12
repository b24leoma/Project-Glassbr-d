using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class MoveUIElementToggle : MonoBehaviour
{
    
    [Header ("Assignables")]
    [Tooltip("The canvas where the UI element resides. Used to calculate the middle of the screen if needed. May be ignored")]
    [SerializeField]
    private Canvas targetCanvas;
    
    [Tooltip("If targetObject is not set, targetObject = this object")]
    [SerializeField] GameObject targetObject;
    
    [Header("Positions")]
    [Tooltip("The starting position of the UI element in local coordinates. May be ignored")] [SerializeField]
    private Vector2 startPosition;

    [Tooltip("The target position the UI element will move to in local coordinates. May be ignored")] [SerializeField]
    private Vector2 targetPosition;

    
    [Header("Movement Settings")]
    [Tooltip("The time (in seconds) the movement will take. May be ignored")] [Range(0f, 10f), SerializeField]
    private float duration = 1;

    [Tooltip("The function that defines the motion style. May be ignored")] [SerializeField]
    private Ease easeType = Ease.Linear;

    [Tooltip("Should the first move be teleported? Redundant if duration = 0")]
    [SerializeField] private bool teleportOnStart;

    [Tooltip("Should targetPosition changes call movement? (Default: true)")]
    [SerializeField] private bool shouldMove = true;
    

    [Header("Events")]
    [Tooltip("Called after animation moved to target position")] [SerializeField]
    private UnityEvent onMovedToTarget;

    [Tooltip("Called after animation moved to start position")] [SerializeField]
    private UnityEvent onMovedToStart;

    private Vector2 _middleOfScreen;
    private bool _isFirstMove = true;

    private void OnEnable()
    {
        if (targetObject == null)
        {
            targetObject = gameObject;
        }
        
        
        
        
        if (startPosition == default)
        {
            startPosition = targetObject.GetComponent<RectTransform>().anchoredPosition;
        }

        if (targetCanvas == null)
        {
            targetCanvas = FindObjectOfType<Canvas>();
            Debug.Log("Canvas was not assigned. Assigned: " + targetCanvas.name + "...");
        }

        if (targetPosition == default)
        {
            if (targetCanvas != null)
            {
                TryGetMiddleOfScreen();
            }
        }
    }

    private void TryGetMiddleOfScreen()
    {
        RectTransform canvasRect = targetCanvas.GetComponent<RectTransform>();
        _middleOfScreen = canvasRect.rect.center;
        targetPosition = _middleOfScreen;
    }

    private void MoveToPosition(Vector2 position)
    {
        float durationVar = teleportOnStart && _isFirstMove ? 0 : duration;

        targetObject.GetComponent<RectTransform>().DOAnchorPos(position, durationVar).SetEase(easeType).OnComplete(() => MoveCompleted(position));
        _isFirstMove = false;
    }

    

    public void ToggleMove()
    {
        Vector2 currentPosition = targetObject.GetComponent<RectTransform>().anchoredPosition;

        MoveToPosition(currentPosition == targetPosition ? startPosition : targetPosition);
    }

    
    
    public void TargetPositionIsThis(Vector2 moveHere)
    {
          
        
        targetPosition = moveHere;
        

        if (shouldMove)
        {
            MoveToPosition(targetPosition);
        }
    }

    public void InformationBroker(GameObject sentObject)
    {
        RectTransform canvasRect  = targetCanvas.GetComponent<RectTransform>();
        RectTransform sentRect = sentObject.GetComponent<RectTransform>();
        
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(targetCanvas.worldCamera, sentRect.position);

        
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRect, screenPoint, targetCanvas.worldCamera, out Vector2 localPoint))
        {
            TargetPositionIsThis(localPoint);
        }
        else
        {
            Debug.LogError("Failed to convert screen point to local point");
        }
    }
    





    



   
    

    
    private void MoveCompleted(Vector2 position)
    {
        if (position == targetPosition)
            onMovedToTarget.Invoke();
        else
            onMovedToStart.Invoke();
    }

  
}