using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Cinematictransition : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float cinMoveSpeed;
    [SerializeField] private float cinTurnSpeed;
    private bool hasEntered;
    private GameObject player;
    private Camera camera;
   

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            player.GetComponent<PlayerMovement>().canMove = false;
            Vector3 newPosition = transform.position;
            newPosition.y = player.transform.position.y;
            transform.position = newPosition;
            hasEntered = true;
            camera = player.GetComponentInChildren<Camera>();
            
        }
    }

    void FixedUpdate()
    {
        if (hasEntered)
        {
            GoToCinematic(player);
        }
    }
    
    void GoToCinematic(GameObject _player)
    {
        Vector3 direction = target.position - _player.transform.position;
        float distance = Vector3.Distance(_player.transform.position, target.position);
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        _player.transform.rotation = Quaternion.Lerp(_player.transform.rotation, targetRotation, 0.035f);
        if (distance > 2f)
        {
             _player.GetComponent<PlayerMovement>().characterController.Move(direction * 0.04f * Time.deltaTime);
            camera.fieldOfView += 0.05f;
            
            
        }

    }

    
}
