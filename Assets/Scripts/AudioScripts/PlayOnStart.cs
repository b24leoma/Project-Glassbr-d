using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayOnStart : MonoBehaviour
{
    void Start()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        switch (currentScene)
        {
            case "MainMenu":
                FMODManager.instance.Timeline("cutefluffyhampter");
                
                break;

            case "IntroHistoryBook":
                
                break;

            case "Game":
                FMODManager.instance.Timeline("kittycatwonderland");
               
                break;

            default:
                Debug.LogWarning($"Scene '{currentScene}' not recognised in PlayOnStart.");
                break;
        }
    }
}