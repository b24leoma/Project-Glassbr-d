using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIClassMethod : MonoBehaviour
{
    protected UIDocument UIDocument;

    protected virtual string targetClass => "";
    protected List<VisualElement> TargetElementList;

    protected virtual void Awake()
    {
        if (UIDocument == null)
        {
            UIDocument = GetComponent<UIDocument>();
        }
    }

    public virtual void Start()
    {
        if (UIDocument == null || string.IsNullOrEmpty(targetClass)) return;

        InitUIEvents();
    }

    protected virtual void InitUIEvents()
    {
        if (UIDocument == null) return;
        var uiElementList = UIDocument.rootVisualElement.Query<VisualElement>().Class(targetClass).ToList();
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