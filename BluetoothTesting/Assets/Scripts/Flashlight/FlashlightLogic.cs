using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FlashlightLogic : MonoBehaviour
{
    [SerializeField] private float lightIntensityMod = 5f;
    [SerializeField] private float lightRangedMod = 10f;
    private float lightIntensity = 5f;
    private float lightRange = 10f;
    private float floatValue = 3f;

    void Start()
    {
        SetLightProperties();
    }

    void Update()
    {
        ChangeFlashlight();
        // Example: Modify intensity and range based on a float variable
        lightIntensity = floatValue * lightIntensityMod; 
        lightRange = floatValue * lightRangedMod; 

        // Update light properties
        SetLightProperties();
    }

    void SetLightProperties()
    {
        // Set intensity and range
        GetComponent<Light>().intensity = lightIntensity;
        GetComponent<Light>().range = lightRange;
    }

    void ChangeFlashlight()
    {
        if (Input.GetMouseButtonDown(0))
        {
            floatValue += 3f;
        }

        if (Input.GetMouseButtonDown(1))
        {
            floatValue -= 3f;
            if (floatValue <= 0f)
            {
                floatValue = 0f;
            }
        }
    }
}
