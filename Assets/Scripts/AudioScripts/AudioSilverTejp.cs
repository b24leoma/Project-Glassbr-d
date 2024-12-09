using UnityEngine;

public class AudioSilvertejp : MonoBehaviour
{
   [SerializeField] private VolumeController volumeController;
   [SerializeField] private GameObject audioManager;
   private void Start()
   {
       if (audioManager || volumeController == null)
       {


           audioManager = GameObject.Find("AudioManager");
           volumeController = audioManager.GetComponent<VolumeController>();
           if (audioManager || volumeController == null)
           {
               Debug.Log("No sound, please contact your local IT-Expert or just ignore :)");
               
               //this wont work men jag har två hjärnceller :) 
           }
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
