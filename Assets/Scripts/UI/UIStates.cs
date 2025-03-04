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
        if (gameEnded) return;
        switch (magicnumber)
        {
            case 0:
            {
                gameEnded = true;
                AnimateToggle();
                onLoss.Invoke();
                break;
            }

            case 1:
            {
                gameEnded = true;
                AnimateToggle();
                onWin.Invoke();
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
        if (!open && gameEnded) return;
        settingsAnimator.SetBool(IsOpen, open);
        Debug.Log($"Toggling panel. Open: {open}");
        if (open) onOpenSettings?.Invoke();
        else onCloseSettings?.Invoke();
    }

  
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void RetryLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ChangeLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    
    
}
