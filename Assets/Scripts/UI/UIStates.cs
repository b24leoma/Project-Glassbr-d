using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UIStates : MonoBehaviour
{
    private static readonly int IsOpen = Animator.StringToHash("isOpen");
    [SerializeField] private Animator settingsAnimator;

    [SerializeField] UnityEvent onWin;
    [SerializeField] UnityEvent onLoss;
    [SerializeField] UnityEvent onOpenSettings;
    [SerializeField] UnityEvent onCloseSettings;
    [SerializeField] private bool gameEnded;


    
    
    
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
    
    
     
    

    public void TogglePanel(int magicnumber)
    {
        switch (magicnumber)
        {
            case 0:
            {
                gameEnded = true;
                onLoss.Invoke();
                AnimateToggle();
                break;
            }

            case 1:
            {
                gameEnded = true;
                onWin.Invoke();
                AnimateToggle();
                break;
            }
            case 3:
            {
                AnimateToggle();
                break;
            }
                    
        }
        
        
        
        
        
    }

    private void AnimateToggle()
    {
        bool open = !settingsAnimator.GetBool(IsOpen);
        settingsAnimator.SetBool(IsOpen, open);
        if (open) onOpenSettings?.Invoke();
        else onCloseSettings?.Invoke();
    }

  
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene("Game");
    }

    public void ChangeLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    
}
