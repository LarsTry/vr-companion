using UnityEngine;
using System.Collections;

//script plays random background music from a variety of audio clips
public class MusicPlayer : MonoBehaviour
{
    //declare CompanionController to access labyrinth bool
    private CompanionController companionController;
    [Tooltip("Time an audio clip fades out in seconds")]
    [SerializeField] private float fadeTime;
    [Tooltip("Music volume")] private float musicVolume;
    [Tooltip("Number of audio clips u can add to the Island playlist.")]
    [SerializeField] private AudioClip[] islandClips;
    [Tooltip("Number of audio clips u can add to the Labyrinth playlist.")]
    [SerializeField] private AudioClip[] labyrinthClips;
    //declare AudioSource and Audioclips
    private AudioSource audioSource;
    [SerializeField]private AudioClip doorSound;
    [SerializeField]private AudioClip triggerSound;
    [SerializeField]private AudioClip openTreasurechest;
    [SerializeField]private AudioClip diggingSound;
    [SerializeField]private AudioClip pumpingSound;
    [SerializeField]private AudioClip harvestSound;
    [SerializeField]private AudioClip purrSound;
    [SerializeField]private AudioClip labyrinthCompleted;
    [SerializeField]private AudioClip clappingSound;
    private bool allowFading = false;
    private bool inIsland = true;
    
    void Start()
    {
        companionController = GameObject.FindGameObjectWithTag("NPC").GetComponent<CompanionController>();
        audioSource = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<AudioSource>();
        musicVolume = audioSource.volume;
        audioSource.loop = false;
    }
    
    //fades out current music and changes playlist on zone transition.
    void Update()
    {
        CheckZoneTransition();
        if(allowFading == true)
        {
            allowFading = false;
            StartCoroutine (FadeOut (audioSource, fadeTime));
        }
        //checks if an AudioSource is playing and if not attaches a new random clip to the AudioSource and plays it
        if (!audioSource.isPlaying)
        {
            audioSource.PlayOneShot(GetRandomClip());
            audioSource.volume = musicVolume;
        }
    }

    //gets random clip out of the playlist, based on the current zone
    private AudioClip GetRandomClip()
    {
        if(companionController.InLabyrinth == false)
        {
            return islandClips[Random.Range(0, islandClips.Length)];
        } 
        return labyrinthClips[Random.Range(0, labyrinthClips.Length)];
    }
    
    //fades out the AudioClip till the volume is zero in FadeTime seconds
    public IEnumerator FadeOut (AudioSource audioSource, float FadeTime) 
    {
        musicVolume = audioSource.volume;

        while (audioSource.volume > 0) {
        
            audioSource.volume -= musicVolume * Time.deltaTime / FadeTime;

            yield return null;
        }
        audioSource.Stop ();
        audioSource.volume = musicVolume;
    }

    //checks if there is a transition between the island and labyrinth zone and enables the audio fadeout
    private void CheckZoneTransition () 
    {
        if(companionController.InLabyrinth == true && inIsland == true)
        {
            inIsland = false;
            allowFading = true;
        }
        if (companionController.InLabyrinth == false && inIsland == false)
        {
            inIsland = true;
            allowFading = true;
        }
    }
    //plays sound effects 
    public void PlaySoundEffect(string name, float volume)
    {
        if (name == "DoorSound")
        {
           audioSource.PlayOneShot(doorSound, volume); 
        }  
        if (name == "TriggerSound")
        {
           audioSource.PlayOneShot(triggerSound, volume); 
        }  
        if (name == "OpenTreasurechest")
        {
            audioSource.PlayOneShot(openTreasurechest, volume); 
        }
        if (name == "DiggingSound")
        {
            audioSource.PlayOneShot(diggingSound, volume); 
        }
        if (name == "PumpingSound")
        {
            audioSource.PlayOneShot(pumpingSound, volume); 
        }
        if (name == "HarvestSound")
        {
            audioSource.PlayOneShot(harvestSound, volume);
        }
        if (name == "LabyrinthCompleted")
        {
            audioSource.PlayOneShot(labyrinthCompleted, volume);
        }
        if (name == "ClappingSound")
        {
            audioSource.PlayOneShot(clappingSound, volume);
        }
        
        if (name == "PurrSound")
        {
            audioSource.PlayOneShot(purrSound, volume);
        }
    }
}

