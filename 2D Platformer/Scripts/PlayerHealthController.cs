using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController instance;
    public int currentHealth, maxHealth;
    public float invincibleLenght;
    private float invincibleCounter;
    private SpriteRenderer theSR;
    public GameObject deathEffect;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        currentHealth = maxHealth;
        theSR = GetComponent<SpriteRenderer>();
    }
    

    void Update()
    {
        if(invincibleCounter>0)
        {
            invincibleCounter -= Time.deltaTime;

            if(invincibleCounter<=0)
            {
                theSR.color = new Color(theSR.color.r,theSR.color.g,theSR.color.b,1f);
            }
        }
    }

    public void dealDamage()
    {
        if(invincibleCounter<=0)
        {
            PlayerController.instance.anim.SetTrigger("Hurt");
            currentHealth--;
            AudioManager.instance.PlaySFX(9);

            if(currentHealth <= 0)
            {
                currentHealth = 0;
                Instantiate(deathEffect,PlayerController.instance.transform.position,PlayerController.instance.transform.rotation);
                LevelManager.instance.RespawnPlayer();
                AudioManager.instance.PlaySFX(8);
            }
            
            PlayerController.instance.knockBack();

            ControllerUI.instance.updateHealthDisplay();

            invincibleCounter=invincibleLenght;
            theSR.color = new Color(theSR.color.r,theSR.color.g,theSR.color.b,.5f);
        }
    }

    public void HealPlayer(){
        currentHealth++;
        ControllerUI.instance.updateHealthDisplay();
    }
    
}
