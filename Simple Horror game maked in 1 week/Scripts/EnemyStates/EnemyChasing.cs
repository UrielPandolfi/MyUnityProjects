using UnityEngine;
using UnityEngine.AI;

public class EnemyChasing : MonoBehaviour
{
    private NavMeshAgent agent;
    EnemyController enemyController;
    public bool firstChase = true;
    public event System.Action OnStartChase;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyController = GetComponent<EnemyController>();
    }
    void Update()
    {
        if(firstChase)
        {
            OnStartChase?.Invoke();     // Reproducimos la animacion de grito
            AudioManager.instance.PlaySFX(0);
            agent.isStopped = true;
            Invoke("SetFirstChase", 1.12f);
        }
        
        agent.SetDestination(PlayerContrller.instance.transform.position);
        
        float disToPlayer = Vector3.Distance(transform.position, PlayerManager.instance.player.transform.position);
        if(disToPlayer <= agent.stoppingDistance)
        {
            enemyController.SetState(state.attacking);
        }
        
    }

    void SetFirstChase()
    {
        firstChase = false;
        agent.isStopped = false;
    }
}
