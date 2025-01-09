using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class MoveUI : MonoBehaviour
{
    [SerializeField] private Vector2 targetPosition;
    [SerializeField] private float duration;
    [SerializeField] private Ease easeType = Ease.Linear;
    [SerializeField] private UnityEvent MoveComplete;
    private RectTransform rt;
    private void Start()
    {
        rt = GetComponent<RectTransform>();
    }

    public void StartMove()
    {
        rt.DOAnchorPos(targetPosition, duration).SetEase(easeType).OnComplete(() => MoveComplete?.Invoke());
    }
}