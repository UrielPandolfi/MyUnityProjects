using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public Transform[] patrolPoints;
    public int currentPoint;

    public NavMeshAgent agent;
    public Animator anim;

    public float waitTime, cdTime;
    private float waitCounter, cdCounter;
    public float chaseDistance, attackDistance;

    public enum aiState
    {
        Idle,
        Patrolling,
        Chasing,
        Attacking
    };
    public aiState currentState;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < patrolPoints.Length; i++)
        {
            patrolPoints[i].parent = null;
        }
        waitCounter = waitTime;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, PlayerController.instance.transform.position);
        switch(currentState)
        {
            //Quieto.
            case aiState.Idle:
                //Pasamos a la animacion de Idle.
                anim.SetBool("IsMoving", false);

                //corremos el contador de tiempo de espera y cuando llega a 0 empezamos a movernos de nuevo.
                if(waitCounter > 0)
                {
                    waitCounter -= Time.deltaTime;
                }
                else
                {
                    agent.isStopped = false;
                    agent.SetDestination(patrolPoints[currentPoint].position);
                    currentState = aiState.Patrolling;
                }

                //si el jugador esta lo suficientemente cerca, empezamos a seguirlo.
                if(distanceToPlayer <= chaseDistance)
                {
                    agent.isStopped = false;
                    currentState = aiState.Chasing;
                }

                break;

            //Patrullando.
            case aiState.Patrolling:
                anim.SetBool("IsMoving", true);
                
                //Si llega al punto definido, sumamos uno al currentPoint y pasamos de nuevo a idle.
                if(agent.remainingDistance <= 0.2f)
                {
                    currentPoint++;
                    if(currentPoint >= patrolPoints.Length)
                    {
                        currentPoint = 0;
                    }

                    waitCounter = waitTime;
                    currentState = aiState.Idle;
                }

                //si el jugador esta lo suficientemente cerca, empezamos a seguirlo.
                if(distanceToPlayer <= chaseDistance)
                {
                    currentState = aiState.Chasing;
                }

                break;
            
            //Persiguiendo.
            case aiState.Chasing:
                anim.SetBool("IsMoving", true);
                //Nos movemos hacia el jugador.
                agent.SetDestination(PlayerController.instance.transform.position);

                if(distanceToPlayer <= attackDistance)
                {
                    //Como de costumbre, antes de cambiar a la siguiente etapa configuramos todo.
                    anim.SetBool("IsMoving", false);
                    agent.velocity = Vector3.zero;
                    agent.isStopped = true;
                    currentState = aiState.Attacking;
                } 
                else if(distanceToPlayer > (chaseDistance * 1.5)) //Si se  aleja demasiado el jugador vuelve a patrullar.
                {
                    agent.isStopped = true;
                    waitCounter = waitTime;
                    currentState = aiState.Idle;
                }

                break;

            case aiState.Attacking:
                cdCounter -= Time.deltaTime;
                //hacemos que mire al jugador.
                Vector3 lookPosition = new Vector3(PlayerController.instance.transform.position.x, transform.position.y, PlayerController.instance.transform.position.z);
                transform.LookAt(lookPosition);

                //si no tiene cooldown elegimos una accion dependiendo de la distancia del jugador, si atacamos reseteamos el cooldown.
                if(cdCounter <= 0)
                {
                    if(distanceToPlayer <= attackDistance)
                    {
                        anim.SetTrigger("Attack");
                        cdCounter = cdTime;
                    }
                    else
                    {
                        //devolvemos el valor de isStoped a falso asi se puede mover de nuevo y hacemos que se quede quieto otra vez.
                        agent.isStopped = false;
                        currentState = aiState.Chasing;
                    }
                }
                break;
        }
        
    }
}
