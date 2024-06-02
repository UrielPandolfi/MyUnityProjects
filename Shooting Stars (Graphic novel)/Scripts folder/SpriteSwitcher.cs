using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteSwitcher : MonoBehaviour
{
    public Image image1;
    public Image image2;
    private Animator animator;
    private bool isSwitched = false;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void SwitchImage(Sprite newImage)  // Función para hacer una transicion hacia otro fondo
    {
        if(!isSwitched)
        {
            image2.sprite = newImage; // Cambiamos el siguiente fondo y pasamos hacia el con la animación
            animator.SetTrigger("SwitchFirst");
        }
        else
        {
            image1.sprite = newImage;
            animator.SetTrigger("SwitchSecond");
        }
        isSwitched = !isSwitched;   // Esta es una forma facil de asignarle el valor contrario al actual
    }

    public void SetImage(Sprite newImage) // Función para cambiar el fondo directamente sin transicion, es decir cambiamos el sprite del BG actual.
    {
        if(!isSwitched)
        {
            image1.sprite = newImage; // Si no estamos en isSwitched cambiamos el actual y lo mismo si estamos en isSwitched
        }
        else
        {
            image2.sprite = newImage;
        }
    }

    public Sprite GetImage() 
    {
        if(!isSwitched)
        {
            return image1.sprite;
        }
        else
        {
            return image2.sprite;
        }
    }
}
