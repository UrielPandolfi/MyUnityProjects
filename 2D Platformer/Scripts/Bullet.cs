using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(-speed * Time.deltaTime * transform.localScale.x, 0f, 0f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerHealthController.instance.dealDamage();
        }
        else if(other.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}
