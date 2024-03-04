using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class FlashlightLogic : MonoBehaviour
{
    [SerializeField] private float lightIntensityMod = 5f;
    [SerializeField] private float lightRangedMod = 10f;
    private float lightIntensity = 5f;
    private float lightRange = 10f;
    private float floatValue = 0f;
    private List<SphereCollider> colliders;
    private ConeRaycaster CR;
    
    void Start()
    {
        SetLightProperties();
        CR = GetComponent<ConeRaycaster>();
        colliders = new List<SphereCollider>();
    }

    void Update()
    {
        ChangeFlashlight();
        // Example: Modify intensity and range based on a float variable
        lightIntensity = floatValue * lightIntensityMod; 
        lightRange = floatValue * lightRangedMod; 

        // Update light properties
        SetLightProperties();
        
        AttachSpheres();
    }

    void SetLightProperties()
    {
        // Set intensity and range
        GetComponent<Light>().intensity = lightIntensity;
        GetComponent<Light>().range = lightRange;
    }

    void AddSphere()
    {
        colliders.Add(CR.CreateSphere());
    }

    void RemoveSphere()
    {
        if (colliders.Count == 0) return;
        SphereCollider sphereToRemove = colliders.Last();
        colliders.Remove(sphereToRemove);
        Destroy(sphereToRemove);
    }
    
    void ChangeFlashlight()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (floatValue >= 5f)
            {
                floatValue = 5f;
                Debug.Log("Light limit reached");
            }
            else
            {
                floatValue += 1f;
               
            }
            
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (floatValue <= 0f)
            {
                floatValue = 0f;
            }
            else
            {
                floatValue -= 1f;
                
            }
        }
    }

    void AttachSpheres()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (floatValue < 5)
            {
                AddSphere();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (floatValue >= 0)
            {
                RemoveSphere();
            }
        }
    }
}
