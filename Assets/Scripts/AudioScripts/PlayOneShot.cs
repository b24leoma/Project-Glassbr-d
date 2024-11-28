using FMODUnity;
using UnityEngine;




public class PlayOneShot : MonoBehaviour
{
 [SerializeField] private EventReference Testing;


 public void PlaySfx ()
 {
  RuntimeManager.PlayOneShot(Testing);
 }


}
