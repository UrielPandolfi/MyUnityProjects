using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : StatsController
{
    private EnemyChasing enemyChasing;
    private EnemyController enemyController;
    public event System.Action OnDeath;

    void Start()
    {
        enemyChasing = GetComponent<EnemyChasing>();
        enemyController = GetComponent<EnemyController>();
    }
    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);

        if(enemyChasing.firstChase)
        {
            enemyController.SetState(state.chasing);
        }
    }
    public override void Die()
    {
        base.Die();
        GameManager.instance.AddKill();
        OnDeath?.Invoke();
    }
}
