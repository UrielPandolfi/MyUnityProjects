using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public static BossController instance;
    public enum bossStates {hurt, moving, shooting, defeated};
    private bossStates currentState;
    public float moveSpeed;
    private bool movingRight,bombACheck, bombBCheck;
    public Transform rightPoint, leftPoint, firePoint, minePoint;
    public Animator anim;
    public float hurtTime, timeBetweenShots, numberOfMines, speedUpShoots,speedUpMove;
    private float hurtCounter, shotCounter, bombA, bombB, health = 6;
    public Rigidbody2D theRB;
    public GameObject Bullet, hitBox, mine, explosion, platformWin;

    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        currentState = bossStates.shooting;
        leftPoint.parent = null;
        rightPoint.parent = null;
        movingRight = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case bossStates.shooting:
                hitBox.gameObject.SetActive(true);
                shotCounter -= Time.deltaTime;
                if(shotCounter <= 0)
                {
                    shotCounter = timeBetweenShots;
                    var newBullet = Instantiate(Bullet, firePoint.position, firePoint.rotation);
                    newBullet.transform.localScale = transform.localScale;
                }
                break;

            case bossStates.hurt:
                if(hurtCounter > 0){
                    hurtCounter -= Time.deltaTime;
                    if(hurtCounter <= 0)
                    {
                        currentState = bossStates.moving;
                        BombsPosition();
                        bombACheck = false;
                        bombBCheck = false;
                        if(health == 0)
                        {
                            currentState = bossStates.defeated;
                        }
                    }
                }
                break;

            case bossStates.moving:
                
                if(movingRight){
                    theRB.velocity = new Vector2(moveSpeed, 0f);
                    if(numberOfMines == 1)
                    {
                        if(transform.position.x > bombA && bombACheck == false)
                        {
                            Instantiate(mine, minePoint.position, minePoint.rotation);
                            bombACheck = true;
                        }
                    }
                    else
                    {
                        if(transform.position.x > bombA && bombACheck == false)
                        {
                            Instantiate(mine, minePoint.position, minePoint.rotation);
                            bombACheck = true;
                        }
                        if(transform.position.x > bombB && bombBCheck == false)
                        {
                            Instantiate(mine, minePoint.position, minePoint.rotation);
                            bombBCheck = true;
                        }
                    }

                    if(transform.position.x > rightPoint.position.x)
                    {
                        theRB.velocity = new Vector2(0f,0f);
                        transform.localScale = new Vector3(1f,1f,1f);
                        movingRight = false;
                        currentState = bossStates.shooting;
                        shotCounter = timeBetweenShots;
                        anim.SetTrigger("StopMoving");
                    }
                }
                else{
                    theRB.velocity = new Vector2(-moveSpeed, 0f);
                    if(numberOfMines == 1)
                    {
                        if(transform.position.x < bombA && bombACheck == false)
                        {
                            Instantiate(mine, minePoint.position, minePoint.rotation);
                            bombACheck = true;
                        }
                    }
                    else
                    {
                        if(transform.position.x < bombA && bombACheck == false)
                        {
                            Instantiate(mine, minePoint.position, minePoint.rotation);
                            bombACheck = true;
                        }
                        if(transform.position.x < bombB && bombBCheck == false)
                        {
                            Instantiate(mine, minePoint.position, minePoint.rotation);
                            bombBCheck = true;
                        }
                    }
                    
                    if(transform.position.x < leftPoint.position.x)
                    {
                        
                        theRB.velocity = new Vector2(0f,0f);
                        transform.localScale = new Vector3(-1f,1f,1f);
                        movingRight = true;
                        currentState = bossStates.shooting;
                        shotCounter = timeBetweenShots;
                        anim.SetTrigger("StopMoving");
                    }
                }

                

                break;
                case bossStates.defeated:
                    gameObject.SetActive(false);
                    Instantiate(explosion,transform.position,transform.rotation);
                    AudioManager.instance.StopBossMusic();
                    platformWin.SetActive(true);
                break;
        }
    }

    public void TakeHit()
    {
        currentState = bossStates.hurt;
        hurtCounter = hurtTime;
        anim.SetTrigger("Hit");
        health -= 1;
        moveSpeed /= speedUpMove;
        timeBetweenShots -= speedUpShoots;
    }

    public void BombsPosition()
    {
        numberOfMines = Random.Range(0,3);
        if(numberOfMines == 1)
        {
            bombA = Random.Range(leftPoint.position.x, rightPoint.position.x);
        }
        else if(numberOfMines == 2)
        {
            bombA = Random.Range(leftPoint.position.x, rightPoint.position.x);
            bombB = Random.Range(leftPoint.position.x, rightPoint.position.x);
        }
    }
}
