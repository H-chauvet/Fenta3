using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cinematictransition : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private CanvasGroup panel;
    [SerializeField] private float cinMoveSpeed = 0.04f;
    [SerializeField] private float cinTurnSpeed = 0.035f;
    [SerializeField] private float POVincrement = 0.05f;
    [SerializeField] private float stoppingDistance = 2f;
    [SerializeField] private float distanceStartFade = 6f;
    [SerializeField] private float fadeIncrement = 0.1f;
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
        _player.transform.rotation = Quaternion.Lerp(_player.transform.rotation, targetRotation, cinTurnSpeed);
        if (distance < distanceStartFade)
        {
            panel.alpha += fadeIncrement;
        }
        if (distance > stoppingDistance)
        {
             _player.GetComponent<PlayerMovement>().characterController.Move(direction * cinMoveSpeed * Time.deltaTime);
            camera.fieldOfView += POVincrement;
        }

    }

    
}
