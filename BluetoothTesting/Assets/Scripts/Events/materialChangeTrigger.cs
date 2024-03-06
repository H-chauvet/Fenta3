using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class materialChangeTrigger : MonoBehaviour
{
    public Material scuffedMaterial;
    public string assetName = "Plane";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            changeMat(scuffedMaterial, assetName);
        }
    }

    private void changeMat(Material newMaterial, string assetName)
    {
        GameObject targetObject = GameObject.Find(assetName);

        if (targetObject != null)
        {
            Renderer targetRenderer = targetObject.GetComponent<Renderer>();

            if (targetRenderer != null)
            {
                targetRenderer.material = newMaterial;
            }
            else
            {
                Debug.LogWarning("Renderer component not found on the target object.");
            }
        }
        else
        {
            Debug.LogWarning("Target object not found in the hierarchy.");
        }
    }
}