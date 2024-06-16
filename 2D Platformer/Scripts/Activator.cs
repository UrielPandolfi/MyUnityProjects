using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activator : MonoBehaviour
{
    public GameObject boss;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            boss.SetActive(true);
            gameObject.SetActive(false);
            AudioManager.instance.PlayBossMusic();
        }
    }
}
