using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrolling : MonoBehaviour
{
    private NavMeshAgent agent;
    private int currentPoint;
    private float waitCooldown;
    EnemyController enemyController;
    [SerializeField] private float viewRadius;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public Transform head; // Configuramos la vision del enemigo desde su cabeza, osea sus ojos
    public Transform[] wayPoints;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(wayPoints[0].position);
        enemyController = GetComponent<EnemyController>();
    }

    void Update()
    {
        waitCooldown -= Time.deltaTime;

        EnviromentView();

        if(waitCooldown <= 0f)
        {
            Patrolling();
        }
        else
        {
            agent.isStopped = true;
        }
    }

    void Patrolling()
    {
        agent.isStopped = false;
        if(!agent.pathPending && agent.remainingDistance < 2f)
        {
            currentPoint++;
            if(currentPoint >= wayPoints.Length)
            {
                currentPoint = 0;
            }
            agent.SetDestination(wayPoints[currentPoint].position);
            waitCooldown = 1.5f;
        }
    }

    void EnviromentView()
    {
        Collider[] Players = Physics.OverlapSphere(transform.position, viewRadius, playerMask);

        for(int i=0; i<Players.Length; i++)
        {
            Transform player = Players[i].transform; // Posición del jugador actual
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float dot = Vector3.Dot(transform.forward, dirToPlayer);    //Si esta a 45 grados da 0.5, a 90 grados da 0, si esta en frente 1
            if(dot >= 0.5) // Si esta dentro de los 45 grados
            {
                if(!Physics.Raycast(head.position, dirToPlayer, viewRadius, obstacleMask)) // Si no hay ningun obstáculo en medio
                {
                    // Perseguir al jugador o algo asi
                    enemyController.SetState(state.chasing);
                    Debug.Log("Jugador a la vista!");
                }
            }
        }
    }
    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
    }
}
