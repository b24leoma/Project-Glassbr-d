using UnityEngine;
using UnityEngine.UIElements;

public class AbilityButton : UIVisualElement
{
    protected override string target => "AbilityButton";
    
    protected override void OnClick(Button button)
    {
        Debug.Log($"Ability button clicked: {button.name}");
    }

    protected override void OnEnter(VisualElement uiElement)
    {
        Debug.Log($"Hovering over ability button: {uiElement.name}");
    }

    protected override void OnLeave(VisualElement uiElement)
    {
        Debug.Log($"Stopped hovering over ability button: {uiElement.name}");
    }

}

   
