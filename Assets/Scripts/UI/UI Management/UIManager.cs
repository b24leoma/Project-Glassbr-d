using System;
using Game;
using Unity.VisualScripting;
using UnityEngine;

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

    


  


    /// <summary>
    ///  Faktiska logiken och metoderna
    /// </summary>
    public void OnClick()
    {
        Console.WriteLine("UI Clicked!");
    }

    public void OnEnter()
    {
        Console.WriteLine("Mouse Entered!");
    }

    public void OnExit()
    {
        Console.WriteLine("Mouse Exited!");
    }

    public void UpdateCharacterDisplay(bool arg1, Entity arg2, bool arg3)
    {
        Console.WriteLine($"Character Display Updated: {arg1}, {arg2}, {arg3}");
    }

    public void OnDamageTaken(int damage, Entity entity)
    { 
        Console.WriteLine($"Damage Taken: {damage} by {entity.Name}");
    }

    public void OnDeath(int damage, Entity entity)
    {
        Console.WriteLine($"Entity {entity.Name} Died!");
    }

    public void OnWin(bool arg1, bool arg2)
    {
        Console.WriteLine($"Player Won: {arg1}, {arg2}");
    }

    public void OnLoss(bool arg1, bool arg2)
    {
        Console.WriteLine($"Player Lost: {arg1}, {arg2}");
    }

    public void OnTurnEnded()
    {
        Console.WriteLine("Turn Ended!");
    }

    public void OnPlayer1Turn()
    {
        Console.WriteLine("Player 1's Turn!");
    }

    public void OnPlayer2Turn()
    {
        Console.WriteLine("Player 2's Turn!");
    }
}