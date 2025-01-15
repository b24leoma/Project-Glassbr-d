using UnityEngine;

public class OnHoverSelection : MonoBehaviour
{
    [SerializeField] private MoveUIElementToggle moveUI;
    public void MouseEnter()
    {
        if (moveUI != null)
        {
            moveUI.InformationBroker(gameObject);
        }
        else
        {
            Debug.LogWarning("No menuSelect or moveUI assigned");
        }



    }
    
    
    
    
}
