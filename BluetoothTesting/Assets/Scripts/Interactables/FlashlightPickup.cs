using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemHighlight))]
public class FlashlightPickup : MonoBehaviour
{
    [SerializeField] private GameObject flashlight;
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
            itemHighlight.ApplyHighlight();
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            itemHighlight.RemoveHighlight();
            playerInRange = false;
        }
    }

    private void Update()
    {
            
        if (Input.GetKeyDown(KeyCode.E) && playerInRange)
        {
            itemHighlight.RemoveHighlight();
            flashlight.SetActive(true);
            
            Destroy(gameObject);
        }
    }
}
