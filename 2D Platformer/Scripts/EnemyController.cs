using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public static EnemyController instance;
    public float moveSpeed;
    public Transform rightPoint, leftPoint;
    private bool movingRight;
    public Rigidbody2D theRB;
    public SpriteRenderer theSR;
    public float moveTime, waitTime;
    private float moveCount, waitCount;
    public GameObject collectible;
    public GameObject deathEffect;
    public float chanceToDrop;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        leftPoint.parent = null;
        rightPoint.parent = null;

        movingRight = true;
        moveCount = moveTime;
    }

    
    void Update()
    {   
        if(moveCount > 0)
        {
            moveCount -= Time.deltaTime;

            if(movingRight)
            {
                theRB.velocity = new Vector2(moveSpeed, theRB.velocity.y);
                theSR.flipX = false;
                if(transform.position.x > rightPoint.position.x)
                {
                    movingRight = false;
                }
            }
            else
            {
                theRB.velocity = new Vector2(-moveSpeed, theRB.velocity.y);
                theSR.flipX = true;
                if(transform.position.x < leftPoint.position.x)
                {
                    movingRight = true;
                }
            }
            
            if(moveCount <= 0)
            {
                waitCount = waitTime;
            }
        }
        else if(waitCount > 0)
        {
            waitCount -= Time.deltaTime;
            theRB.velocity = new Vector2(0f,theRB.velocity.y);
            if(waitCount <= 0)
            {
                moveCount = moveTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            PlayerHealthController.instance.dealDamage();
        }
    }
    public void death()
    {
        gameObject.SetActive(false);
        Instantiate(deathEffect,transform.position,transform.rotation);
        float dropSelect = Random.Range(0f,100f);
        if(dropSelect <= chanceToDrop)
        {
            Instantiate(collectible,transform.position,transform.rotation);
        }
        AudioManager.instance.PlaySFX(3);
    }
}
