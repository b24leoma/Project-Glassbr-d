using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeOnHover : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }
    public void QuitGame()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #endif
        Application.Quit();
    }

    public void OpenSettings()
    {
        
    }

    public void CloseSettings()
    {
        
    }
}
