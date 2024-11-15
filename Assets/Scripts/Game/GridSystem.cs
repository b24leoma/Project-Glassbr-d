using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem : MonoBehaviour
{
    [SerializeField] private GameObject gridSpace;
    [SerializeField] private LayerMask layerMask;
    void OnMouseOver()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity,layerMask);
        if (hit.collider != null)
        {
            Debug.Log(hit.point);
            Vector2 pos = Vector2.Scale(transform.InverseTransformPoint(hit.point),transform.localScale);
            gridSpace.transform.position =
                new Vector3(Mathf.Round(pos.x + 0.5f) - 0.5f, Mathf.Round(pos.y + 0.5f) - 0.5f, -6);
        }
    }
}
