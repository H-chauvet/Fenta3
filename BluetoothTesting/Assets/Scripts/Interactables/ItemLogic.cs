using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemHighlight))]
public class ItemLogic : MonoBehaviour
{
    public GameObject tiedDoor;
    private PickupLogic pickupLogic;
    private ItemHighlight itemHighlight;
    private bool playerInRange;

    private void Start()
    {
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
            itemHighlight.ApplyHighlight();
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
       //Debug.Log("Player exited");
        pickupLogic = null;
        itemHighlight.RemoveHighlight();
        playerInRange = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && playerInRange)
        {
            itemHighlight.RemoveHighlight();
            pickupLogic.PickupItem(this);
        }
    }

    public void HandleInteract(bool isPressed)
    {
        if (isPressed == true && playerInRange)
        {
            itemHighlight.RemoveHighlight();
            pickupLogic.PickupItem(this);
        }
    }
}
