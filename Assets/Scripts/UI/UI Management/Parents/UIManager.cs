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
    private Dictionary<string, (Component component, MethodInfo method)> _uiMethodDictionary;

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

        if (_uiDocument == null)
        {
            _uiDocument = GetComponent<UIDocument>();
        }

        _uiMethodDictionary ??= new Dictionary<string, (Component, MethodInfo)>();
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
            case Entity.EntityType.HumanArcher:
                break;
            case Entity.EntityType.HumanSpearman:
                break;
            case Entity.EntityType.HumanTank:
                break;
            // case Entity.EntityType.HumanMage: MageUI(); break;
        }
    }

    private void InitUI()
    {
        

        var uiElementList = _uiDocument.rootVisualElement.Query<VisualElement>().ToList();
        if (uiElementList.Count == 0) return;

        FindAllUIScripts();
    }

    private void FindAllUIScripts()
    {
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        foreach (var assembly in assemblies)
        {
            var componentNames = assembly.GetTypes()
                .Where(type => type.IsSubclassOf(typeof(UIInteractable)) || type.IsSubclassOf(typeof(UINameMethod)) ||
                               type.IsSubclassOf(typeof(UIClassMethod)) && !type.IsAbstract);

            foreach (var componentName in componentNames)
            {
                {
                    var component = gameObject.AddComponent(componentName);
                    var componentMethods = componentName.GetMethods(BindingFlags.Public | BindingFlags.Instance)
                        .Where(method => method.DeclaringType == componentName && !method.IsSpecialName);

                    foreach (var componentMethod in componentMethods)
                    {
                        var dictName = $"{componentName}.{componentMethod.Name}";
                        if (!_uiMethodDictionary.ContainsKey(dictName))
                        {
                            _uiMethodDictionary[dictName] = (component, componentMethod);
                            Debug.Log($"Adding method: {dictName}");
                        }
                    }
                }
            }
        }
    }

    public void RunThis(string scriptdotmethod)
    {
        if (!_uiMethodDictionary.TryGetValue(scriptdotmethod, out var value)) return;
        var (component, method) = value;

        if (component == null || method == null)
        {
            Debug.LogError($"Script {scriptdotmethod} has not been found!");
            return;
        }

        Debug.Log($"Ran {scriptdotmethod}!");
        method.Invoke(component, null);
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
}

public delegate void EndTurn();