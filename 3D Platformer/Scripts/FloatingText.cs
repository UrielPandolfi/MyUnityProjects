using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
    private float destroyTime = 2f;
    private Vector3 offset = new Vector3(0f, 1f, 0f);
    void Start()
    {
        Destroy(gameObject, destroyTime);

        transform.localPosition = offset;
    }
}
