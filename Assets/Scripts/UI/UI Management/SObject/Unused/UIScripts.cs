using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;

[CreateAssetMenu(fileName = "UIScripts", menuName = "UI/UIScriptsData")]
public class UIScripts : ScriptableObject
{
    public List<UIInteractable> uiScripts = new List<UIInteractable>();

   
    public void FindAllUIScripts()
    {
        uiScripts.Clear();

       
        var assembly = Assembly.GetAssembly(typeof(UIInteractable)); 
        var types = assembly.GetTypes(); 
        foreach (var type in types)
        {
            if (type.IsSubclassOf(typeof(UIInteractable)) && !type.IsAbstract)
            {
                uiScripts.Add((UIInteractable)Activator.CreateInstance(type)); 
            }
        }
    }
}