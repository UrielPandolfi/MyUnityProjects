using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashlightController : MonoBehaviour
{
    private Light flashlight;
    bool ligthOn;
    void Start()
    {
        flashlight = GetComponent<Light>();
        ligthOn = true;
    }

    void Update()
    {
        transform.rotation = Camera.main.transform.rotation;

        if(Input.GetKeyDown(KeyCode.F))
        {
            if(ligthOn)
            {
                flashlight.enabled = false;
                ligthOn = false;
            }
            else
            {
                flashlight.enabled = true;
                ligthOn = true;
            }
            
        }
    }
}
