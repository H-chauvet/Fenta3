using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    [SerializeField]private AudioClip woodSound;
    [SerializeField]private AudioClip metalSound;

    private AudioSource woodSurfaceAudio;
    private AudioSource metalSurfaceAudio;

    private bool pulaCaLemnu;
    private bool pulaCaTeava;
    
    public PlayerMovement playerMovement;

    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();

        // Assign the AudioSource components
        woodSurfaceAudio = gameObject.AddComponent<AudioSource>();
        woodSurfaceAudio.clip = woodSound;
        //woodSurfaceAudio.Play();
        metalSurfaceAudio = gameObject.AddComponent<AudioSource>();
        metalSurfaceAudio.clip = metalSound;
        //metalSurfaceAudio.Play();
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.W)){
            if(pulaCaLemnu){
                Debug.Log("w is pressed");
                
                woodSurfaceAudio.Play();
            }
            else if(pulaCaTeava){
                metalSurfaceAudio.Play();
            }
        }
        if(Input.GetKeyUp(KeyCode.W))
            {
                if(pulaCaLemnu){
                Debug.Log("w is released");
                woodSurfaceAudio.Stop();
            }
                
            }
        
    }

    void OnCollisionStay(Collision col)
    {
        Debug.Log(col.gameObject);
        if(col.gameObject.CompareTag("wood"))
        {
            pulaCaLemnu = true;
            pulaCaTeava = false;
            // if(Input.GetKeyDown(KeyCode.W))
            // {
                
            // }
            
        }
        else if(col.gameObject.CompareTag("metal"))
        {
            pulaCaTeava = true;
            pulaCaLemnu = false;
                Debug.Log("fututten nas");
            if(playerMovement.characterController.velocity.x != 0)
            {
            }
        }
        else{
            pulaCaLemnu = false;
            pulaCaTeava = false;
            Debug.Log("Nu e pulaaa");
        }
    }
}
