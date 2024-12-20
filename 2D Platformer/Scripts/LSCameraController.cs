using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LSCameraController : MonoBehaviour
{
    public Transform target;
    public Vector2 maxPos, minPos;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float xPos = Mathf.Clamp(target.position.x, minPos.x, maxPos.x);
        float yPos = Mathf.Clamp(target.position.y, minPos.y, maxPos.y);
        transform.position = new Vector3(xPos,yPos,transform.position.z);
    }
}
