using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    public AudioClip footstepSound;
    public AudioSource audioSource;
    

    public PlayerMovement playerMovement;
   

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        CheckMovementSound();
    }

    void CheckMovementSound()
    {
        if (Mathf.Abs(playerMovement.characterController.velocity.x) != 0)
        {
            // Play footstep sound
            audioSource.PlayOneShot(footstepSound);
            
        }
    }
}
