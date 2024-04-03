using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    [SerializeField]private AudioClip woodSound;
    [SerializeField]private AudioClip metalSound;

    private AudioSource woodSurfaceAudio;
    private AudioSource metalSurfaceAudio;

    public bool pulaCaLemnu;
    public bool pulaCaTeava;
    
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
        
        
        if(Mathf.Abs(Input.GetAxis("Horizontal"))>Mathf.Epsilon || Mathf.Abs(Input.GetAxis("Vertical"))>Mathf.Epsilon){
            if(pulaCaLemnu){
                //Debug.Log("w is pressed");
                if(!woodSurfaceAudio.isPlaying)woodSurfaceAudio.Play();
                
            }
            else if(pulaCaTeava){
                if(!metalSurfaceAudio.isPlaying)metalSurfaceAudio.Play();
            }
        }
        if(Mathf.Abs(Input.GetAxis("Horizontal")) <= Mathf.Epsilon && Mathf.Abs(Input.GetAxis("Vertical"))<= Mathf.Epsilon)
            {
                //Debug.Log("w is released");
                if(pulaCaLemnu){
                woodSurfaceAudio.Stop();
                
            }else if(pulaCaTeava){
                metalSurfaceAudio.Stop();
            }
                
            }
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("Entering on Object: " + collision.gameObject);
        Debug.Log("Entered object has tag: " + collision.gameObject.tag);
    }
    
    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("Exiting Object: " + collision.gameObject);
        Debug.Log("Exited object has tag: " + collision.gameObject.tag);
    }

    void OnCollisionStay(Collision col)
    {
        //Debug.Log(col.gameObject);
        if(col.gameObject.CompareTag("wood"))
        {
            pulaCaLemnu = true;
            pulaCaTeava = false;
            metalSurfaceAudio.Stop();
            // if(Input.GetKeyDown(KeyCode.W))
            // {
                
            // }
            
        }
        else if(col.gameObject.CompareTag("metal"))
        {
            pulaCaTeava = true;
            pulaCaLemnu = false;
            woodSurfaceAudio.Stop();
               // Debug.Log("fututten nas");
            
        }
        else{
            pulaCaLemnu = false;
            pulaCaTeava = false;
            Debug.Log("Nu e pulaaa");
        }
    }
}