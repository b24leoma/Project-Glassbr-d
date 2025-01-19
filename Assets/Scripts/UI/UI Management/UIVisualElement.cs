using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class UIVisualElement : MonoBehaviour
{
    private UIDocument _uiDocument;


    protected virtual string target => "";


    private void Start()
    {
        _uiDocument = GetComponent<UIDocument>();

        if (_uiDocument == null) return;

        InitUIEvents();



    }

    private void InitUIEvents()
    {
        var uiElementList = _uiDocument.rootVisualElement.Query<VisualElement>().ToList();
        foreach (var uiElement in uiElementList.Where(ShouldManage))
        {
            if (uiElement is Button button)
            {
                button.clicked+= () => { OnClick(button); };
            }
            uiElement.RegisterCallback<PointerEnterEvent>(_ => OnEnter(uiElement));
            uiElement.RegisterCallback<PointerLeaveEvent>(_ => OnLeave(uiElement));
        }
    }

    protected virtual bool ShouldManage(VisualElement uiElement)
    {
        return uiElement.name.Contains(target);
    }
    protected abstract void OnClick(Button button);
    protected abstract void OnEnter(VisualElement uiElement);
    protected abstract void OnLeave(VisualElement uiElement);

}

