using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemHighlight : MonoBehaviour
{
    // The material to use for highlighting the object
    [SerializeField] private Material highlightMaterial;

    // Flag to determine whether the object is currently highlighted
    private bool isHighlighted = false;

    // Keep a reference to the original material of the object
    private List<Material> originalMaterial;

    // The material to use for highlighting the object (transparent)
    private Material highlightedMaterial;

    private MeshRenderer[] renderer;

    // Start is called before the first frame update
    void Start()
    {
        highlightedMaterial = new Material(highlightMaterial);
        originalMaterial = new List<Material>();

        if (GetComponentInChildren<MeshRenderer>() != null)
        {
            renderer = GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer rend in renderer)
            {
                originalMaterial.Add(rend.material);
            }
        }
    }

    public void ApplyHighlight()
    {
        if (renderer != null)
        {
            foreach (MeshRenderer rend in renderer)
            {
                if (rend.materials.Length == 1)
                {
                    Material existingMaterial = rend.material;

                    // Create a new instance of the existing material
                    Material newMaterial = new Material(existingMaterial);

                    // Get the current materials array
                    Material[] currentMaterials = rend.materials;

                    // Create a new array with the desired size (one more than the current materials array)
                    Material[] newMaterials = new Material[currentMaterials.Length + 1];

                    // Copy the current materials to the new array
                    currentMaterials.CopyTo(newMaterials, 0);

                    // Add the new material to the last index of the new array
                    newMaterials[newMaterials.Length - 1] = newMaterial;

                    // Assign the new materials array to the Renderer
                    rend.materials = newMaterials;
                }
            }
        }
        
        if (highlightMaterial != null)
        {
            // Create a new material instance for the highlighted material and set it to the object
            Material newMaterial = new Material(highlightedMaterial);
            foreach (Material matt in originalMaterial)
            {
                newMaterial.mainTexture = matt.mainTexture;
                
                if (renderer != null)
                {
                    foreach (MeshRenderer rend in renderer)
                    {
                        rend.material = newMaterial;
                    }
                }
            }
            
            isHighlighted = true;
        }
    }

    public void RemoveHighlight()
    {
        if (isHighlighted)
        {
            if (renderer != null)
            {
                foreach (MeshRenderer rend in renderer)
                {
                    rend.material = originalMaterial[0];
                }
            }
            
            isHighlighted = false;
        }
    }
}
