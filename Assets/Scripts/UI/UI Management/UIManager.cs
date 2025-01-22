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
        

       
        var assembly = Assembly.GetAssembly(typeof(UIVisualElement)); 
        var types = assembly.GetTypes(); 
        foreach (var type in types)
        {
            if (type.IsSubclassOf(typeof(UIVisualElement)) && !type.IsAbstract)
            {
              gameObject.AddComponent(type);
            }
        }
    }

  
}

public delegate void EndTurn();

    
