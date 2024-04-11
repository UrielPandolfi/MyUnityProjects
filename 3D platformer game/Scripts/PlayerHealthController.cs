using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    public GameObject floatingTextPrefab, deathExplosion;
    public static PlayerHealthController instance;
    public Animator anim;
    public float maxHealth, currentHealth;
    public float invincibilityTime;
    private float invincibilityCounter;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        //Igualamos la vida a la vida maxima.
        ResetHealth();
    }

    void Update()
    {
        if(invincibilityCounter > 0)
        {
            invincibilityCounter -= Time.deltaTime;
        }
    }

    public void TakeDamage()
    {
        //Comprobamos que el contador este en 0 para evitar que el jugador sea dañado multiples veces.
        if(invincibilityCounter <= 0)
        {
            //Restamos un punto de vida, actualizamos el UI, hacemos que aparezca el popup del daño y agregamos knockback.
            currentHealth --;
            UIManager.instance.UpdateHP();
            AudioManager.instance.playSFX(8);
            ShowFloatingText();
            PlayerController.instance.Knockback();
            if(currentHealth <= 0) //Si muere el personaje corremos la funcion Death.
            {
                Death();
            }
            else //Caso contrario le agregamos tiempo de invencibilidad.
            {
                invincibilityCounter = invincibilityTime;
            }
        }
    }

    public void Heal()
    {
        currentHealth ++;
        UIManager.instance.UpdateHP();
    }
    
    void ShowFloatingText()
    {
        var ft = Instantiate(floatingTextPrefab, transform.position, transform.rotation, transform);
        ft.GetComponent<TextMesh>().text = "-1HP";
    }

    public void Death()
    {
        Instantiate(deathExplosion, transform.position, transform.rotation);
        GameManager.instance.Respawn();
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        UIManager.instance.UpdateHP();
    }
}
