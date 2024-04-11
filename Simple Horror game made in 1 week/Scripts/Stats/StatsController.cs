using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsController : MonoBehaviour
{
    public float maxHealth;
    public float damage;
    public float resist = 2f;
    public float currentHealth { get; private set;}

    void Awake()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(float damage)
    {
        damage *= 1f / resist;      // Aplicamos la resistencia del personaje
        currentHealth -= damage;    // Inflingimos el daño
        currentHealth = Mathf.Clamp(currentHealth, 0f, int.MaxValue);
        Debug.Log(transform.name + " recibió " + damage + " de daño y le queda " + currentHealth + " puntos de vida.");
        if(currentHealth <= 0f)     // Si llega a 0 ejecutamos la funcion Die
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Debug.Log(transform.name + " a Muerto");
        // Función hecha para ser sobreescrita según el personaje
    }
}
