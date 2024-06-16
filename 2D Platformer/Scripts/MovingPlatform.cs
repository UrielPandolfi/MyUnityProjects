using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform[] points;
    public int currentPoint;
    public Transform platform;
    public float moveSpeed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        platform.position = Vector3.MoveTowards(platform.position,points[currentPoint].position, moveSpeed * Time.deltaTime);
        if(platform.position == points[currentPoint].position)
        {
            currentPoint++;
            if(currentPoint >= points.Length)
            {
                currentPoint = 0;
            }
        }
    }
}
