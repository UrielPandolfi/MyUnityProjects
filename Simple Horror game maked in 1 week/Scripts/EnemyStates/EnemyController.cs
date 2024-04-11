using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    #region Singleton
    public static EnemyController instance;
    void Awake()
    {
        instance = this;
    }
    #endregion
    
    private state currentState;
    public MonoBehaviour[] statesScripts;
    private EnemyStats myStats;
    private BoxCollider enemyCollider;
    
    void Start()
    {
        SetState(state.patrolling);
        myStats = GetComponent<EnemyStats>();
        myStats.OnDeath += DeathEnemy;
        enemyCollider = GetComponent<BoxCollider>();
    }

    void DeathEnemy()
    {
        enemyCollider.enabled = false;
        SetState(state.death);
    }

    public void SetState(state newState)
    {
        statesScripts[(int)currentState].enabled = false;
        currentState = newState;
        statesScripts[(int)currentState].enabled = true;
    }
}

public enum state{patrolling, chasing, attacking, death}