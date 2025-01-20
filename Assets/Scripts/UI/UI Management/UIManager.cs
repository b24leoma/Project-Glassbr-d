using System.Collections.Generic;
using Game;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }
    private UIDocument _uiDocument;
    

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
        var uiElementList = _uiDocument.rootVisualElement.Query<VisualElement>().ToList();
        if (uiElementList.Count == 0) return;

        DisableRayCastsOnHidden(uiElementList);
    }

    private static void DisableRayCastsOnHidden(List<VisualElement> listOfTargets)
    {
        foreach (var uiElement in listOfTargets)
        {
            uiElement.pickingMode = uiElement.style.display == DisplayStyle.None ? PickingMode.Ignore : PickingMode.Position;
        }
    }
    
    
    public void MageUI()
    {
        
    }
    
    
}

    
