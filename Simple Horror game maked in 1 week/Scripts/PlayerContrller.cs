using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerContrller : MonoBehaviour
{
    
    #region Singleton
    public static PlayerContrller instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    [Header("Movement")]
    public float walkSpeed;
    public float sprintSpeed;
    public float jumpForce;
    public Animator anim;
    private float moveSpeed, xMove, zMove;
    private bool isMoving, isRunning;
    private CharacterController charController;
    private Vector3 movement;
    private float baseGravity = -9.81f, fallMultiplier = 2.5f;
    private float xInput, yInput, zInput;
    public bool canMove = true;
    public Transform raycastOrigin;
    public LayerMask interactableMask;
    
    
    void Start()
    {
        charController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove)
            Movement();

        if(Input.GetKeyDown(KeyCode.E))
        {
            Interact();
        }
    }

    void Movement()
    {
         //Le damos valor a las variables que vamos a usar para el movimiento y para saber si el jugador se esta moviendo o no.
        xInput = Input.GetAxisRaw("Horizontal");
        zInput = Input.GetAxisRaw("Vertical");

        xMove = Mathf.Lerp(xMove, xInput, 6f * Time.deltaTime);
        zMove = Mathf.Lerp(zMove, zInput, 6f * Time.deltaTime);


        isMoving = zInput != 0f || xInput != 0f;
        isRunning = Input.GetKey(KeyCode.LeftShift) && zInput > 0f;

        if(isMoving && !isRunning) //Caminando
        {
            moveSpeed = walkSpeed;
            transform.rotation = Quaternion.Euler(0f, CameraMove.instance.transform.rotation.eulerAngles.y, 0f);
        }
        else if(isMoving && isRunning) //Corriendo
        {
            moveSpeed = sprintSpeed;
            transform.rotation = Quaternion.Euler(0f, CameraMove.instance.transform.rotation.eulerAngles.y, 0f);
        }


        if(Input.GetKeyDown(KeyCode.Space) && charController.isGrounded) //Salto
        {
            Jump();
        }

        //Aplicamos gravedad si no estamos en el suelo.
        if(!charController.isGrounded)
        {
            anim.SetBool("OnGround", false);
            Gravity();
        }
        else
        {
            anim.SetBool("OnGround", true);
        }

        anim.SetFloat("xInput", xMove);
        anim.SetFloat("zInput", zMove);
        anim.SetBool("IsRunning", isRunning);
        
        //hacemos los dos vectores, el del salto y el del movimiento.
        Vector3 jumpVector = new Vector3(0f, yInput, 0f);
        movement = (transform.forward * zInput) + (transform.right * xInput);

        //Pasamos el movimiento en lineas diferentes asi no se multiplica el salto por la velocidad de movimiento.
        charController.Move(movement * moveSpeed * Time.deltaTime);
        charController.Move(jumpVector * Time.deltaTime);
    }

    private void Jump()
    {
        yInput = jumpForce;
    }

    private void Gravity()
    {
        if(yInput > baseGravity)
            {
                yInput += baseGravity * Time.deltaTime;
            }
            else
            {
                yInput += baseGravity * fallMultiplier * Time.deltaTime;
            }
    }

    private void Interact()
    {
        /* Hacer que cuando apretas la E interactue con el objeto que tiene en frente, a travez de un raycast */
        RaycastHit hit;
        bool hitSomething = Physics.Raycast(raycastOrigin.position, raycastOrigin.forward, out hit, 100f, interactableMask);
        if(hitSomething)
        {
            if(hit.collider.tag == "Item")
            {
                ItemPickup itemPickup = hit.collider.gameObject.GetComponent<ItemPickup>();
                itemPickup.Pickup();
            }
            Debug.Log("interact with: " + hit.collider.name);
        }
    }
}
