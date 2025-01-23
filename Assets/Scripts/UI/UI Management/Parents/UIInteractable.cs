using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class UIInteractable : MonoBehaviour
{
    private UIDocument _uiDocument;

    protected virtual string targetClass => "";

    protected List<VisualElement> TargetElementList;

    private void Awake()
    {
        if (_uiDocument == null)
        {
            _uiDocument = GetComponent<UIDocument>();
        }
    }

    private void Start()
    {
        if (_uiDocument == null || string.IsNullOrEmpty(targetClass)) return;

        InitUIEvents();
    }

    private void InitUIEvents()
    {
        var uiElementList = _uiDocument.rootVisualElement.Query<VisualElement>().Class(targetClass).ToList();
        if (uiElementList.Count == 0) return;
        TargetElementList = uiElementList;

        foreach (var uiElement in uiElementList)
        {
            if (uiElement is Button button)
            {
                button.clicked += () => { OnClick(button); };
            }

            uiElement.RegisterCallback<PointerEnterEvent>(_ => OnEnter(uiElement));
            uiElement.RegisterCallback<PointerLeaveEvent>(_ => OnLeave(uiElement));
        }
    }

    protected virtual void OnClick(Button button)
    {
        Debug.Log($"Button clicked: {targetClass} : {button.name}");
    }

    protected virtual void OnEnter(VisualElement uiElement)
    {
        Debug.Log($"Hovering over {targetClass} : {uiElement.name}");
    }

    protected virtual void OnLeave(VisualElement uiElement)
    {
        Debug.Log($"Stopped hovering over {targetClass} : {uiElement.name}");
    }
}