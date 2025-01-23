using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIClassMethod : MonoBehaviour
{
    private UIDocument _uiDocument;

    protected virtual string targetClass => "";
    protected List<VisualElement> TargetElementList;

    protected virtual void Awake()
    {
        if (_uiDocument == null)
        {
            _uiDocument = GetComponent<UIDocument>();
        }
    }

    public virtual void Start()
    {
        if (_uiDocument == null || string.IsNullOrEmpty(targetClass)) return;

        InitUIEvents();
    }

    protected virtual void InitUIEvents()
    {
        if (_uiDocument == null) return;
        var uiElementList = _uiDocument.rootVisualElement.Query<VisualElement>().Class(targetClass).ToList();
        if (uiElementList.Count == 0) return;

        TargetElementList = uiElementList;
    }

    public virtual void DoThis()
    {
        Debug.Log($"Ran void for " + targetClass);
    }

    public virtual void DoThis1()
    {
        Debug.Log($"Ran void1 for " + targetClass);
    }

    public virtual void DoThis2()
    {
        Debug.Log($"Ran void2 for " + targetClass);
    }
}