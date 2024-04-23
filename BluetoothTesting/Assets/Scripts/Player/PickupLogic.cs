using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupLogic : MonoBehaviour
{
    #region Picked Up Item Offsets

    [SerializeField] private float xOffset = 5;
    [SerializeField] private float yOffset = 2;
    [SerializeField] private float zOffset = 4;
    #endregion
    
    [HideInInspector]public ItemLogic currentItem;
    private bool hasItem;

    public void PickupItem(ItemLogic pickedUpItem)
    {
        if (!hasItem)
        {
            //Debug.Log("Item picked up");
            currentItem = pickedUpItem;
            hasItem = true;
            Transform currentItemTransform = currentItem.gameObject.transform;
            currentItemTransform.SetParent(gameObject.transform);
            currentItemTransform.localPosition = new Vector3();
            currentItemTransform.localPosition += new Vector3(xOffset, yOffset, zOffset); 
            //currentItemTransform.localScale = transform.localScale / scaleDivider;
        }
        
    }

    public void UseItem(DoorLogic targetDoor)
    {
        if (currentItem.tiedDoor = targetDoor.gameObject)
        {
            //Debug.Log("Item used");
            Destroy(currentItem.gameObject);
            hasItem = false;
            Destroy(targetDoor.gameObject);
        }
        
    }
}
