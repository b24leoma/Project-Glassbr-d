using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayOnStart : MonoBehaviour
{

        void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            string currentScene = scene.name;

            switch (currentScene)
            {
                case "MainMenu":
                    FMODManager.instance.NewTimeline("Music");
                    break;

                case "IntroHistoryBook":
                    break;

                case "Game":
                    FMODManager.instance.SetParameter("Battle", 1f);
                    break;

                default:
                    Debug.LogWarning($"Scene '{currentScene}' not recognised in PlayOnStart.");
                    break;
            }
        }
}

