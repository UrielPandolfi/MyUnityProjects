using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitbox : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("HurtBox"))
        {
            BossController.instance.TakeHit();
            PlayerController.instance.bounce();
            gameObject.SetActive(false);
        }
        else
        {
            PlayerHealthController.instance.dealDamage();
        }
    }
}
