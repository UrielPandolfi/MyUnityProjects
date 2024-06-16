using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public Transform farBG;
    public Transform middleBG;
    private Vector2 lastPosition;
    public Transform target;
    public float maxHeight, minHeight;

    void Start()
    {
        lastPosition = transform.position;
    }

    void Update()
    {
        transform.position = new Vector3(target.position.x, Mathf.Clamp(target.position.y, minHeight, maxHeight), transform.position.z);

        Vector2 amountToMove = new Vector2(transform.position.x - lastPosition.x, transform.position.y - lastPosition.y);

        farBG.position += new Vector3(amountToMove.x, amountToMove.y, 0f);
        middleBG.position += new Vector3(amountToMove.x, amountToMove.y, 0f) * .5f;

        lastPosition = transform.position;
    }
}
