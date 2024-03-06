using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flickering : MonoBehaviour
{
    //IntensityRange
    public float minIntensity = 0.05f;
    public float maxIntensity = 1.2f;
    //Flickering
    public float Delay = 0.07f;

    private Light lightComponent;
    private float timeElapsed;

    void Start()
    {
        lightComponent = GetComponent<Light>();
        if (lightComponent == null)
        {
            Debug.LogError("FlickeringLight script requires a Light component on the GameObject.");
            enabled = false;
        }
        lightComponent.intensity = Random.Range(minIntensity, maxIntensity);
        timeElapsed = 0f;
    }

    void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed >= Delay)
        {
            lightComponent.intensity = Random.Range(minIntensity, maxIntensity);
            timeElapsed = 0f;
        }
    }
}

