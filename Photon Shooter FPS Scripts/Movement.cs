using UnityEngine;

public class Movement : MonoBehaviour
{
    public static Movement instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }
    
    public float maxVelocityChange = 12;
    public bool sprinting;
    public bool jumping;
    public bool grounded = false;
    public float jumpHeight = 2.0f;
    public float gravity = 20.0f;
    public float speed = 6.0f;
    public float sprintSpeedMultiplier = 1.5f; // Multiplicador de velocidad para el sprint
    private Vector3 moveDirection = Vector3.zero;
    private CharacterController characterController;
    public Vector2 input;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        input.Normalize();
        sprinting = Input.GetButton("Sprint");
        jumping = Input.GetButton("Jump");
    }

    private void FixedUpdate()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        if (!GameTextChat.instance.isChatting)
        {
            if (characterController.isGrounded)
            {
                // Calcula la dirección de movimiento en el espacio global (World Space)
                Vector3 right = Camera.main.transform.right; // Derecha según la cámara
                Vector3 forward = Camera.main.transform.forward; // Adelante según la cámara
                
                // Asegúrate de que forward esté plano en el eje Y
                forward.y = 0f;
                forward.Normalize();
                right.y = 0f;
                right.Normalize();

                // Dirección de movimiento en el World Space
                moveDirection = (right * input.x + forward * input.y);

                // Aplica el multiplicador de velocidad si está corriendo
                float currentSpeed = sprinting ? speed * sprintSpeedMultiplier : speed;
                moveDirection *= currentSpeed;

                if (jumping)
                {
                    moveDirection.y = Mathf.Sqrt(2 * jumpHeight * gravity);
                }
            }

            // Aplica la gravedad
            moveDirection.y -= gravity * Time.deltaTime;

            // Mueve al jugador
            characterController.Move(moveDirection * Time.deltaTime);
        }
    }
}