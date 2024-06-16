using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Enemy"))
        {
            EnemyController.instance.death();
            PlayerController.instance.bounce();
        }
        else if(other.CompareTag("FlyEnemy"))
        {
            FlyingEnemyController.instance.death();
            PlayerController.instance.bounce();
        }
    }
}
