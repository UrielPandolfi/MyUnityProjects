using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    [Header("Animaciones FPS")]
    private Animator animator;
    private CharacterController characterController;
    private float oldValue;
    
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float movementValue = Mathf.Lerp(oldValue, characterController.velocity.magnitude, Time.deltaTime * 10f);
        animator.SetFloat("MovementSpeed", movementValue);
        oldValue = movementValue;
    }
}