using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : StatsController
{
    
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        UIManager.instance.HurtScreen();
    }
    public override void Die()
    {
        base.Die();
        // Animación de muerte y vuelta al menu principal
        GameManager.instance.LevelLost();
    }
}
