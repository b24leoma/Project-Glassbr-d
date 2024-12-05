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
 
    
    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    public void WinUI()
    {
        onWin.Invoke();
        TogglePanel();
    }
    
    public void LossUI()
    {
        onLoss.Invoke();
        TogglePanel();
    }

    private void TogglePanel()
    {
        bool isOpen = settingsAnimator.GetBool(IsOpen);
        
        settingsAnimator.SetBool(IsOpen, !isOpen);
    }

    public void OpenSettings()
    {
        onOpenSettings.Invoke();
        TogglePanel();
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    
    
    
}
