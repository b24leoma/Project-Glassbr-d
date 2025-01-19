using Game;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }
    

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
    }


    private void Start()
    {
        
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
    
    public void MageUI()
    {
        
    }
    
    
}

    
