using UnityEngine;

//script checks if companion is on a pressure plate and communicates it to the necessary scripts
public class DoorTriggerCompanion : MonoBehaviour
{
    
    [SerializeField] private DoorController doorController;
    [SerializeField] private CompanionController companionController;
    [SerializeField] private MusicPlayer musicPlayer;
    
    private void Start()
    {
        companionController = GameObject.FindGameObjectWithTag("NPC").GetComponent<CompanionController>();
        musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>();
    }

   private void OnTriggerEnter(Collider other)
    {   
        if(other.CompareTag("Tiger")){
            doorController.CompanionOnTrigger = true;
            companionController.CompanionOnTrigger = true;
            musicPlayer.PlaySoundEffect("TriggerSound",10f);
           
        }
    }
    private void OnTriggerExit(Collider other)
    {    
        if(other.CompareTag("Tiger"))
        {
            doorController.CompanionOnTrigger = false;
            companionController.CompanionOnTrigger = false;
            musicPlayer.PlaySoundEffect("TriggerSound",10f);
        }
    }
}
