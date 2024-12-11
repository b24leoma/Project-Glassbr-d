using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Vector2 = UnityEngine.Vector2;

public class MoveUIElementToggle : MonoBehaviour
{
    private Vector2 _startposition;
    [SerializeField] Vector2 targetPosition;
    [Range(0f, 10f), SerializeField] private float duration = 1;
    [SerializeField] private Canvas targetCanvas;
    private Vector2 _middleOfScreen;
    [SerializeField] private bool invertOrder;

    void OnEnable()
    {
        _startposition = gameObject.transform.localPosition;

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

    private void MoveToTarget()
    {
        transform.DOLocalMove(targetPosition, duration);
    }

    private void MoveToStart()
    {
        transform.DOLocalMove(_startposition, duration);
    }

    public void ToggleMove()
    {
        Vector2 currentPosition = gameObject.transform.localPosition;
        
        
        if ( currentPosition == targetPosition)
        {
            MoveToStart();
        }
        else
        {
            MoveToTarget();
        }
    }

    
}