using System.Numerics;
using DG.Tweening;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class MoveElementIn : MonoBehaviour
{
    private Vector2 startposition;
    [SerializeField] Vector2 endPosition;
    [Range(0f, 10f), SerializeField] private float duration = 1;
    [SerializeField] private Canvas targetCanvas;

    void OnEnable()
    {
        startposition = gameObject.transform.localPosition;

        if (targetCanvas == null)
        {
            
            targetCanvas = FindObjectOfType<Canvas>();
            Debug.Log("Canvas was not assigned. Assigned: " + targetCanvas.name + "...");
        }

        if (endPosition == default)
        {
            if (targetCanvas != null)
            {
                 
                endPosition = targetCanvas.renderingDisplaySize / 2;
            }
        }
    }

    private void MoveToScreen()
    {
        transform.DOMove(new Vector3(endPosition.x, endPosition.y), duration);
    }

    private void MoveOffScreen()
    {
        transform.DOMove(new Vector3(startposition.x, startposition.y), duration);
    }

    public void ToggleMove()
    {
        Vector2 currentPosition = gameObject.transform.localPosition;
        if ( currentPosition == endPosition)
        {
            MoveOffScreen();
        }
        else
        {
            MoveToScreen();
        }
    }
}