using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cinematictransition : MonoBehaviour
{
    [SerializeField] private Transform target;
    private GameObject player;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            player.GetComponent<PlayerMovement>().canMove = false;
            GoToCinematic(player);
        }
    }

    void GoToCinematic(GameObject _player)
    {
        Vector3 direction = target.position - _player.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        _player.transform.rotation = Quaternion.Lerp(_player.transform.rotation, targetRotation, 0.3f);
        
    }
}
