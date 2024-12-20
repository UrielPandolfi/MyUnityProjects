using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sway : MonoBehaviour
{
    // Para crear este efecto visual no debemos hacer que el arma llegue hacia la camara de a poco, sino que hacemos que se mueva
    // hacia el lado contrario un poco, para que parezca que va retrasada.
    // Porque debemos moverlo un poco para atras y devolverlo a la posicion inicial? porque la posicion inicial es como estaria acomodado,
    // ya que este arma se encuentra dentro de la camara, asi que origin position seria la cam centrada
    // En resumen: simplemente lo movemos un poco hacia el  lado contrario y lo volvemos al centro, por eso usamos local position (la posicion acorde al padre del objeto osea la cam)
    
    public float swayClamp; // El maximo que el arma se va a separar de la mira, mientras mas alto mas se puede separar
    public float smooth;
    private Vector3 origin; // Posicion inicial del arma

    void Start()
    {
        origin = transform.localPosition;
    }
    void Update()
    {
        float inputX = Input.GetAxis("Mouse X");
        float inputY = Input.GetAxis("Mouse Y");

        inputX = Mathf.Clamp(inputX, -swayClamp, swayClamp);
        inputY = Mathf.Clamp(inputY, -swayClamp, swayClamp);

        Vector3 target = new Vector3(-inputX, -inputY, 0);

        transform.localPosition = Vector3.Slerp(transform.localPosition, origin + target, Time.deltaTime * smooth);
    }
}
