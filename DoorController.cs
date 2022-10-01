using UnityEngine;

//script handles door animation
public class DoorController : MonoBehaviour
{
    [SerializeField] private Animator doorAnimator; 
    [SerializeField] private float animatorSpeed = 3f;
    [SerializeField] private MusicPlayer musicPlayer;
    [SerializeField] CompanionController companionController;
    private bool open = false;
    private bool companionOnTrigger;
    public bool CompanionOnTrigger { get => companionOnTrigger; set => companionOnTrigger = value; }
    private bool reset;
    public bool Reset{get => reset; set => reset = value;}

    //resets the door
    private void FixedUpdate()
    {
        if(reset)
        {
            doorAnimator.Play("CloseDoor");
            reset = false;
        }
    }
    
    //opens the door
    private void OnTriggerStay (Collider other)
    {
        if (other.CompareTag("Player")){
            if (companionOnTrigger){
                if(!open)
                {
                    doorAnimator.speed = animatorSpeed;
                    musicPlayer.PlaySoundEffect("DoorSound",10f);
                    doorAnimator.Play("OpenDoor");
                    open = true;
                    companionController.IsDoorOpen = true;
                }
            }
        }   
    }
    //plays pressure plate sound
    private void OnTriggerExit(Collider other)
    {
       
        if (other.CompareTag("Player"))
        {
            open = false;
            musicPlayer.PlaySoundEffect("TriggerSound",10f);
        }
    }   
    //plays pressure plate sound
    private void OnTriggerEnter(Collider other)
    {
       if (other.CompareTag("Player"))
       {
            musicPlayer.PlaySoundEffect("TriggerSound",10f);
       }
    }   
}
