using System;
using UnityEngine;

public abstract class EnvironmentState : BaseState<EnvironmentStateManager.EnvironmentStates>
{
    // necesitamos que todos los states tengan acceso al context
    protected EnvironmentStateContext _context;

    // pasamos el contexto y el stateKey para la clase base que pide una key
    public EnvironmentState(EnvironmentStateContext context, EnvironmentStateManager.EnvironmentStates stateKey) : base(stateKey)
    {
        _context = context;
    }

    // Necesitamos una funcion que devuelva el punto mas cercano a un collider
    private Vector3 GetClosestPointToCollider(Collider other, Vector3 positionToCheck)
    {
        // Esto retorna el punto mas cercano
        return other.ClosestPoint(positionToCheck);
    }

    protected bool resetCheck()
    {
        if(_context.shouldReset)
        {
            return true;
        }

        bool isIdle = _context.characterController.velocity.magnitude < 0.1;

        bool isWalkingAway = CheckIsWalkingAway();

        bool isBadAngle = CheckBadAngle();

        Debug.Log("Idle reset: " + isIdle);
        Debug.Log("Walking away reset: " + isWalkingAway);
        Debug.Log("Bad angle reset: " + isBadAngle);

        if(isIdle || isWalkingAway || isBadAngle)
        {
            return true;
        }

        return false;
    }

    private bool CheckIsWalkingAway()
    {
        float currentDistance = Vector3.Distance(_context.currentIkConstraint.data.root.position, _context.currentClosestPoint);

        if(_context.currentCollider == null)
        {
            return false;
        }

        // Si se aleja devolvemos true, sino false
        if(currentDistance > _context.ClosestDistance + 0.05f)
        {
            return true;
        }

        if(currentDistance <= _context.ClosestDistance)
        {
            _context.ClosestDistance = currentDistance;
        }

        return false;
    }

    private bool CheckBadAngle()
    {
        if(_context.currentCollider == null)
        {
            return false;
        }

        Vector3 directionToTarget = _context.currentClosestPoint - _context.currentIkConstraint.data.root.position;
        Vector3 currentShoulderDirection = _context.currentSide == EnvironmentStateContext.side.RIGHT ? 
        _context.characterController.transform.right : -_context.characterController.transform.right;

        // Si el dot devuelve 1 estan alineados, si devuelve 0 es deferencia de 90°, si da -1 es 180° de diferencia
        float dotProduct = Vector3.Dot(currentShoulderDirection, directionToTarget);
        bool isBadAngle = dotProduct < 0;

        return isBadAngle;
    }

    // Necesitamos crear los OnTrigger para los 3 primeros states que utilizaran el IK target
    protected void StartIkTargetTracking(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Interactable") && _context.currentCollider == null)
        {
            // Seteamos todos los valores necesarios en el context para hacer el trackeo:
            _context.currentCollider = other; 
            // Aqui tomamos el punto mas cercano del centro, solo para saber que brazo usar
            Vector3 centerClosestPoint = GetClosestPointToCollider(other, _context.characterController.center);
            // Seteamos que lado vamos a usar pasandole el punto mas cercano al centro del jugador
            _context.SetCurrentSide(centerClosestPoint);
            // Guardamos el punto mas cercano al hombro actual y se lo pasamos al currentTarget
            SetIkTargetPosition();
        }
    }
    
    protected void UpdateIkTargetTracking(Collider other)
    {
        if(other == _context.currentCollider)
        {   
            SetIkTargetPosition();
        }
    }
    protected void ResetIkTargetTracking(Collider other)
    {
        if(other == _context.currentCollider)   // Si no hacemos esta condicion, al salir cualquier objeto del trigger este desactivara el objeto trackeado actual
        {
            _context.currentCollider = null;
            _context.currentClosestPoint = Vector3.positiveInfinity;
            _context.shouldReset = true;
        }
    }

    private void SetIkTargetPosition()
    {
        _context.currentClosestPoint = GetClosestPointToCollider(_context.currentCollider, new Vector3(_context.currentIkConstraint.data.root.position.x, _context.shoulderHeight,
        _context.currentIkConstraint.data.root.position.z));
        

        // Necesitamos agregar un offset para que no entre a la pared la mano
        Vector3 rayDirection = _context.currentIkConstraint.data.root.position - _context.currentClosestPoint;

        // Lo normalizamos a magnitud 1, ahora tenemos este vector 3 que apunta en la direccion de nosotros
        Vector3 normalizedRay = Vector3.Normalize(rayDirection);

        // Simplemente queda sumarlo
        float offset = 0.05f;
        Vector3 offsetPosition = _context.currentClosestPoint + normalizedRay * offset;

        _context.currentIkConstraint.data.target.position = new Vector3(offsetPosition.x, offsetPosition.y + _context.targetOffsetY, offsetPosition.z);
    }
}
