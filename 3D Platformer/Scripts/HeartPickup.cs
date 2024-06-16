using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartPickup : MonoBehaviour
{
    public GameObject heartEffect;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && PlayerHealthController.instance.currentHealth != PlayerHealthController.instance.maxHealth)
        {
            Instantiate(heartEffect, transform.position, transform.rotation); 
            PlayerHealthController.instance.Heal();
            AudioManager.instance.playSFX(7);
            Destroy(gameObject);
        }
    }
}
