using System.Collections;
using UnityEngine;

namespace MyStuff.MyScripts
{
    //script handles treasure chest interaction
    public class TreasurechestController : MonoBehaviour
    {
        private MusicPlayer musicPlayer;
        private Animator animator;
        private InLabyrinth inLabyrinth;
        private CompanionController companionController;
        private SBowlScript sBowlScript;
        private bool treasureChestTouched = false;
        [SerializeField] private GameObject player;
        private CharacterController characterController;
        private Vector3 teleportPosPlayer;
        
    
        private void Start()
        {
            musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>();
            animator = GameObject.Find("Treasurechest(Clone)").GetComponent<Animator>();
            player = GameObject.FindGameObjectWithTag("Player");
            characterController = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
            inLabyrinth = GameObject.Find("EntryTrigger").gameObject.GetComponent<InLabyrinth>();
            companionController = GameObject.FindGameObjectWithTag("NPC").GetComponent<CompanionController>();
            sBowlScript = GameObject.FindGameObjectWithTag("FruitBowl").GetComponent<SBowlScript>();
            teleportPosPlayer = GameObject.Find("TeleportPosPlayer").transform.position;

        }
        
        //opens treasure chest
        private void OnCollisionEnter(Collision other)
        {
            if(other.gameObject.CompareTag("LeftHand") || other.gameObject.CompareTag("RightHand"))
            {
                if (!treasureChestTouched)
                {
                    treasureChestTouched = true;
                    animator.Play("OpenTreasurechest");
                    musicPlayer.PlaySoundEffect("OpenTreasurechest",10f);
                    //sBowlScript.spawnStrawberries();
                    StartCoroutine(Teleport());
                }
            }
        }
        
        //teleports player and companion into the garden and partial resets the Labyrinth for next use
        private IEnumerator Teleport()
        {
            yield return new WaitForSeconds(5);
            musicPlayer.PlaySoundEffect("LabyrinthCompleted", 10f);
            yield return new WaitForSeconds(10);
            companionController.IsHungry = true;
            characterController.enabled = false;
            player.gameObject.transform.position = teleportPosPlayer;
            companionController.TouchedChest = true;
            inLabyrinth.SpawnLock = false;
            companionController.InLabyrinth = true;
            characterController.enabled = true;
        }
    }
}
