using UnityEngine;
using UnityEngine.UIElements;

public abstract class UIVisualElement : MonoBehaviour
{
    private UIDocument _uiDocument;

    protected virtual string targetClass => "";

    private void Awake()
    {
        if (_uiDocument == null )
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

    protected abstract void OnClick(Button button);
    protected abstract void OnEnter(VisualElement uiElement);
    protected abstract void OnLeave(VisualElement uiElement);
}