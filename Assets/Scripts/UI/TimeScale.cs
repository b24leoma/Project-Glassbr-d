using UnityEngine;

public class TimeScale : MonoBehaviour
{
    public void PauseTimeScale()
    {
        Time.timeScale = 0;
    }


    public void UnPauseTimeScale()
    {
        Time.timeScale = 1;
    }
}
