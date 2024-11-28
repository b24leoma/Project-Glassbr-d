using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
using FMODUnity;
using UnityEngine.Serialization;

public class VolumeController : MonoBehaviour
{
   [SerializeField] private VCA Master;
   [SerializeField] private VCA Music;
   [SerializeField] private VCA SFX;
   [SerializeField] private VCA Ambience;
    
 [Range (0, 100)]
    public int master;
   [Range (0, 100)]
    public int music;
   [Range (0, 100)]
    public int sfx;
[Range (0, 100)]
    public int ambience;

    private void Start()
    {
        master = 100;
        music = 100;
        sfx = 100;    
        ambience = 100;
        
    }
}
