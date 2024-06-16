using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthController : MonoBehaviour
{
    public static EnemyHealthController instance;
    public Rigidbody theRB;
    public float maxHealth, knockbackForce;
    public int deahtSound;
    private float currentHealth;
    public GameObject deathEffect;

    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage()
    {

        //Restamos un punto de vida, actualizamos el UI, hacemos que aparezca el popup del daño y agregamos knockback.
        currentHealth --;
        if(currentHealth <= 0)
        {
            AudioManager.instance.playSFX(deahtSound);
            Instantiate(deathEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
