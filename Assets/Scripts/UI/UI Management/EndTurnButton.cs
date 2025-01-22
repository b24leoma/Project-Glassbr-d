using UnityEngine;
using UnityEngine.UIElements;

public class EndTurnButton : UIVisualElement
{
    protected override string targetClass => "EndTurnButton";
    
    
    protected override void OnClick(Button button)
    {
        UIManager.OnEndTurn();
    }

  
}



