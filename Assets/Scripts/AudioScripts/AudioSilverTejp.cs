using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class AudioSilvertejp : MonoBehaviour
{
   [SerializeField] private VolumeController volumeController;
   [SerializeField] private GameObject audioManager;
   [FormerlySerializedAs("_masterVolumeUI")] [SerializeField]private Slider masterVolumeUI;
   [FormerlySerializedAs("_musicVolumeUI")] [SerializeField] private Slider musicVolumeUI;
   [FormerlySerializedAs("_sfxVolumeUI")] [SerializeField] private Slider sfxVolumeUI;
   private void Start()
   {
       if (audioManager || volumeController == null)
       {


           audioManager = GameObject.Find("AudioManager");
           volumeController = audioManager.GetComponent<VolumeController>();
           
           
       }

       if (masterVolumeUI == null)
       {
           masterVolumeUI = GameObject.Find("MasterVolumeSlider").GetComponent<Slider>();
          StartCoroutine(WaitAndSetVolume(0, 0.5f));

       }
       
       
       
       if (musicVolumeUI == null)
       {
           musicVolumeUI = GameObject.Find("MusicVolumeSlider").GetComponent<Slider>();
           StartCoroutine(WaitAndSetVolume(1, 0.5f));
       }
       
       if (sfxVolumeUI == null)
       {
          
           sfxVolumeUI = GameObject.Find("SFXVolumeSlider").GetComponent<Slider>();
          StartCoroutine(WaitAndSetVolume(2, 0.5f));
       }

       

   }



   private IEnumerator WaitAndSetVolume(int casenumber, float waitime)
   {
       yield return new WaitForSeconds(waitime);
       
       var volume = GetMyVolume(casenumber);
       SetMyVolume(casenumber, volume);
   }
   public float GetMyVolume(int casenumber)
   {
       if (volumeController == null)
       {
           Debug.Log("Could'nt return a value!");
           return -1;
       }

       var myvolume = volumeController.UpdateUI(casenumber);

       return myvolume;
   }

   public void SetMyVolume(int casenumber, float volume)
   {
       switch (casenumber)
       {
           case 0:
               masterVolumeUI.value = volume;
               break;
           case 1:
               musicVolumeUI.value = volume;
               break;
           case 2:
               sfxVolumeUI.value = volume;
               break;
       }
   }
   


   

   public void MasterMiddleman(float value)
   {
      float mastervolume = value;
      volumeController.SetMasterVolume(mastervolume);
     
   }
   
   public void MusicMiddleman(float value)
   {
       float musicvolume = value;
       volumeController.SetMusicVolume(musicvolume);
   }
   
   public void SfxMiddleman(float value)
   {
       float sfxvolume = value;
       volumeController.SetSfxVolume(sfxvolume);
   }
   
   public void AmbienceMiddleman(float value)
   {
       float ambiencevolume = value;
       volumeController.SetAmbienceVolume(ambiencevolume);

   }
      
      
}
