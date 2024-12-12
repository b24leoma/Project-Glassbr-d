using UnityEngine;

public class OnHoverSelection : MonoBehaviour
{
    [SerializeField] private RectTransform menuSelect;
    [SerializeField] private MoveUIElementToggle moveUI;
    public void MouseEnter()
    {
        if (menuSelect != null || moveUI != null)
        {
            menuSelect.position = GetComponent<RectTransform>().position;
            moveUI.InformationBroker(gameObject);
        }
        else
        {
            Debug.LogWarning("No menuSelect or moveUI assigned");
        }



    }
    
    
    
    
}
