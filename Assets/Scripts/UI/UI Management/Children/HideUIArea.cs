using UnityEngine;
using UnityEngine.UIElements;

public class HideUIArea : UIInteractable
{
    protected override string targetClass => "HideUIArea";

    private bool _shouldHide;

    public void HideUI(bool shouldHide)
    {
        _shouldHide = shouldHide;
    }

    private void TryHideUI(bool shouldHide)
    {
            foreach (var targetElement in TargetElementList)
            {
                if (shouldHide)
                {
                    targetElement.RemoveFromClassList("visible");
                    targetElement.AddToClassList("hidden");
                }
                else
                {
                    targetElement.RemoveFromClassList("hidden");
                    targetElement.AddToClassList("visible");
                }
            }
    }
    



    protected override void OnEnter(VisualElement uiElement)
    {
        TryHideUI(_shouldHide);
    }
    
    protected override void OnLeave(VisualElement uiElement)
    {
        TryHideUI(false);
    }

    
    
}