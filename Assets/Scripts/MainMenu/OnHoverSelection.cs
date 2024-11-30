using UnityEngine;

public class OnHoverSelection : MonoBehaviour
{
    [SerializeField] private RectTransform menuSelect;
    public void MouseEnter()
    {
        menuSelect.position = GetComponent<RectTransform>().position;
    }
}
