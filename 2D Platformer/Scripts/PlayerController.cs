using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public float moveSpeed;
    public float jumpforce;
    public float bounceForce;
    public Rigidbody2D theRB;
    public Transform groundCheckpoint;
    public LayerMask whatIsGround;
    private bool isGrounded;
    private bool canDoubleJump;
    public Animator anim;
    public SpriteRenderer theSR;
    public float knockBackLenght, knockBackForce;
    private float knockBackCounter;
    public bool stopInput;
    public GameObject platform;

    public void Awake(){
        instance=this;
    }
    void Start()
    {
        
    }

    void Update()
    {
        if(!PauseMenu.instance.isPaused && !stopInput)
        {
            if(knockBackCounter<=0){
                isGrounded = Physics2D.OverlapCircle(groundCheckpoint.position, .2f, whatIsGround);

                theRB.velocity = new Vector2(moveSpeed * Input.GetAxis("Horizontal"), theRB.velocity.y);

                if(isGrounded)
                {
                    canDoubleJump = true;
                }
                

                if(Input.GetButtonDown("Jump"))
                {
                    if(isGrounded)
                    {
                        theRB.velocity = new Vector2(theRB.velocity.x, jumpforce);
                        AudioManager.instance.PlaySFX(10);
                    }else
                    {
                        if(canDoubleJump)
                        {
                            theRB.velocity = new Vector2(theRB.velocity.x, jumpforce);
                            canDoubleJump = false;
                            AudioManager.instance.PlaySFX(10);
                        }
                    }
                }

                if(theRB.velocity.x < 0)
                {
                    theSR.flipX = true;
                }else if(theRB.velocity.x > 0)
                {
                    theSR.flipX = false;
                }
            }
            else
            {
                knockBackCounter -= Time.deltaTime;
            }

            anim.SetFloat("moveSpeed", Mathf.Abs(theRB.velocity.x));
            anim.SetBool("isGrounded", isGrounded);
        }
    }

    public void knockBack()
    {
        knockBackCounter = knockBackLenght;
        if(!theSR.flipX){
            theRB.velocity = new Vector2(-knockBackForce, knockBackForce);
        }
        else
        {
            theRB.velocity = new Vector2(knockBackForce, knockBackForce);
        }
    }

    public void bounce()
    {
        theRB.velocity = new Vector2(theRB.velocity.x, bounceForce);
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Platform")
        {
            transform.parent = other.transform;
        }
    }
    public void OnCollisionExit2D(Collision2D other)
    {
        if(other.gameObject.tag == "Platform")
        {
            transform.parent = null;
        }
    }
}
