using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtEnemy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Enemy"))
        {
            EnemyHealthController enemyHealthScript = other.GetComponent<EnemyHealthController>();
            enemyHealthScript.TakeDamage();
        }
    }
}
