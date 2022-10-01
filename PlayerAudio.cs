using System.Linq;
using UnityEngine;

//modified audio script from the "Mini First Person Controller" asset
namespace MyStuff.MyScripts.Commented
{   
    //plays step sound while player is moving
    public class PlayerAudio : MonoBehaviour
    {
        [SerializeField] PlayerController playerController;
        [Tooltip("Sound source which should be played during player movement.")]
        [SerializeField] AudioSource movementAudio;
        [Tooltip("Minimum velocity for moving audio to play.")]
        [SerializeField] float velocityThreshold = .01f;
        
        //setup current and  last character position so u can calculate velocity in FixedUpdate()
        private Vector2 lastCharacterPosition;
        private Vector2 currentCharacterPosition => new Vector2(playerController.transform.position.x, playerController.transform.position.z);
        
        //generates AudioSource array
        private AudioSource[]  movingAudios => new AudioSource[] {movementAudio};

        private void Reset()
        {
            //setup connection to Player Controller
            playerController = GetComponentInParent<PlayerController>();
        }
        private void FixedUpdate()
        {
            //saves the current velocity
            var velocity = Vector3.Distance(currentCharacterPosition, lastCharacterPosition);
            
            //plays moving audio if the velocity threshold is exceeded
            if (velocity >= velocityThreshold)
            {
                SetPlayingMovingAudio(movementAudio);
            }
            else
            {
                SetPlayingMovingAudio(null);
            }
            // Remember lastCharacterPosition.
            lastCharacterPosition = currentCharacterPosition;
        }
        /// <summary>
        /// Pause all movingAudios and enforce play on audioToPlay.
        /// </summary>
        /// <param name="audioToPlay">Audio that should be playing.</param>
        private void SetPlayingMovingAudio(AudioSource audioToPlay)
        {
            // Pause all movingAudios.
            foreach (var audio in movingAudios.Where(audio => audio != audioToPlay && audio != null))
            {
                audio.Pause();
            }
            // Play audioToPlay if it was not playing.
            if (audioToPlay && !audioToPlay.isPlaying)
            {
                audioToPlay.Play();
            }
        }
    }
}

