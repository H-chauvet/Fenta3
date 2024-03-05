using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLogic : MonoBehaviour
{
    public GameObject tiedDoor;
    private PickupLogic pickupLogic;
    private bool playerInRange;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered");
            pickupLogic = other.GetComponent<PickupLogic>();
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("Player exited");
        pickupLogic = null;
        playerInRange = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInRange)
        {
            pickupLogic.PickupItem(this);
        }
    }
}
