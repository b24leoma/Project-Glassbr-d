using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;

[CreateAssetMenu(fileName = "UIScripts", menuName = "UI/UIScriptsData")]
public class UIScripts : ScriptableObject
{
    public List<UIVisualElement> uiScripts = new List<UIVisualElement>();

   
    public void FindAllUIScripts()
    {
        uiScripts.Clear();

       
        var assembly = Assembly.GetAssembly(typeof(UIVisualElement)); 
        var types = assembly.GetTypes(); 
        foreach (var type in types)
        {
            if (type.IsSubclassOf(typeof(UIVisualElement)) && !type.IsAbstract)
            {
                uiScripts.Add((UIVisualElement)Activator.CreateInstance(type)); 
            }
        }
    }
}