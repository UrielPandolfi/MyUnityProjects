using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Killzone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerHealthController.instance.currentHealth=1;
            PlayerHealthController.instance.dealDamage();
            LevelManager.instance.RespawnPlayer();
        }
    }
}
