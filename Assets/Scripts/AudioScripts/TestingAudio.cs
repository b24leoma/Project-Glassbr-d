using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingAudio : MonoBehaviour
{

    public void TestPlay()
    {
        FMODManager.instance.OneShot("Hit", transform.position);
    }
}
