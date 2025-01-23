using UnityEngine;
using UnityEngine.UIElements;

public class UINameMethod : MonoBehaviour
{
    private UIDocument _uiDocument;

    protected virtual string targetName => "";
    private VisualElement _targetElement;


    private void Awake()
    {
        if (_uiDocument == null )
        {
            _uiDocument = GetComponent<UIDocument>();
        }
    }
    
    
    private void Start()
    {
        if (_uiDocument == null || string.IsNullOrEmpty(targetName)) return;

        InitUIEvents();
    }

    private void InitUIEvents()
    {
        if (_uiDocument != null)
        {
            var uiElement = _uiDocument.rootVisualElement.Q(targetName);
            if (uiElement == null) return;


            _targetElement = uiElement;
        }
    }

    protected virtual void TargetNameDoThis1()
    {
        Debug.Log($"Ran void1 for "+ _targetElement);
    }

    protected virtual void TargetNameDoThis2()
    {
        Debug.Log($"Ran void2 for "+ _targetElement);
    }
    
}
