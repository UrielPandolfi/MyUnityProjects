using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimator : MonoBehaviour
{
    Animator animator;
    NavMeshAgent agent;
    EnemyAttack enemyAttack;
    EnemyChasing enemyChasing;
    EnemyStats myStats;
    void Start()
    {
        #region References
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        enemyAttack = GetComponent<EnemyAttack>();
        enemyChasing = GetComponent<EnemyChasing>();
        myStats = GetComponent<EnemyStats>();
        #endregion
    
        enemyAttack.OnAttack += AttackAnimation;
        enemyChasing.OnStartChase += StartChaseAnimation;
        myStats.OnDeath += DeathAnimation;
    }

    void Update()
    {
        float speedPercent = agent.velocity.magnitude / agent.speed;
        animator.SetFloat("speedPercent", speedPercent);
    }
    
    void AttackAnimation()
    {
        animator.SetTrigger("attack");
    }

    void StartChaseAnimation()
    {
        animator.SetTrigger("startChase");
        enemyChasing.OnStartChase -= StartChaseAnimation; // Siempre que no necesitamos mas una suscripciòn, la sacamos

    }

    void DeathAnimation()
    {
        animator.SetBool("isDeath", true);
        myStats.OnDeath -= DeathAnimation;
    }

    void OnDestroy() // Nos desuscribimos de todos los eventos al ser destruido para evitar memory leaks
    {
        enemyAttack.OnAttack -= AttackAnimation;
        enemyChasing.OnStartChase -= StartChaseAnimation;
        myStats.OnDeath -= DeathAnimation;
    }
}
