using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayThis : MonoBehaviour
{
    public void PlayDefeatSound()
    {
        FMODManager.instance.OneShot("Lost");
    }
}
