using UnityEngine;

//script for the trigger at the labyrinth exit
public class NotInLabyrinth : MonoBehaviour
{
   private CompanionController companionController;
   
    private void Start()
    {
        companionController = GameObject.FindGameObjectWithTag("NPC").GetComponent<CompanionController>();
    }
   private void OnTriggerEnter (Collider other)
    {
        if(other.gameObject.CompareTag("Player")){
           companionController.InLabyrinth = false;
        }
    }
}
