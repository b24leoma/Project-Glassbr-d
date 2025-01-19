using System;
using Game;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class UIManager : MonoBehaviour
{
    public static UIManager instance { get; private set; }
    
    
    [SerializeField] private GameObject uiPrefab;
    private GameObject ui;

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
                ui = Instantiate(uiPrefab);
    }
}

    
