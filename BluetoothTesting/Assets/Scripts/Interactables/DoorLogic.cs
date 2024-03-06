using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemHighlight))]
public class DoorLogic : MonoBehaviour
{
    private PickupLogic pickupLogic;
    private ItemHighlight itemHighlight;
    private bool playerInRange;

    private void Start()
    {
        itemHighlight = GetComponent<ItemHighlight>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered");
            pickupLogic = other.GetComponent<PickupLogic>();
            if(pickupLogic.currentItem.tiedDoor == gameObject) itemHighlight.ApplyHighlight();
            
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player entered");
            pickupLogic = null;
            if(pickupLogic.currentItem.tiedDoor == gameObject) itemHighlight.RemoveHighlight();
            playerInRange = false;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInRange && pickupLogic.currentItem.tiedDoor == gameObject)
        {
            itemHighlight.RemoveHighlight();
            pickupLogic.UseItem(this);
        }
    }
}
