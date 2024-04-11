using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public static CameraMove instance;
    private float  movY, movX;
    public Transform playerHead;    
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = playerHead.position;
        movX += Input.GetAxis("Mouse X");
        movY += Input.GetAxis("Mouse Y");
        movY = Mathf.Clamp(movY, -90f, 90f);
        transform.rotation = Quaternion.Euler(-movY, movX, 0f);
    }
}
