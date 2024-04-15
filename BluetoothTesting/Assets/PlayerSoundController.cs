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
    
   

    void Start()
    {

        // Assign the AudioSource components
        woodSurfaceAudio = gameObject.AddComponent<AudioSource>();
        woodSurfaceAudio.clip = woodSound;
 
        metalSurfaceAudio = gameObject.AddComponent<AudioSource>();
        metalSurfaceAudio.clip = metalSound;

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
            //Debug.Log("Nu e pulaaa");
        }
    }
}