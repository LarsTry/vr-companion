using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class CollisionTiger : MonoBehaviour
{
    //[SerializeField] private MusicPlayer musicPlayer;
    [SerializeField] private CompanionController companionController;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip audioClip;
    
    void Start()
    {
        //musicPlayer = GameObject.FindGameObjectWithTag("MusicPlayer").GetComponent<MusicPlayer>();
        companionController = GameObject.FindGameObjectWithTag("NPC").GetComponent<CompanionController>();
    }

    private void OnCollisionStay(Collision other)
    {
        if(other.gameObject.CompareTag("RightHand") || other.gameObject.CompareTag("LeftHand"))
        {
            //StartCoroutine(Wait(2.0f));
            companionController.OnPetting = true;

            if (!audioSource.isPlaying)
            {
                audioSource.PlayOneShot(audioClip);
                Debug.Log("spieltSound");
            }
        }

        if (other.gameObject.CompareTag("Brush"))
        {
            companionController.OnBrushing = true;
        }
    }
    private IEnumerator Wait(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        
    }
}
