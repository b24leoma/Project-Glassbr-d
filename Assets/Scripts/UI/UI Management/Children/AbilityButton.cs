using System;
using UnityEngine.UIElements;

public class AbilityButton : UIInteractable
{
    protected override string targetClass => "AbilityButton";

    protected override void DoThisAfterInit()
    {
      /*   foreach (var element in TargetElementList)
        {
            var abilityNumber = ExtractNumberFromButtonName(element.name);
            Debug.Log(element.name + ": " + abilityNumber);
        }
        */
    }
    private static int ExtractNumberFromButtonName(string elementName)
    {
        var parts = elementName.Split(new[] { "AbilityButton" }, StringSplitOptions.None);
        if (parts.Length > 1 && int.TryParse(parts[1], out var abilityNumber))
        {
            return abilityNumber;
        }

        return 0;
    }


    protected override void OnClick(Button button)
    {
        var abilityNumber = ExtractNumberFromButtonName(button.name);
        UIManager.AbilityEvent(abilityNumber);
    }
}

   
