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
    private bool firstVines;
    private Animator c1a;
    private Animator c1b;
    private Animator c2a;
    private Animator c2b;
    private Animator c3a;
    private Animator c3b;

    private void OnEnable()
    {
        firstVines = true;
        c1a = transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        c1b = transform.GetChild(0).GetChild(1).GetComponent<Animator>();
        c2a = transform.GetChild(1).GetChild(0).GetComponent<Animator>();
        c2b = transform.GetChild(1).GetChild(1).GetComponent<Animator>();
        c3a = transform.GetChild(2).GetChild(0).GetComponent<Animator>();
        c3b = transform.GetChild(2).GetChild(1).GetComponent<Animator>();
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

    public void ActivateSelection(int choice)
    {
        switch (choice)
        {
            case 0:
                c1a.SetBool("FadeIn", true);
                c1b.SetBool("FadeIn", true);
                c2a.SetBool("FadeIn", false);
                c2b.SetBool("FadeIn", false);
                c3a.SetBool("FadeIn", false);
                c3b.SetBool("FadeIn", false);
                break;
            case 1:
                c1a.SetBool("FadeIn", false);
                c1b.SetBool("FadeIn", false);
                c2a.SetBool("FadeIn", true);
                c2b.SetBool("FadeIn", true);
                c3a.SetBool("FadeIn", false);
                c3b.SetBool("FadeIn", false);
                break;
            case 2:
                c1a.SetBool("FadeIn", false);
                c1b.SetBool("FadeIn", false);
                c2a.SetBool("FadeIn", false);
                c2b.SetBool("FadeIn", false);
                c3a.SetBool("FadeIn", true);
                c3b.SetBool("FadeIn", true);
                break;
        }
        float durationVar = 0;
        _isFirstMove = false;
    }

    public void InformationBroker(GameObject sentObject)
    {
        RectTransform canvasRect  = targetCanvas.GetComponent<RectTransform>();
        RectTransform sentRect = sentObject.GetComponent<RectTransform>();
        
        Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(targetCanvas.worldCamera, sentRect.position);
    }

    public void FadeThemOut()
    {
        c1a.SetBool("FadeIn", false);
        c1b.SetBool("FadeIn", false);
        c2a.SetBool("FadeIn", false);
        c2b.SetBool("FadeIn", false);
        c3a.SetBool("FadeIn", false);
        c3b.SetBool("FadeIn", false);
    }
    





    



   
    

    
    private void MoveCompleted(Vector2 position)
    {
        if (position == targetPosition) onMovedToTarget.Invoke();
        else onMovedToStart.Invoke();
    }

  
}