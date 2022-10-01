using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class CompanionController : MonoBehaviour
{   
    //@@@@State@@@@
    private State state;
    private enum State
    {
        Idle,
        FollowPlayer,
        FollowDoor,
        HideCompanion,
        IsHide,
        Pet,
        Teleport,
        Tutorial,
        GoToLabyrinth,
        DoWaitAtLabyrinth,
    }
    //@@@@

    //@@@@Getter and Setter@@@@
    //Set in InLabyrinth
    private bool isHiding = false;
    public bool IsHiding { get => isHiding; set => isHiding = value; }
    
    //Set in InLabyrinth
    private bool inLabyrinth = false;
    public bool InLabyrinth { get => inLabyrinth; set => inLabyrinth = value; }

    //
    private bool doorIsOpen = false;
    public bool IsDoorOpen { get => doorIsOpen; set => doorIsOpen = value; }

    //
    private bool companionOnTrigger = false;
    public bool CompanionOnTrigger { get => companionOnTrigger; set => companionOnTrigger = value; }

    //
    private bool touchedChest = false;
    public bool TouchedChest { get => touchedChest; set => touchedChest = value; }
    
    private bool onPetting = false;
    public bool OnPetting {get => onPetting; set => onPetting = value; }

    private bool onBrushing = false;
    public bool OnBrushing { get => onBrushing; set => onBrushing = value; }

    private bool isHungry = false;
    public bool IsHungry {get => isHungry; set => isHungry = value; }
    //@@@@
    
    //@@@@Player@@@@
    [SerializeField] private NavMeshObstacle playerObstacle;

    //@@@@Companion@@@@
    private MusicPlayer musicPlayer;
    private AudioSource audioSource;
    private NavMeshAgent nav;
    [SerializeField] private float navSpeed;
    [SerializeField] private float navAccelerration;

    [Tooltip("Target the Companion looks at")]
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject goal;

    [Tooltip("Target the Companion follow")]
    [SerializeField] private Transform target;

    [Tooltip("Shows the loaded List of doors to follow")]
    [SerializeField] private List<GameObject> doors;
    private GameObject door;
    [Tooltip("Places the Companion may hide")]
    [SerializeField] private List<GameObject> hidingSpots;
    
    [SerializeField] private Animator animator;

    [SerializeField] private GameObject labyrinthPoint;
    [SerializeField] private Transform targetLabyrinthPoint;
    [SerializeField] private GameObject heart;
    [SerializeField] private GameObject smiley;
    [SerializeField] private GameObject sad;
    [SerializeField] private GameObject questionmark;
    [SerializeField] private GameObject party;
    private int randomAnimation;
    [SerializeField] private List<string> animationList;
    private bool animationPlay = false;
    
    //@@@@
    
    //@@@@PrefabsToSpawn@@@@
    //[SerializeField] private GameObject steak;
    [SerializeField] private GameObject tomato;
    [SerializeField] private GameObject pumpkin;
    [SerializeField] private GameObject cauliflower;
    [SerializeField] private GameObject salat;
    [SerializeField] private GameObject melon;
    //[SerializeField] private GameObject spawnpointSteak;
    [SerializeField] private GameObject spawnpointTomato;
    [SerializeField] private GameObject spawnpointPumpkin;
    [SerializeField] private GameObject spawnpointCauliflower;
    [SerializeField] private GameObject spawnpointSalat;
    [SerializeField] private GameObject spawnpointMelon;

    //@@@@Distances@@@@
    [SerializeField] private float tooClose = 3;
    [SerializeField] private float maxDistToDoor = 6;
    [SerializeField] private float howClose = 4;
    private float distToPlayer;
    private float distToHidingSpot;
    private float distToGoal;
    private float distToDoor;
    private float distToLabyrinthPoint;
    private float distToTriggerTiger;
    
    //[SerializeField] private Vector3 teleportPosCompanion = new Vector3(24,1,-6);
    [SerializeField]private Vector3 teleportPosCompanion;

    [Tooltip("Overrides Stopping Distance from NavMesh")]
    //Needs to be so small that the companion can access his trigger while the player is on the other trigger
    [SerializeField] private float navStoppingDistance = 3;
    [Tooltip("Overrides Stopping Speed from NavMesh")]
    //Needs to be so small that the companion can access his trigger while the player is on the other trigger
    
    private int randomIndex;
    
    
 
    private bool randomLock;

    [SerializeField] private List<DoorController> doorControllers;

    private void Start()
    {
        animationList.Add("Hungry");
        animationList.Add("Hungry1");
        playerObstacle = GameObject.FindGameObjectWithTag("Player").GetComponent<NavMeshObstacle>();
        nav = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player");
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        teleportPosCompanion = GameObject.Find("TeleportPosCompanion").transform.position;
        audioSource = GameObject.FindGameObjectWithTag("NPC").GetComponent<AudioSource>();
        musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>();
        audioSource.mute = true;
        audioSource.volume = 4.0f;
        
        foreach(GameObject hidingSpot in GameObject.FindGameObjectsWithTag("HidingSpot"))
        {
            hidingSpots.Add(hidingSpot);
        }

        foreach(GameObject door in GameObject.FindGameObjectsWithTag("Door") )
        {
            doors.Add(door);
              
        }
        state = State.Tutorial;
    }
    private void FixedUpdate()
    {  
        distToLabyrinthPoint = Vector3.Distance(labyrinthPoint.transform.position, transform.position);
        distToHidingSpot = Vector3.Distance(hidingSpots[randomIndex].transform.position, transform.position);
        distToGoal = Vector3.Distance(goal.transform.position, transform.position);
        distToPlayer = Vector3.Distance(target.position, transform.position);
        switch(state)
        {
            
            case State.Tutorial:
                DoTutorial();
                Debug.Log("State:Tutorial");
                break;
            
            case State.GoToLabyrinth:
                DoGoToLabyrinth();
                Debug.Log("State:GoToLabyrinth");
                break;
            
            case State.DoWaitAtLabyrinth:
                DoWaitAtLabyrinth();
                Debug.Log("State:DoWaitAtLabyrinth");
                break;

            case State.Idle:
                DoIdle();
                Debug.Log("State:Idle");
                break;

            case State.FollowPlayer:
                DoFollowPlayer();
                Debug.Log("State:FollowPlayer");
                break;
            
            case State.HideCompanion:
                DoHideCompanion();
                Debug.Log("State:HideCompanion");
                break;

            case State.IsHide:
                DoIsHide();
                Debug.Log("State:IsHide");
                break;
            
            case State.Pet:
                DoPet();
                Debug.Log("State:Pet");
                break;

            case State.FollowDoor:
                DoFollowDoor();
                Debug.Log("State:FollowDoor");
                break;
            
            case State.Teleport:
                DoTeleport();
                Debug.Log("State:Teleport");
                break;
        }
    }

    private void DoTutorial()
    {
        transform.LookAt(player.transform);
        //StartCoroutine(waitForAnimation("Wave", 10.0f));
        animator.Play("WaveLong");

        if(distToPlayer <= howClose)
        {
            state = State.GoToLabyrinth;
        }
    }

    private void DoGoToLabyrinth()
    {   
        nav.SetDestination(targetLabyrinthPoint.position);
        animator.Play("Walk");
        if(distToLabyrinthPoint < 1)
        {
            state = State.DoWaitAtLabyrinth;
        }
    }

    private void DoWaitAtLabyrinth()
    {
        transform.LookAt(player.transform);
        animator.enabled = true;
        animator.Play("ComeHere");

        if (distToPlayer <= howClose)
        {
            state = State.FollowPlayer;
        }
    }

    private void DoIdle()
    {
        transform.LookAt(player.transform);
        animator.enabled = true;
        if(isHungry && !animationPlay){
            animationPlay = true;
            randomAnimation = Random.Range(0, 2);
            animator.Play(animationList[randomAnimation]);
            StartCoroutine(WaitSad());
        }else{
            animator.Play("Idle");
        }
        

        if(distToPlayer <= howClose)
        {
            state = State.FollowPlayer;
        }

        if(touchedChest)
        {
            state = State.Teleport;   
        }
    }

    private void DoFollowPlayer()
    {
        nav.speed = navSpeed;
        nav.acceleration = navAccelerration;
        nav.stoppingDistance = navStoppingDistance;
        nav.SetDestination(target.position);
        transform.LookAt(player.transform);
        audioSource.mute = true;
        animator.enabled = true;
        if(distToPlayer < navStoppingDistance)
        {
            animator.Play("Idle");
        }
        else
        {
            animator.Play("Walk");
        }
        if(touchedChest)
        {
            state = State.Teleport;   
        }

        if(isHiding)
        {
            state = State.HideCompanion;   
        }
        foreach(GameObject door in doors)
        {
            

            distToDoor = Vector3.Distance(door.transform.position, transform.position);
            if(distToDoor <= maxDistToDoor)
            {    
                this.door = door;
                state = State.FollowDoor;
                  
            }     
        }
        if(distToPlayer <= tooClose && inLabyrinth == false)
        {
            state = State.Pet;
        }

               
    }
    private void DoFollowDoor(){
        
        nav.stoppingDistance = 0;
        GameObject triggerTiger = door.transform.GetChild(0).gameObject;
        nav.SetDestination(triggerTiger.transform.position);
        transform.LookAt(triggerTiger.transform);
        
        if (companionOnTrigger)
        {
            animator.Play("Idle");
        }
        else
        {
            animator.Play("Walk");
        }
        
        if(doorIsOpen && companionOnTrigger)
        {
            companionOnTrigger = false;
            doors.Remove(door);
            doorIsOpen = false;
        
            state = State.Idle;
        }     
    }
     private void DoHideCompanion()
    {
        if(randomLock == false)
        {  
            this.randomIndex = Random.Range(0,3);
            randomLock = true;
        }

        if (distToHidingSpot > 1)
        {
            animator.Play("Walk");
        }
        
        nav.SetDestination(hidingSpots[randomIndex].transform.position);
        nav.stoppingDistance = 0;
        nav.speed = navSpeed*5;
        nav.acceleration = navAccelerration*5;
        

        if(distToHidingSpot < 1)
        {
            randomLock = false;
            isHiding = false;
            audioSource.mute = false;
            
            state = State.IsHide;
        }
    }

    private void DoIsHide()
    {
        if(distToPlayer > howClose)
        {
            animator.Play("CloseEyes");
        }

        if(distToPlayer <= howClose)
        {
            StartCoroutine(WaitOpenEyes());
        }
    }

    private void DoPet()
    {
        animator.enabled = true;
        playerObstacle.enabled = false;

        if (onPetting == false && onBrushing == false)
        {
            if(isHungry){
                animator.Play("Hungry");
                StartCoroutine(WaitSad());
            }else{
                animator.Play("Idle");
            }
        }

        if(onPetting && inLabyrinth == false)
        {
            animator.Play("Enjoy");
            heart.SetActive(true);
            onPetting = false;
        }

        if (onBrushing)
        {
            StartCoroutine(WaitSmiley());
        }
        

        if(distToPlayer > tooClose)
        {
            heart.SetActive(false);
            playerObstacle.enabled = true;
            state = State.FollowPlayer;
        }
        if(touchedChest)
        {
            state = State.Teleport;   
        }
    }

    private void DoTeleport()
    {  
        //Instantiate(steak, spawnpointSteak.transform.position, Quaternion.identity);
        Instantiate(tomato, spawnpointTomato.transform.position, Quaternion.identity);
        Instantiate(cauliflower, spawnpointCauliflower.transform.position, Quaternion.identity);
        Instantiate(melon, spawnpointMelon.transform.position, Quaternion.identity);
        Instantiate(salat, spawnpointSalat.transform.position, Quaternion.identity);
        Instantiate(pumpkin, spawnpointPumpkin.transform.position, Quaternion.identity);
        touchedChest = false;
        nav.Warp(teleportPosCompanion);
        foreach(GameObject door in GameObject.FindGameObjectsWithTag("Door") )
        {
            doors.Remove(door);
              
        }
        foreach(GameObject door in GameObject.FindGameObjectsWithTag("Door") )
        {
            doors.Add(door);
            
        }
        foreach(DoorController doorController in doorControllers)
        {
            doorController.Reset = true;
        }
        
        Destroy(GameObject.FindGameObjectWithTag("Treasurechest"));
        inLabyrinth = false;
        
        state = State.Idle;
        
    }

    private IEnumerator WaitSmiley()
    {
        smiley.SetActive(true);
        yield return new WaitForSeconds(5.0f);
        smiley.SetActive(false);
        onBrushing = false;
    }

    private IEnumerator WaitSad(){
        sad.SetActive(true);
        yield return new WaitForSeconds(5.0f);
        sad.SetActive(false);
        animationPlay = false;

    }

    private IEnumerator WaitOpenEyes()
    {
        animator.Play("OpenEyes");
        yield return new WaitForSeconds(2.0f);
        state = State.Idle;
    }

    public void Clap()
    {
        animator.Play("Clap");
        musicPlayer.PlaySoundEffect("ClappingSound",20f);
    }

}
