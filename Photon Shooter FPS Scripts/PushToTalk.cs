using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Voice.Unity;

public class PushToTalk : MonoBehaviour
{
    private Recorder recorder;
    void Start()
    {
        recorder = GetComponent<Recorder>();
    }

    // Update is called once per frame
    void Update()
    {
        DoPushToTalk();
    }

    private void DoPushToTalk()
    {
        if(Input.GetKey(KeyCode.K))
        {
            recorder.TransmitEnabled = true;
        }
        else
        {
            recorder.TransmitEnabled = false;
        }
    }
}
