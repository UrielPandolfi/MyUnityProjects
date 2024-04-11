using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackSpeed = 1f;
    public float attackCooldown;
    private float attackDelay = 0.75f;      // Pequeño delay hecho para que el ataque coordine con la animación
    StatsController playerStats;
    StatsController myStats;
    public event System.Action OnAttack;
    EnemyController enemyController;
    
    void Start()
    {
        playerStats = PlayerManager.instance.player.GetComponent<StatsController>();
        myStats = GetComponent<StatsController>();
        enemyController = GetComponent<EnemyController>();
    }

    void Update()
    {
        attackCooldown -= Time.deltaTime;

        if(attackCooldown <= 0f)
        {
            Attack();
        }

        float disToPlayer = Vector3.Distance(transform.position, PlayerManager.instance.player.transform.position);
        if(disToPlayer > 2.3f)
        {
            enemyController.SetState(state.chasing);
        }

        FaceTarget();
    }
    
    void Attack()
    {
        if(attackCooldown <= 0f)
        {
            OnAttack?.Invoke();
            AudioManager.instance.PlaySFX(1);
            StartCoroutine(DoDamage());
            attackCooldown = 1f / attackSpeed;
        }
    }

    void FaceTarget()   // Función para siempre mirar al jugador mientras ataca
    {
        Vector3 direction = (playerStats.transform.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5f * Time.deltaTime);
    }

    IEnumerator DoDamage()
    {
        yield return new WaitForSeconds(attackDelay);   // Aplicamos el delay
        playerStats.TakeDamage(myStats.damage);         // Inflingimos el daño
    }
}
