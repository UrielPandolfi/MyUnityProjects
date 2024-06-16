using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{   
    public static PlayerController instance;
    public float moveSpeed;
    public float jumpForce, knockbackTime, cdTime;
    public float rotateSpeed, gravity;
    public Vector2 knockbackForce;
    private Vector3 movement;
    private float zInput, hInput, yInput, knockbackCounter, cdCounter;
    private CharacterController charController;
    public Camera playerCamera;
    public Animator anim;
    public GameObject playerModel;
    public bool stopMove;
    
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        charController = GetComponent<CharacterController>();
    }

    void Update()
    {
        if(knockbackCounter <= 0f && !stopMove) //comprobamos si el jugador esta knockeado por un golpe o si no se puede mover por otra razon.
        {
            PlayerMovement();
            anim.SetBool("isHurt", false);
        }
        else if(knockbackCounter > 0f)
        {
            knockbackCounter -= Time.deltaTime;
        }
        else if(stopMove)
        {
            movement = Vector3.zero;
        }

        //Gravedad y aplicación de fuerzas de movimiento.
        yInput += gravity * Time.deltaTime;
        yInput = Mathf.Clamp(yInput, -5, yInput);
        movement.y = yInput;
        charController.Move(movement * moveSpeed * Time.deltaTime);

        //Pasamos los parametros al animator.
        anim.SetFloat("Speed", Mathf.Abs(movement.x) + Mathf.Abs(movement.z));
        anim.SetBool("isGrounded", charController.isGrounded);
    }

    private void PlayerMovement()
    {
        //Si estamos en cooldown no nos podemos mover
        if(cdCounter <= 0)
        {
            //Entrada de controles.
            hInput = Input.GetAxisRaw("Horizontal");
            zInput = Input.GetAxisRaw("Vertical");
            movement = (transform.forward * zInput) + (transform.right * hInput);
            if (charController.isGrounded)
            {
                if (Input.GetButtonDown("Jump"))
                {
                    yInput = jumpForce;
                }
            }

            //Rotacion de jugador con la camara.
            if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
            {
                transform.rotation = Quaternion.Euler(0f, playerCamera.transform.rotation.eulerAngles.y, 0f);
                Quaternion newRotation = Quaternion.LookRotation(new Vector3(movement.x, 0f, movement.z));
                playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
            }

            //Golpe del jugador.
            if(Input.GetKeyDown(KeyCode.Mouse0) && charController.isGrounded)
            {
                movement = (transform.forward * 0f) + (transform.right * 0f);
                anim.SetTrigger("Hit");
                //Aplicamos el cooldown
                cdCounter = cdTime;
            }
        }
        else
        {
            cdCounter -= Time.deltaTime;
        }
    }

    public void Knockback()
    {
        knockbackCounter = knockbackTime;
        anim.SetBool("isHurt", true);
        movement = (transform.forward * -knockbackForce.x);
        yInput = knockbackForce.y;
    }
}
