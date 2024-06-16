using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyController : MonoBehaviour
{
    public static FlyingEnemyController instance;
    public Transform[] points;
    private int currentPoint;
    public float moveSpeed;
    public bool playerNear;
    public float attackDistance, chaseSpeed;
    public SpriteRenderer theSR;
    private Vector3 targetAttack;
    public float waitAfterAttack;
    private float attackCounter;
    public GameObject deathEffect;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        for(int i=0; i < points.Length; i++)
        {
            points[i].parent = null;
        }
    }

    void Update()
    {
        if(attackCounter > 0.1f)
        {
            attackCounter -= Time.deltaTime;
        }
        else
        {
            if(Vector2.Distance(transform.position, PlayerController.instance.transform.position) > attackDistance)
            {
                targetAttack = Vector3.zero;
                transform.position = Vector3.MoveTowards(transform.position,points[currentPoint].position, moveSpeed * Time.deltaTime);
                if(transform.position == points[currentPoint].position)
                {
                    currentPoint++;
                    if(currentPoint >= points.Length)
                    {
                        currentPoint = 0;
                    }
                }
                if(transform.position.x < points[currentPoint].position.x)
                {
                    theSR.flipX = true;
                } 
                else
                {
                    theSR.flipX = false;
                }
            }
            else{
                if(targetAttack == Vector3.zero)
                {
                    targetAttack = PlayerController.instance.transform.position;
                }

                transform.position = Vector3.MoveTowards(transform.position, targetAttack, chaseSpeed * Time.deltaTime);

                if(Vector3.Distance(transform.position, targetAttack) < .1f)
                {
                    attackCounter = waitAfterAttack;
                    targetAttack = Vector3.zero;
                }
            }
        }
    }
    public void death()
    {
        gameObject.SetActive(false);
        Instantiate(deathEffect,transform.position,transform.rotation);
        AudioManager.instance.PlaySFX(3);
    }
}
