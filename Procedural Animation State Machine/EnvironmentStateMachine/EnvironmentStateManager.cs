using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Assertions;
using UnityEngine.TextCore.Text;

public class EnvironmentStateManager : StateManager<EnvironmentStateManager.EnvironmentStates>
{
    private EnvironmentStateContext _context;

    public enum EnvironmentStates
    {
        Search,
        Approach,
        Rise,
        Touch,
        Reset
    }

    void Awake()
    {
        ValidateConstraints();
        _context = new EnvironmentStateContext(leftArmIkConstraint, rightArmIkConstraint, leftArmMultiRotationConstraint,
        rightArmMultiRotationConstraint, characterController, rightArmIkConstraint.data.root.position.y, characterController.center.y);
        InitializeStates();

        CreateCollider();
    }

    public TwoBoneIKConstraint leftArmIkConstraint;
    public TwoBoneIKConstraint rightArmIkConstraint;
    public MultiRotationConstraint leftArmMultiRotationConstraint;
    public MultiRotationConstraint rightArmMultiRotationConstraint;
    public CharacterController characterController;

    void ValidateConstraints()
    {
        Assert.IsNotNull(leftArmIkConstraint, "leftArmIkConstraint is not assigned");
        Assert.IsNotNull(rightArmIkConstraint, "rightArmIkConstraint is not assigned");
        Assert.IsNotNull(leftArmMultiRotationConstraint, "leftArmMultiRotationConstraint is not assigned");
        Assert.IsNotNull(rightArmMultiRotationConstraint, "rightArmMultiRotationConstraint is not assigned");
        Assert.IsNotNull(characterController, "characterController is not assigned");
    }

    void InitializeStates() // instanciamos todos los states y les pasamos elcontexto y a traves del segundo parametro le generamos una key
    {     
        states.Add(EnvironmentStates.Search, new EnvironmentSearchState(_context, EnvironmentStates.Search));
        states.Add(EnvironmentStates.Approach, new EnvironmentApproachState(_context, EnvironmentStates.Approach));
        states.Add(EnvironmentStates.Rise, new EnvironmentRiseState(_context, EnvironmentStates.Rise));
        states.Add(EnvironmentStates.Reset, new EnvironmentResetState(_context, EnvironmentStates.Reset));
        currentState = states[EnvironmentStates.Reset];
    }

    void CreateCollider()
    {
        BoxCollider boxCollider = gameObject.AddComponent<BoxCollider>();
        // Hacemos del collider del tamaño de la altura del jugador
        boxCollider.size = new Vector3(characterController.height, characterController.height, characterController.height * 1.2f);

        // Haccemos que este un poco por arriba del centro del jugador
        boxCollider.center = new Vector3(characterController.center.x, characterController.center.y, characterController.center.z + characterController.height * 0.5f);

        // Lo hacemos trigger
        boxCollider.isTrigger = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        if(_context != null && _context.currentClosestPoint != null)
        {
            Gizmos.DrawSphere(_context.currentClosestPoint, 0.03f);
        }
    }
}