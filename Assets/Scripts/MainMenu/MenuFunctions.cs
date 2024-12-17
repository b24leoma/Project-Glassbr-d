using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuFunctions : MonoBehaviour
{
    [SerializeField] private Animator settingsAnimator;
    private void Start()
    {
        
    }

    public void PlayIntro()
    {
        SceneManager.LoadScene("IntroHistoryBook");
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

    public void StartGame()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
