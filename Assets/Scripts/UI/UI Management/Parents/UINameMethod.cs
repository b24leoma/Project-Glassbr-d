using UnityEngine;
using UnityEngine.UIElements;

public class UINameMethod : MonoBehaviour
{
    private UIDocument _uiDocument;

    protected virtual string targetName => "";
    protected VisualElement TargetElement;


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


            TargetElement = uiElement;
        }
    }

    public virtual void DoThis()
    {
        Debug.Log($"Ran void for "+ TargetElement);
    }
    public virtual void DoThis1()
    {
        Debug.Log($"Ran void1 for "+ TargetElement);
    }

    public virtual void DoThis2()
    {
        Debug.Log($"Ran void2 for "+ TargetElement);
    }
    
}
