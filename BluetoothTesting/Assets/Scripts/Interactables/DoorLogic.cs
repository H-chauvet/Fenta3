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
        AudioSource audi = new AudioSource();
        
        itemHighlight = GetComponent<ItemHighlight>();
        TCPChannelHandler.Instance.interactEvent += OnInteractEvent;
    }

    private void OnInteractEvent(bool isPressed)
    {
        HandleInteract(isPressed);
    }

    void OnDisable()
    {
        TCPChannelHandler.Instance.interactEvent -= OnInteractEvent;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player entered");
            pickupLogic = other.GetComponent<PickupLogic>();
            playerInRange = true;
            if (pickupLogic.currentItem == null) return;
            if(pickupLogic.currentItem.tiedDoor == gameObject) itemHighlight.ApplyHighlight();
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player entered");
            pickupLogic = null; 
            if(itemHighlight.isHighlighted)itemHighlight.RemoveHighlight();
            playerInRange = false;
        }
    }

    private void Update()
    {
        
         if (pickupLogic == null || pickupLogic.currentItem == null) return;
         if (Input.GetKeyDown(KeyCode.E) && playerInRange && pickupLogic.currentItem.tiedDoor == gameObject)
         {
             itemHighlight.RemoveHighlight();
             pickupLogic.UseItem(this);
         }
    }

    public void HandleInteract(bool isPressed)
    {
        if (isPressed == true)
        {
            if (pickupLogic == null || pickupLogic.currentItem == null) return;
            if (playerInRange && pickupLogic.currentItem.tiedDoor == gameObject)
            {
                itemHighlight.RemoveHighlight();
                pickupLogic.UseItem(this);
            }
        }
    }
}
