using FMOD.Studio;
using UnityEngine;

public class VolumeController : MonoBehaviour
{
    private VCA _masterVca;
    private VCA _musicVca;
    private VCA _sfxVca;
    private VCA _ambienceVca;
    
    
 [Range(0f, 1f)] public float masterVolume = 1f;
 [Range(0f, 1f)] public float musicVolume = 1f;
 [Range(0f, 1f)] public float sfxVolume = 1f;
 [Range(0f, 1f)] public float ambienceVolume = 1f;

    private void Start()
    {
        _masterVca = FMODUnity.RuntimeManager.GetVCA("vca:/Master");
        _musicVca = FMODUnity.RuntimeManager.GetVCA("vca:/Music");
        _sfxVca = FMODUnity.RuntimeManager.GetVCA("vca:/SFX");
        _ambienceVca = FMODUnity.RuntimeManager.GetVCA("vca:/Ambience");
        
        UpdateVca();
        

    }

    private void OnValidate()
    {
        UpdateVca();
    }

    private void UpdateVca()
    {
        if (_masterVca.isValid()) _masterVca.setVolume(masterVolume);
        if (_musicVca.isValid()) _musicVca.setVolume(musicVolume);
        if (_sfxVca.isValid()) _sfxVca.setVolume(sfxVolume);
        if (_ambienceVca.isValid()) _ambienceVca.setVolume(ambienceVolume);
    }
    
    
    //For UI 



    public float UpdateUI(int magicnumber)
    {
        if (!_masterVca.isValid() || !_musicVca.isValid() || !_sfxVca.isValid() || !_ambienceVca.isValid())
        {
            Debug.LogError("One or more VCA references are invalid.");
            return -1f;
        }
        float  currentVolume = 0;
        
            switch (magicnumber)
            {
                
                case 0:
                    currentVolume = masterVolume;
                    break;
                case 1 :
                    currentVolume = musicVolume;
                    break;
                    
                case 2 :
                    currentVolume = sfxVolume;
                    break;
                case 3 : 
                    currentVolume = ambienceVolume;
                    break;
                default:
                    Debug.LogWarning("Invalid magic number");
                    break;
                    


            }
        
        return currentVolume;

    }
    
    
    
    
    
    public void SetMasterVolume(float value)
    {
        masterVolume = value;
        if (_masterVca.isValid()) _masterVca.setVolume(masterVolume);
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        if (_musicVca.isValid()) _musicVca.setVolume(musicVolume);
    }

    public void SetSfxVolume(float value)
    {
        sfxVolume = value;
        if (_sfxVca.isValid()) _sfxVca.setVolume(sfxVolume);
    }

    public void SetAmbienceVolume(float value)
    {
        ambienceVolume = value;
        if (_ambienceVca.isValid()) _ambienceVca.setVolume(ambienceVolume);
    }
}
