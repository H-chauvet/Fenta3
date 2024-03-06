using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class officeLight : MonoBehaviour
{
    public float delay = 4f;

    private float timeElapsed;
    public Light ofcLight;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("TurnOffLight", delay);
    }

    // Update is called once per frame
    void TurnOffLight()
    {
        if (ofcLight != null)
        {
            ofcLight.enabled = false;
        }
    }
}
