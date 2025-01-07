using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayThis : MonoBehaviour
{
    public void PlayDefeatSound()
    {
        FMODManager.instance.OneShot("Lost");
    }

    public void PlayUISelect()
    {
        FMODManager.instance.OneShot("UISelect");
    }

    public void PlayUIClick()
    {
        FMODManager.instance.OneShot("UIClick");
    }

    public void PlayPageFlip()
    {
        FMODManager.instance.OneShot("PageFlip");
    }
    }
