using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonAction : MonoBehaviour
{
    [SerializeField] private Animator settingsAnimator;
    private void Start()
    {
        
    }

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
        settingsAnimator.SetBool("isOpen",true);
    }

    public void CloseSettings()
    {
        settingsAnimator.SetBool("isOpen",false);
    }
}
