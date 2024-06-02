using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteController : MonoBehaviour
{
    private SpriteSwitcher switcher;
    private Animator animator;
    private RectTransform rectTransform;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        animator = GetComponent<Animator>();
        switcher = GetComponent<SpriteSwitcher>();
    }

    public void Setup(Sprite sprite)
    {
        switcher.SetImage(sprite);
    }

    public void Show()
    {
        animator.SetBool("IsOn", true);
    }

    public void Hide()
    {
        rectTransform.anchoredPosition = new Vector2(0f, 0f);   // Cuando desaparece un personaje, se resetea su posición
        animator.SetBool("IsOn", false);
    }

    public void SetCharacterPosition(Vector2 coords)
    {
        rectTransform.anchoredPosition = coords; 
    }

    public void SwitchSprite(Sprite sprite)
    {
        if(switcher.GetImage() != sprite)   // Comprobamos que no sea igual al sprite actual, asi no hacemos el cambio de no ser necesario
        {
            switcher.SetImage(sprite);
        }
    }
}
