using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Game;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }
    private UIDocument _uiDocument;
    public static event EndTurn EndTurnEvent;
    [SerializeField] private UIScripts uiScripts;

    private VisualElement _infoBox;
    private Dictionary<string, UINameMethod> _nameMethodDictionary;
    private List <Component> _componentList;
    private List <MethodInfo> _methodList;
    private Dictionary<string, string> _componentMethodDictionary;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.Log("UI Manager instance already exists! Destroying...");
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
        
            if (_uiDocument == null )
            {
                _uiDocument = GetComponent<UIDocument>();
            }
    }

      

    


    private void Start()
    {
        
        if (_uiDocument == null) return;

        InitUI();
    }


    public void PlayerUnitUI(Entity entity)
    {
        switch (entity.Type)
        {
            case Entity.EntityType.HumanArcher: break;
            case Entity.EntityType.HumanSpearman: break;
            case Entity.EntityType.HumanTank: break;
            // case Entity.EntityType.HumanMage: MageUI(); break;
            
        }
    }
    
    
    private void InitUI()
    {
        _infoBox = _uiDocument.rootVisualElement.Query("InfoBar").First();
        InfoBoxDisable();
        
        
        
        var uiElementList = _uiDocument.rootVisualElement.Query<VisualElement>().ToList();
        if (uiElementList.Count == 0) return;
        
        FindAllUIScripts();
    }


    public void RunThis(string scriptName, string methodName)
    {
        var targetComponent = GetComponent(scriptName);
        if (targetComponent== null)
        {
            Debug.Log($"{scriptName} not found!");
            return;
        }
        _componentList.Add(targetComponent);
        
        var targetMethod = targetComponent.GetType().GetMethod(methodName);
        if (targetMethod == null)
        {
            Debug.Log($"{methodName} not found!");
            return;
        }
        _methodList.Add(targetMethod);
        
        
        
        targetMethod.Invoke(targetComponent, null);
        
        
        
        
    }
    
    
    
    public void InfoBoxEnable()
    {
        Debug.Log("UI Manager InfoBoxEnable");
        _infoBox.RemoveFromClassList("hidden");
        _infoBox.AddToClassList("visible");

    }
    
    public void InfoBoxDisable()
    {
        Debug.Log("UI Manager InfoBoxDisable");
        
        _infoBox.RemoveFromClassList("visible");
        _infoBox.AddToClassList("hidden");
     
    }
    
    public void MageUI()
    {
        
    }



    public static void WhenPlayerEndTurn()
    {
        
    }


    public static void WhenPlayerStartTurn()
    {
        
    }


    public static void OnEndTurn()
    {
        EndTurnEvent?.Invoke();
    }

    private void FindAllUIScripts()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();


        foreach (var assembly in assemblies)
        {
            var types = assembly.GetTypes().Where(type =>
                type.IsSubclassOf(typeof(UIVisualElement)) || type.IsSubclassOf(typeof(UINameMethod)) && !type.IsAbstract);
            
            foreach (var type in types)
            {
                {
                    gameObject.AddComponent(type);
                }
            }
        }






    }

  
}

public delegate void EndTurn();

    
