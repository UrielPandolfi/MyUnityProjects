using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public GameObject nicknameParent;

    // Update is called once per frame
    void Update()
    {
        nicknameParent.transform.LookAt(Camera.main.transform.position);
    }
}
