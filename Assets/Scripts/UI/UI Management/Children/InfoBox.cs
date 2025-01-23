using UnityEngine;

public class InfoBox : UINameMethod
{
    protected override string targetName => "InfoBar";
    
    
    
    public void InfoBoxEnable()
    {
        Debug.Log("UI Manager InfoBoxEnable");
        TargetElement.RemoveFromClassList("hidden");
        TargetElement.AddToClassList("visible");
    }

    public void InfoBoxDisable()
    {
        Debug.Log("UI Manager InfoBoxDisable");

        TargetElement.RemoveFromClassList("visible");
        TargetElement.AddToClassList("hidden");
    }
    
}
