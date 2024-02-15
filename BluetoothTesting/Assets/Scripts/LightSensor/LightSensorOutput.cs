using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LightSensorOutput : MonoBehaviour
{
    public TMP_Text tm;
    public LightSensorPluginScript test;
    

    // Update is called once per frame
    void Update()
    {
        tm.text = test.getLux().ToString();
    }
}
