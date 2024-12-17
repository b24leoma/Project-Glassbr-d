using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneHandler : MonoBehaviour
{
   
  

    void Start()
    {
       var scene = SceneManager.GetActiveScene();
       
       Debug.Log(scene.name + " - " + scene.buildIndex);
    }

    public void NextScene()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex >= SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }
        
        SceneManager.LoadScene(nextSceneIndex);
    }

    public void PreviousScene()
    {
        int previousSceneIndex = SceneManager.GetActiveScene().buildIndex - 1;

        if (previousSceneIndex < 0)
        {
            previousSceneIndex = SceneManager.sceneCountInBuildSettings -1;
        }    
        SceneManager.LoadScene(previousSceneIndex);
    }
    
    
    


}
