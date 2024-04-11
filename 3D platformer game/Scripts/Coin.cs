using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public GameObject coinEffect;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            GameManager.instance.addCoins();
            Instantiate(coinEffect, transform.position, transform.rotation);
            AudioManager.instance.playSFX(5);
            Destroy(gameObject);
        }
    }
}
