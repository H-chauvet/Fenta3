using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.VersionControl;
using UnityEngine;

public class FlashlightLogic : MonoBehaviour
{
    [SerializeField] private float lightIntensityMod = 5f;
    [SerializeField] private float lightRangedMod = 10f;
    private float lightIntensity = 5f;
    private float lightRange = 10f;
    private float floatValue = 0f;
    private bool hasReachedMax;
    private List<SphereCollider> colliders;
    private ConeRaycaster CR;
    
    void Start()
    {
        SetLightProperties();
        CR = GetComponent<ConeRaycaster>();
        colliders = new List<SphereCollider>();

        TCPChannelHandler.Instance.lightEvent += OnLightEvent;
    }

    private void OnLightEvent(float luxValue)
    {
        isIntensityChanged(luxValue);
    }

    void OnDisable()
    {
        TCPChannelHandler.Instance.lightEvent -= OnLightEvent;
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
            floatValue += 1f;
            if (floatValue > 5f)
            {
                floatValue = 5f;
                hasReachedMax = true;
                Debug.Log("Light limit reached");
                
            }
            
            
        }

        if (Input.GetMouseButtonDown(1))
        {
            hasReachedMax = false;
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

    public void isIntensityChanged(float value)
    {
            floatValue = value;
    }

    void AttachSpheres()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (floatValue <= 5)
            {
                if(!hasReachedMax)AddSphere();
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
